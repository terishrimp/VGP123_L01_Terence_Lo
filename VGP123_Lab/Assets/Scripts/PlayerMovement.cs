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
    [SerializeField] Collider2D[] groundCollisions;
    [SerializeField] Transform groundCheck = null;
    [SerializeField] float groundCheckLength = .1f;

    [Header("Wall Collision Properties")]
    [SerializeField] Transform wallCheck = null;
    [SerializeField] float wallGravityScale = .5f;
    [SerializeField] float wallCheckWidth = .25f;
    [SerializeField] float wallCheckLength = 1f;
    [SerializeField] float wallFriction = 0f;
    Rigidbody2D rb;
    Vector3 ogScale;
    float ogMoveSpeed;
    float ogFriction;
    float ogGravity;
    Animator animator;
    float jumpTimer;
    bool isGrounded;
    bool isOnWall;
    bool prevIsOnWall;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ogGravity = rb.gravityScale;
        ogFriction = rb.sharedMaterial.friction;
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
        if (isOnWall && !isGrounded)
        {
            //if (!Input.GetButton("Jump")) { 
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            //}
            wallCheck.GetComponent<SpriteRenderer>().color = Color.green;
            rb.gravityScale = wallGravityScale;
            rb.sharedMaterial.friction = wallFriction;
            foreach (var groundCollision in groundCollisions)
            {
                groundCollision.sharedMaterial.friction = 0;
            }
        }
        else
        {
            wallCheck.GetComponent<SpriteRenderer>().color = Color.red;
            rb.gravityScale = ogGravity;
            rb.sharedMaterial.friction = ogFriction;
            foreach (var groundCollision in groundCollisions)
            {
                groundCollision.sharedMaterial.friction = 1f;
            }

        }
        Debug.DrawLine(groundCheck.position, groundCheck.position + (transform.up * -1 * groundCheckLength), Color.red);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
        }
        if (Input.GetButtonDown("Jump") && !isGrounded && isOnWall)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(jumpForce/3 * transform.localScale.x * -1, jumpForce/3), ForceMode2D.Impulse);
        }
        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                animator.SetTrigger("justJumped");
            }
            else if (isOnWall)
            {
                //
            }
            animator.SetBool("isJumping", true);
            jumpTimer += Time.deltaTime;
            if (jumpTimer < jumpPeriod && !isOnWall)
            {
                rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
            }
        }
        else if (Input.GetButtonUp("Jump") && jumpTimer < jumpPeriod)
        {
            jumpTimer = jumpPeriod;
        }

        if (isGrounded || isOnWall)
        {
            animator.SetBool("isJumping", false);
            jumpTimer = 0f;
        }


        if (hAxis != 0 && !isOnWall)
        {
            moveSpeed = ogMoveSpeed;
            rb.velocity = new Vector2(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y);
            animator.SetBool("isWalking", true);
            if (hAxis < 0)
            {
                transform.localScale = new Vector3(-ogScale.x, ogScale.y, ogScale.z);
            }
            else
            {
                transform.localScale = ogScale;
            }
        }
        else if (hAxis == 0 & !Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isWalking", false);
        }
        else if (isOnWall)
        {

            if (!prevIsOnWall)
            {
                moveSpeed = ogMoveSpeed / 4;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            if (Mathf.Sign(transform.localScale.x) == -1 && hAxis < 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("isWalking", false);
            }
            else if (Mathf.Sign(transform.localScale.x) == 1 && hAxis > 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("isWalking", false);
            }
            else if (!Input.GetButton("Jump"))
            {
                rb.velocity = new Vector2(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isShooting", true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("isShooting", false);
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        prevIsOnWall = isOnWall;
    }
}
