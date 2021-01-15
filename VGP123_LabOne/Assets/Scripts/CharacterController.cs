using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float playerSpeed = 10;
    public float jumpForce = 10;
    [SerializeField] Transform raycastOrigin;
    // bool to check if player is grounded
    public bool isGrounded;
    // variable to cache the player's initial scale, just in case its differeny from 1,1,1
    private Vector3 initialScale;
    // Use this for initialization
    void Start()
    {
        // Cache the scale
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // If right arrow is "held down"
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // move character to right, deltaTime is the time between frame updates
            GetComponent<Rigidbody2D>().velocity = new Vector2(playerSpeed * Vector3.right.x * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            // flip character
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        // If left arrow is "held down"
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            // move character to left, deltaTime is the time between frame 
            GetComponent<Rigidbody2D>().velocity = new Vector2(playerSpeed * Vector3.left.x * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
            // change the scale back to intial so character faces the right direction
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }

        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Commented this, this is one way of checking grounded
        //isGrounded = (GetComponent<Rigidbody2D>().velocity.y == 0);

        // if up arrow is pressed and player is grounded
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            // Add vertical jump force
            GetComponent<Rigidbody2D>().AddForce(jumpForce * Vector3.up, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 8 is ground layer
        if (collision.gameObject.layer == 8)
        {
            isGrounded = true;
        }

        // 10 is enemy layer
        if (collision.collider.gameObject.layer == 10 && !isGrounded)
        {
            Destroy(collision.rigidbody.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 8 is ground layer
        if (collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }
}
