using System;
using System.Collections;
using UnityEngine;

public class JumpCommand : Command
{

    private Rigidbody _rigidbody;
    private Collision _collision;
    private IJumpInput _jumpInput;

    public float jumpVelocity;
    public bool wallJumped;
    public bool canMove;


    private void Awake()
    {
        canMove = true;
        _jumpInput = GetComponent<IJumpInput>();
        _collision = GetComponent<Collision>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Execute()
    {
        if (_collision.onGround)
            Jump(Vector3.up);
        if (_collision.onWall && !_collision.onGround)
            WallJump();
    }
    
    private void Jump(Vector3 direction)
    {
        _rigidbody.AddForce((direction * jumpVelocity), ForceMode.Impulse);
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
    
        Vector3 wallDir = _collision.onRightWall ? Vector3.left : Vector3.right;
    
        Jump((Vector3.up / 1.5f + wallDir / 1.5f));
    
        wallJumped = true;
    }

    // Could probably move this into it's own class
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }


}