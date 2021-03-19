using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BunnyAI : MonoBehaviour
{
    [SerializeField] Collider2D upperBodyCollider = null;
    [SerializeField] Vector2 jumpForce = new Vector2(1, 1);
    [SerializeField] float idlePeriod = 1f;

    [SerializeField] float canMoveCheckRadius = 4f;
    [SerializeField] int health = 2;
    [SerializeField] ParticleSystem deathExplosion;

    [Header("Ground Raycast")]
    [SerializeField] Transform groundRaycastOrigin = null;
    [SerializeField] float groundRaycastLength = .5f;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Player Detection Raycast")]
    [SerializeField] Transform playerRaycastOrigin = null;
    [SerializeField] float playerRaycastLength = 1f;

    [Header("Projectile Properties")]
    [SerializeField] EnemyProjectile shotPrefab = null;

    [Header("Sound")]
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip hitClip;

    AudioSource audioSource;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    BoxCollider2D groundCollider;
    bool canMove = false;
    bool isGrounded = true;
    bool isShooting = false;
    bool isDead = false;
    float idleTimer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyListGo = GameObject.FindGameObjectWithTag("enemyList");
        if (enemyListGo == null)
        {
            enemyListGo = Instantiate(new GameObject("Enemy List"), new Vector3(0, 0, 0), Quaternion.identity);
            enemyListGo.tag = "enemyList";
        }

        transform.parent = enemyListGo.transform;
        idleTimer = idlePeriod;
        groundCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource.mute = GameManager.instance.IsMuted;
        audioSource.volume *= GameManager.instance.GlobalVolume;
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
                    animator.SetTrigger("jumped");
                    rb.velocity = new Vector2(0f, 0f);
                    rb.AddForce(jumpForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                animator.ResetTrigger("jumped");
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

            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("isShooting", isShooting);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
        else
        {

            foreach (var collider in canMoveCheckColliderList)
            {
                if (collider.CompareTag("Player"))
                {
                    canMove = true;
                }
            }
        }
        if (isDead)
        {
            spriteRenderer.sprite = null;
            animator.enabled = false;
            rb.simulated = false;
            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

#pragma warning disable IDE0051 // Remove unused private members
    void Fire()
#pragma warning restore IDE0051 // Remove unused private members
    {
        audioSource.PlayOneShot(shootClip);
        var cProjectile = Instantiate(shotPrefab, playerRaycastOrigin.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, canMoveCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerProjectile = collision.GetComponent<PlayerProjectile>();
        if (playerProjectile)
        {
            DecrementHealth(playerProjectile.GetDamage());
        }

        var playerCollision = collision.GetComponent<PlayerCollision>();
        if (playerCollision)
        {
            if (!playerCollision.IsHit)
            {
                GameManager.instance.Health -= 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var logCollision = collision.gameObject.GetComponent<LumberJackLog>();
        if (logCollision)
        {
            Physics2D.IgnoreCollision(groundCollider, collision.collider);
        }
    }

    void DecrementHealth(int decrementValue)
    {
        health -= decrementValue;
        if (health <= 0)
        {
            isDead = true;
            var cDeathExplosion = Instantiate(deathExplosion.gameObject, transform.position, Quaternion.identity);
            Destroy(cDeathExplosion, deathExplosion.main.duration);
            audioSource.PlayOneShot(deathClip, 0.75f * GameManager.instance.GlobalVolume);

        }
        else
        {
            audioSource.PlayOneShot(hitClip, 0.75f * GameManager.instance.GlobalVolume);
        }
    }
}
