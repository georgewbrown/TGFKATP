using System;
using System.Collections;
using UnityEngine;

public class InputMoveCommand : Command
{

    private Rigidbody _rigidbody;
    private IMoveInput _move;
    private Coroutine _moveCoroutine;
    private Transform _transform;
    private JumpCommand _jumpCommand;
    private GrabCommand _grabCommand;

    public float movementSpeed;
    public float wallJumpLerp = 10;

    private void Awake()
    {
        _jumpCommand = GetComponent<JumpCommand>();
        _grabCommand = GetComponent<GrabCommand>();
        _rigidbody = GetComponent<Rigidbody>();
        _move = GetComponent<IMoveInput>();
        _transform = transform;
    }

    public override void Execute()
    {
        if (_moveCoroutine == null)
            _moveCoroutine = StartCoroutine(Move());

    }
    
    private IEnumerator Move()
    {
        if (!_jumpCommand.canMove)
            yield return null;

        if (_grabCommand.wallGrab)
            yield return null;
        // Jitter Problem because ??? we can resolve it be not taking inputs from ranges between 0f - 1f for the y axis
        // we could remove the Up binding but then players can't jump or dash in that direction...
        while (_move.MoveDirection != Vector3.zero || _move.MoveDirection != Vector3.up || _move.MoveDirection != Vector3.down)
        {
            if (!_jumpCommand.wallJumped)
            {
                _rigidbody.MovePosition(_transform.position + _move.MoveDirection * (Time.fixedDeltaTime * movementSpeed));
            }
            else
            {
                _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, (new Vector3(_move.MoveDirection.x * movementSpeed, _rigidbody.velocity.y, 0)), wallJumpLerp * Time.fixedDeltaTime);
            }

            yield return null;
        }

        _moveCoroutine = null;
    }



}
