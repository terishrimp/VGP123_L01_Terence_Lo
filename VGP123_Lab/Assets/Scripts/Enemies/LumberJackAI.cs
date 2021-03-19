using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class LumberJackAI : MonoBehaviour
{
    [SerializeField] float playerInRangeCheckRadius = 7f;
    [SerializeField] Transform playerInRangeCheckPos;
    [SerializeField] int health = 2;
    [SerializeField] LumberJackTree tree;

    [SerializeField] ParticleSystem deathExplosion;
    [Header("Sound")]
    [SerializeField] AudioClip deathClip;
    [SerializeField] AudioClip hitClip;


    AudioSource audioSource;
    bool isPlayerInRange = false;
    bool canBreakLog = false;
    bool isDead = false;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    GameObject enemyListGo;
    const string animBreakLogString = "canBreakLog";
    public bool IsPlayerInRange
    {
        get { return isPlayerInRange; }
        set
        {
            if (value == isPlayerInRange) return;
            isPlayerInRange = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
         enemyListGo = GameObject.FindGameObjectWithTag("enemyList");

        if (enemyListGo == null)
        {
            enemyListGo = Instantiate(new GameObject("Enemy List"), new Vector3(0, 0, 0), Quaternion.identity);
            enemyListGo.tag = "enemyList";
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource.mute = GameManager.instance.IsMuted;
        audioSource.volume *= GameManager.instance.GlobalVolume;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] goInRangeList = Physics2D.OverlapCircleAll(playerInRangeCheckPos.position, playerInRangeCheckRadius);
        IsPlayerInRange = false;
        foreach (var go in goInRangeList)
        {
            if (go.CompareTag("Player"))
            {
                IsPlayerInRange = true;
                break;
            }
        }
        if (IsPlayerInRange && tree.HasLogs) canBreakLog = true;
        else canBreakLog = false;

        animator.SetBool(animBreakLogString, canBreakLog);

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
    void BreakLog()
#pragma warning restore IDE0051 // Remove unused private members
    {
        tree.ShootLog(tree.LogList.Count-1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerProjectile>() != null)
        {
            var playerProjectile = collision.GetComponent<PlayerProjectile>();
            DecrementHealth(playerProjectile.GetDamage());
        }

        if (collision.GetComponent<PlayerCollision>() != null)
        {
            var player = collision.GetComponent<PlayerCollision>();
            if (!player.IsHit)
            {
                GameManager.instance.Health -= 1;
            }
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
            if (!GameManager.instance.IsMuted)
                audioSource.PlayOneShot(deathClip, .75f * GameManager.instance.GlobalVolume);
            tree.gameObject.transform.parent = enemyListGo.transform;
            Destroy(gameObject);
        }
        else
        {
            audioSource.PlayOneShot(hitClip, 0.75f * GameManager.instance.GlobalVolume);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerInRangeCheckPos.position, playerInRangeCheckRadius);
    }
}
