using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpPeriod = .75f;

    [Header("Ground Collision Properties")]
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Transform groundCheck = null;
    [SerializeField] float groundCheckLength = .1f;

    [Header("Wall Collision Properties")]
    [SerializeField] Transform wallCheck = null;
    [SerializeField] float wallCheckWidth = .25f;
    [SerializeField] float wallCheckLength = 1f;
    [SerializeField] float fallSpeedMax = -2f;
    [SerializeField] float wallJumpPeriod = .1f;
    Rigidbody2D rb;
    Vector3 ogScale;
    float ogMoveSpeed;
    Animator animator;
    float jumpTimer;
    float wallJumpTimer;
    bool isGrounded = true;
    bool isOnWall = false;
    bool isJumping = false;
    bool wallJumped = false;
    bool prevIsOnWall = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ogScale = transform.localScale;
        ogMoveSpeed = moveSpeed;
        jumpTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.Raycast(groundCheck.position, transform.up * -1, groundCheckLength, GroundLayer);
        isOnWall = Physics2D.OverlapBox(wallCheck.position, new Vector2(wallCheckWidth, wallCheckLength), 0, GroundLayer);

        if (isOnWall)
        {
            if (!isGrounded)
            {
                if (!prevIsOnWall)
                {
                    // reduce y-velocity to zero on wall impact
                    animator.SetTrigger("justHitWall");
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                }

                //clamp fall speed
                if (rb.velocity.y < fallSpeedMax)
                {
                    rb.velocity = new Vector2(rb.velocity.x, fallSpeedMax);
                }


                if (Input.GetButtonDown("Jump"))
                {

                    wallJumped = true;
                    animator.SetTrigger("justJumped");
                    //isJumping = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    Debug.Log(new Vector2(jumpForce / 3 * transform.localScale.x * -1, jumpForce / 3));
                    rb.AddForce(new Vector2(jumpForce / 3 * transform.localScale.x * -1, jumpForce / 3), ForceMode2D.Impulse);

                }
            }
            else if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("justJumped");
                    //isJumping = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
                }
            }

            //stop the player from trying to walk into the wall if they're not stationary
            if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(hAxis))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("isWalking", false);
            }
            else
            {

            }
        }
        else if (!isOnWall)
        {
            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("justJumped");
                    //isJumping = true;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
                }
            }
        }

        if (isGrounded || isOnWall)
        {
            isJumping = false;
            jumpTimer = 0f;
            wallJumpTimer = 0f;
        }

        if (isGrounded)
        {

            wallJumped = false;
        }
        else if (!isGrounded && !isOnWall)
        {
            isJumping = true;
        }



        CheckJump();
        CheckHorizontalAxis(hAxis);
        CheckFire();

        animator.SetBool("isOnWall", isOnWall);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        prevIsOnWall = isOnWall;
    }

    void CheckJump()
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
            else if (!isGrounded && wallJumped)
            {

                wallJumpTimer += Time.deltaTime;
                if (wallJumpTimer < wallJumpPeriod)
                {
                    //rb.AddForce(new Vector2(jumpForce * transform.localScale.x * -1 * Time.deltaTime, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
                }
            }

        }

        if (Input.GetButtonUp("Jump") && jumpTimer < jumpPeriod)
        {
            if (jumpTimer < jumpPeriod) { jumpTimer = jumpPeriod; }
            if (wallJumpTimer < wallJumpPeriod)
            {
                wallJumpTimer = wallJumpPeriod;
                wallJumped = false;
            }
        }

    }

    void CheckHorizontalAxis(float hAxis)
    {
        if (hAxis != 0)
        {
            moveSpeed = ogMoveSpeed;
            if (!isOnWall)
            {
                //if (!wallJumped)
                rb.velocity = new Vector2(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y);
                //else if (wallJumped && wallJumpTimer < wallJumpPeriod)
                //{
                //    var clampedHAxis = Mathf.Clamp(hAxis, -0.25f, 0.25f);
                //    rb.velocity = new Vector2(clampedHAxis * moveSpeed * Time.deltaTime, rb.velocity.y);
                //}
            }
            animator.SetBool("isWalking", true);


            if (hAxis < 0)
            {
                transform.localScale = new Vector3(-ogScale.x, ogScale.y, ogScale.z);
            }
            else if (hAxis > 0)
            {
                transform.localScale = ogScale;
            }
        }

        if (hAxis == 0)
        {
            if (!isOnWall && !wallJumped)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            animator.SetBool("isWalking", false);
        }

    }
    void CheckFire()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isShooting", true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("isShooting", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(wallCheck.position, new Vector3(wallCheckWidth, wallCheckLength, wallCheckWidth));
    }
}
