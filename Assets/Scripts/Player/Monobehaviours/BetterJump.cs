using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private CustomGravity _gravity;
    private IJumpInput _jumpInput;

    public float fallMutltiplier = 5f;
    public float lowJumpMultiplier = 10f;

    private void Awake()
    {
        _jumpInput = GetComponent<IJumpInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _gravity = GetComponent<CustomGravity>();
    }

    void FixedUpdate()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _gravity.gravityScale = fallMutltiplier;
        }
        else if (_rigidbody.velocity.y > 0)
        {
            _gravity.gravityScale = lowJumpMultiplier;
        }
    }
}
