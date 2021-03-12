using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpPeriod = .75f;

    [Header("Ground Collision Properties")]
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Transform groundCheck = null;
    [SerializeField] float groundCheckRadius = .1f;

    [Header("Wall Collision Properties")]
    [SerializeField] Transform wallCheck = null;
    [SerializeField] float wallCheckWidth = .25f;
    [SerializeField] float wallCheckLength = 1f;
    [SerializeField] float fallSpeedMax = -2f;
    [SerializeField] float wallJumpPeriod = .1f;

    Rigidbody2D rb;
    Vector3 ogScale;
    float ogMoveSpeed;
    Animator anim;
    float jumpTimer;
    float wallJumpTimer;
    public bool movementEnabled = true;
    public bool MovementEnabled
    {
        get { return movementEnabled; }
        set
        {
            if (value == movementEnabled) return;
            movementEnabled = value;
        }
    }
    bool isGrounded = true;
    bool isOnWall = false;
    public bool IsOnWall
    {
        get { return isOnWall; }
        set
        {
            if (value == isOnWall) return;
            if (value)
            {
                // reduce y-velocity to zero on wall impact
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                anim.SetTrigger("justHitWall");
            }
            isOnWall = value;
        }
    }
    bool isJumping = false;
    bool isWalking = false;
    public bool wallJumped = false;
    const string animJustJumpedString = "justJumped";
    const string animWalkingString = "isWalking";
    const string animGroundedString = "isGrounded";
    const string animJumpingString = "isJumping";
    const string animOnWallString = "isOnWall";
    const string animYVelocityString = "yVelocity";
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ogScale = transform.localScale;
        ogMoveSpeed = moveSpeed;
        jumpTimer = 0f;
        GameManager.instance.PauseChange += OnPauseChange;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, GroundLayer);
        IsOnWall = Physics2D.OverlapBox(wallCheck.position, new Vector2(wallCheckWidth, wallCheckLength), 0, GroundLayer);

        if (movementEnabled)
        {
            if (IsOnWall)
            {
                if (!isGrounded)
                {
                    //clamp fall speed
                    if (rb.velocity.y < fallSpeedMax) rb.velocity = new Vector2(rb.velocity.x, fallSpeedMax);

                    if (Input.GetButtonDown("Jump"))
                    {
                        wallJumped = true;
                        HelperFunctions.AnimTrigger(anim, animJustJumpedString);
                        rb.velocity = new Vector2(0, 0);
                        rb.AddForce(new Vector2(jumpForce / 3 * transform.localScale.x * -1, jumpForce / 2), ForceMode2D.Impulse);
                    }
                }
                else if (isGrounded)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        HelperFunctions.AnimTrigger(anim, animJustJumpedString);
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
                    }
                }
            }
            else if (!IsOnWall)
            {
                if (isGrounded)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        HelperFunctions.AnimTrigger(anim, animJustJumpedString);
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
                    }
                }
                else if (!isGrounded)
                {
                    isJumping = true;
                }
            }


            if (isGrounded || IsOnWall)
            {
                isJumping = false;
                jumpTimer = 0f;
                wallJumpTimer = 0f;
            }

            if (isGrounded)
            {
                wallJumped = false;
            }

            CheckJump();
            CheckHorizontalAxis(hAxis);

            anim.SetBool(animOnWallString, IsOnWall);
            anim.SetBool(animJumpingString, isJumping);
            anim.SetBool(animGroundedString, isGrounded);
            anim.SetBool(animWalkingString, isWalking);
            anim.SetFloat(animYVelocityString, rb.velocity.y);
        }

    }

    void CheckJump()
    {
        if (movementEnabled)
        {
            if (Input.GetButton("Jump"))
            {

                if (!isGrounded && !wallJumped)
                {

                    jumpTimer += Time.deltaTime;
                    if (jumpTimer < jumpPeriod)
                    {
                        rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
                    }
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                if (jumpTimer < jumpPeriod) { jumpTimer = jumpPeriod; }
                if (wallJumpTimer < wallJumpPeriod)
                {
                    wallJumpTimer = wallJumpPeriod;
                    wallJumped = false;
                }
            }
        }

    }

    void CheckHorizontalAxis(float hAxis)
    {
        if (movementEnabled)
        {
            if (hAxis != 0)
            {
                moveSpeed = ogMoveSpeed;
                if (!IsOnWall)
                {
                    if (!wallJumped)
                        rb.velocity = new Vector2(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y);
                    else if (wallJumped && wallJumpTimer < wallJumpPeriod)
                    {
                        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y), 10f * Time.deltaTime);
                    }
                }
                isWalking = true;


                if (hAxis < 0)
                {
                    transform.localScale = new Vector3(-ogScale.x, ogScale.y, ogScale.z);
                }
                else if (hAxis > 0)
                {
                    transform.localScale = ogScale;
                }
            }
            else
            {
                if (!IsOnWall && !wallJumped && movementEnabled)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                isWalking = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheck.position, new Vector3(wallCheckWidth, wallCheckLength, wallCheckWidth));

        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnPauseChange(object sender, bool isPaused)
    {
        if (isPaused)
        {
            movementEnabled = false;
        }
        else movementEnabled = true;
    }

    private void OnDestroy()
    {
        GameManager.instance.PauseChange -= OnPauseChange;
    }
}
