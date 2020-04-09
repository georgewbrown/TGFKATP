using System.Collections;
using UnityEngine;

public class GrabCommand : Command
{
    private Transform _transform;
    private Rigidbody _rigidbody;
    private DashCommand _dashCommand;
    private JumpCommand _jumpCommand;
    private IMoveInput _moveInput;
    private IGrabInput _grabInput;
    private Collision _collision;
    private CustomGravity _gravity;
    
    [Space]
    [Header("Conditions")]
    public bool wallGrab;
    public bool wallSlide;
    public float slideSpeed = 5f;

    private void Awake()
    {
        _dashCommand = GetComponent<DashCommand>();
        _jumpCommand = GetComponent<JumpCommand>();
        _gravity = GetComponent<CustomGravity>();
        _collision = GetComponent<Collision>();
        _rigidbody = GetComponent<Rigidbody>();
        _moveInput = GetComponent<IMoveInput>();
        _grabInput = GetComponent<IGrabInput>();
        _transform = transform;
    }

    public override void Execute()
    {
        // Note sure if I need this anymore
        if (_collision.onWall && !_collision.onGround)
        {
            if (_moveInput.MoveDirection.x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }
    }

    private void Update()
    {
        if (_collision.onWall && _grabInput.IsPressingGrab && _jumpCommand.canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }
    
    
        if (wallGrab && !_dashCommand.isDashing)
        {
            _gravity.gravityScale = 2f;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, 0);
    
        }
        else
        {
            _gravity.gravityScale = 5f;
        }
    
        if (_grabInput.IsPressingGrab || !_collision.onWall || !_jumpCommand.canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }
    
        if (wallGrab || wallSlide || !_jumpCommand.canMove)
        {
            return;
        }
    }

    private void WallSlide()
    {
        if (!_jumpCommand.canMove)
            return;
        
        bool pushingWall = (_rigidbody.velocity.x > 0 && _collision.onRightWall) || (_rigidbody.velocity.x < 0 && _collision.onLeftWall);
        float push = pushingWall ? 0 : _rigidbody.velocity.x;
        _rigidbody.velocity = new Vector3(push, -slideSpeed, 0);
    }



}