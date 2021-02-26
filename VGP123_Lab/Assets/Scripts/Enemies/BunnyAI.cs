using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class BunnyAI : MonoBehaviour
{
    [SerializeField] Collider2D upperBodyCollider = null;
    [SerializeField] Vector2 jumpForce = new Vector2(1, 1);
    [SerializeField] float idlePeriod = 1f;

    [SerializeField] float canMoveCheckRadius = 4f;
    public int health = 2;

    [Header("Ground Raycast")]
    [SerializeField] Transform groundRaycastOrigin = null;
    [SerializeField] float groundRaycastLength = .5f;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Player Detection Raycast")]
    [SerializeField] Transform playerRaycastOrigin = null;
    [SerializeField] float playerRaycastLength = 1f;

    [Header("Projectile Properties")]
    [SerializeField] EnemyProjectile shotPrefab = null;
    Animator anim;
    Rigidbody2D rb;
    bool canMove = false;
    bool isGrounded = true;
    bool isShooting = false;
    float idleTimer;
    // Start is called before the first frame update
    void Start()
    {
        idleTimer = idlePeriod;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] canMoveCheckColliderList = Physics2D.OverlapCircleAll(transform.position, canMoveCheckRadius);
        RaycastHit2D groundHitInfo = Physics2D.Raycast(groundRaycastOrigin.position, transform.up * -1, groundRaycastLength, groundLayerMask);
        if (canMove)
        {
            if (groundHitInfo)
            {
                if (!isGrounded) isGrounded = true;
                if (idleTimer <= idlePeriod && !isShooting) idleTimer += Time.deltaTime;
                Debug.DrawLine(groundRaycastOrigin.position, groundRaycastOrigin.position + transform.up * -1 * groundRaycastLength, Color.green);
                //jump
                if (idleTimer > idlePeriod)
                {
                    anim.SetTrigger("jumped");
                    rb.velocity = new Vector2(0f, 0f);
                    rb.AddForce(jumpForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                anim.ResetTrigger("jumped");
                if (idleTimer > idlePeriod) idleTimer = 0;
                if (isGrounded) isGrounded = false;
                Debug.DrawLine(groundRaycastOrigin.position, groundRaycastOrigin.position + transform.up * -1 * groundRaycastLength, Color.red);
            }


            RaycastHit2D playerHitInfo = Physics2D.Raycast(playerRaycastOrigin.position, transform.right * -1, playerRaycastLength);


            if (playerHitInfo && playerHitInfo.collider.gameObject.CompareTag("Player"))
            {
                if (!isShooting) isShooting = true;
                idleTimer = 0;
                if (!upperBodyCollider.enabled) upperBodyCollider.enabled = true;
                Debug.DrawLine(playerRaycastOrigin.position, playerRaycastOrigin.position + transform.right * -1 * playerRaycastLength, Color.green);
                //shoot
            }
            else
            {
                if (isShooting) isShooting = false;
                if (upperBodyCollider.enabled) upperBodyCollider.enabled = false;
                Debug.DrawLine(playerRaycastOrigin.position, playerRaycastOrigin.position + transform.right * -1 * playerRaycastLength, Color.red);
            }

            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("isShooting", isShooting);
            anim.SetFloat("yVelocity", rb.velocity.y);
        }
        else
        {

            foreach(var collider in canMoveCheckColliderList)
            {
                if (collider.CompareTag("Player"))
                {
                    canMove = true;
                }
            }
        }
    }

    void Fire()
    {
        var cProjectile = Instantiate(shotPrefab, playerRaycastOrigin.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, canMoveCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerProjectile>() != null)
        {
            var playerProjectile = collision.GetComponent<PlayerProjectile>();
            DecrementHealth(playerProjectile.GetDamage());
        }
    }

    void DecrementHealth(int decrementValue)
    {
        health -= decrementValue;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
