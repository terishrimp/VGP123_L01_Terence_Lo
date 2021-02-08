using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpPeriod = .75f;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckLength = .1f;
    Rigidbody2D rb;
    Vector3 ogScale;
    Animator animator;
    float jumpTimer;
    bool isGrounded;
    bool isOnWall;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ogScale = transform.localScale;
        jumpTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        /*add Overlap to see if player is near wall
          if next to wall, cause player friction to be smaller, allowing them to slide
        allow wall-jumping
         */
        //isOnWall = Physics2D.OverlapBox()
        isGrounded = Physics2D.Raycast(groundCheck.position, transform.up * -1, groundCheckLength, GroundLayer);
        Debug.DrawLine(groundCheck.position, groundCheck.position + (transform.up * -1 * groundCheckLength), Color.red);
        float hAxis = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector3(0, jumpForce / 3), ForceMode2D.Impulse);
        }
        if (Input.GetButton("Jump"))
        {
            if (isGrounded)
            {
                animator.SetTrigger("justJumped");
            }
            animator.SetBool("isJumping", true);
            jumpTimer += Time.deltaTime;
            if(jumpTimer < jumpPeriod)
            {
                rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
            }
        }
        else if (Input.GetButtonUp("Jump") && jumpTimer < jumpPeriod)
        {
            jumpTimer = jumpPeriod;
        }
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            jumpTimer = 0f;
        }
        if (hAxis != 0)
        {
            rb.velocity = new Vector3(hAxis * moveSpeed * Time.deltaTime, rb.velocity.y, 0f);
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
        else if (hAxis ==0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0f);
            animator.SetBool("isWalking", false);
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
    }
}
