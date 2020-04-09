using System.Collections;
using UnityEngine;

public class DashCommand : Command
{
    private Collision _collision;
    private Rigidbody _rigidbody;
    private CustomGravity _gravity;
    private IMoveInput _move;
    private Transform _transform;
    private BetterJump _betterJump;
    private JumpCommand _jumpCommand;


    [Space]
    [Header("Conditions")]
    public bool _hasDashed;
    public float _dashSpeed;
    public bool groundTouch;
    public bool isDashing;

    private void Awake()
    {
        _collision = GetComponent<Collision>();
        _rigidbody = GetComponent<Rigidbody>();
        _gravity = GetComponent<CustomGravity>();
        _move = GetComponent<IMoveInput>();
        _jumpCommand = GetComponent<JumpCommand>();
        _betterJump = GetComponent<BetterJump>();
        _transform = transform;
    }

    public override void Execute()
    {
        if (!_hasDashed)
            Dash(_move.MoveDirection.x, _move.MoveDirection.y);
    }

    private void FixedUpdate()
    {
        if (_collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (_collision.onGround && !isDashing)
        {
            _jumpCommand.wallJumped = false;
            _betterJump.enabled = true;
        }

        if (!_collision.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }

    void GroundTouch()
    {
        _hasDashed = false;
        isDashing = false;
    }

    private void Dash(float x, float y)
    {
        _hasDashed = true;

        _rigidbody.velocity = Vector3.zero;
        Vector3 direction = new Vector3(x, y, 0);

        _rigidbody.velocity += direction.normalized * _dashSpeed;
        StartCoroutine(DashWait());
    }


    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());
        _gravity.gravityScale = 4;
        _betterJump.enabled = false;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        _gravity.gravityScale = 6;
        _betterJump.enabled = true;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (_collision.onGround)
            _hasDashed = false;
    }

}