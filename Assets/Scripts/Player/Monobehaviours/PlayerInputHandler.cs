using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IMoveInput, IGrabInput, IDashInput, IJumpInput
{
    public Command movementInput;
    public Command grabInput;
    public Command dashInput;
    public Command jumpInput;
    
    public PlayerInputActions _inputActions;
    public Vector3 MoveDirection { get; private set; }
    public bool IsPressingGrab { get; private set; }
    public bool IsPressingJump { get; private set; }
    public bool IsPressingDash { get; private set; }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        if (movementInput)
            _inputActions.Player.Movement.performed += OnMoveInput;

        if (grabInput)
            _inputActions.Player.Grab.performed += OnGrabButton;

        if (dashInput)
            _inputActions.Player.Dash.performed += OnDashButton;

        if (jumpInput)
            _inputActions.Player.Jump.performed += OnJumpButton;
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        MoveDirection = new Vector3(value.x, value.y, 0);

        if (movementInput != null)
            movementInput.Execute();
    }

    private void OnGrabButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingGrab = value >= 0.15f;

        if (grabInput != null && IsPressingGrab)
            grabInput.Execute();
    }

    private void OnDashButton(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingDash = value >= 0.15f;

        if (dashInput != null)
            dashInput.Execute();
    }

    private void OnJumpButton(InputAction.CallbackContext context)
    {       
        var value = context.ReadValue<float>();
        IsPressingJump = value >= 0.15f;

        if (jumpInput != null && IsPressingJump)
            jumpInput.Execute();
    }
    
    private void OnDisable()
    {
        if (movementInput)
            _inputActions.Player.Movement.performed -= OnMoveInput;

        if (grabInput)
            _inputActions.Player.Grab.performed -= OnGrabButton;

        if (dashInput)
            _inputActions.Player.Dash.performed -= OnDashButton;

        if (jumpInput)
            _inputActions.Player.Jump.performed -= OnJumpButton;

        _inputActions.Disable();
    }
}
