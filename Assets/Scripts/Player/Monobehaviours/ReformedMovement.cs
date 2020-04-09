using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReformedMovement : MonoBehaviour
{
    private Collision collision;
    private Rigidbody2D rb;
    private Camera m_MainCamera;


    [Space]
    [Header("Stats")]
    public float speed = 12;
    public float jumpVelocity = 50;
    public float dashSpeed = 20;
    public float wallJumpLerp = 10;
    public float slideSpeed = 5f;


    [Space]
    [Header("Conditions")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    private bool groundTouch;
    private bool hasDashed;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        m_MainCamera = Camera.main;
        canMove = true;

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y);

        Walk(direction);

        if (Input.GetButtonDown("Jump"))
        {
            if (collision.onGround)
                Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround)
                WallJump();
        }

        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.F)) && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }


        if (collision.onWall && (Input.GetButton("Fire3") || Input.GetKeyDown(KeyCode.LeftShift)) && canMove)
        {
            Debug.Log("ksdfgbdsfdjkgb");
            wallGrab = true;
            wallSlide = false;

        }

        if ((Input.GetButton("Fire3") || Input.GetKeyDown(KeyCode.LeftShift)) || !collision.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (wallGrab && !isDashing)
        {
            slideSpeed = 0.2f;
            //rb.gravityScale = 0;
            //if (x > .2f || x < -.2f)
            //    rb.velocity = new Vector2(rb.velocity.x, 0);

            //float speedModifier = y > 0 ? .5f : 1;
            //rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            slideSpeed = 5f;
            //rb.gravityScale = 3;
        }

        if (collision.onWall && !collision.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJump>().enabled = true;
        }

        if (collision.onWall && !collision.onGround)
        {
            WallSlide();
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (wallGrab || wallSlide || !canMove)
            return;


    }

    private void FixedUpdate()
    {
        m_MainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
    }

    private void Walk(Vector2 direction)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(direction.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        Debug.Log("adfs");
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpVelocity;
    }

    private void WallSlide()
    {
        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && collision.onRightWall) || (rb.velocity.x < 0 && collision.onLeftWall))
        {
            pushingWall = true;
        }

        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }


    private void Dash(float x, float y)
    {
        hasDashed = true;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        rb.gravityScale = 0;
        GetComponent<BetterJump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 3;
        GetComponent<BetterJump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (collision.onGround)
            hasDashed = false;
    }
}
