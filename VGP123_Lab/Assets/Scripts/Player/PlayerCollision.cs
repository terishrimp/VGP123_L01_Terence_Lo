using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerCollision : MonoBehaviour
{

    [SerializeField] ParticleSystem deathPSPrefab;
    [SerializeField] Vector2 hitForce = new Vector2(-5f, 5f);
    [SerializeField] float iFramePeriod = 1f;
    [SerializeField] float movementDisablePeriod = .5f;
    [SerializeField] float flickerPeriod = 0.05f;

    [Header("Sound")]
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip deathClip;
    AudioSource audioSource;
    bool isHit = false;

    public bool IsHit
    {
        get { return isHit; }
    }
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    PlayerMovement playerMovement;

    const string animHitString = "isHit";
    Coroutine lastOnHit;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        //make sure there is only one spriteRenderer to look at
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (hitForce.x > 0) hitForce.x = 0;
        if (hitForce.y < 0) hitForce.y = 0;
        if (movementDisablePeriod > iFramePeriod) movementDisablePeriod = iFramePeriod;

        anim = GetComponent<Animator>();
        audioSource.mute = GameManager.instance.IsMuted;
        audioSource.volume *= GameManager.instance.GlobalVolume;
        GameManager.instance.HealthChange += OnHealthChange;
        GameManager.instance.LivesChange += OnLivesChange;
    }

    void OnLivesChange(object sender, int livesAmount)
    {
        if(!GameManager.instance.IsMuted)
            AudioSource.PlayClipAtPoint(deathClip, transform.position, 0.75f * GameManager.instance.GlobalVolume);
        var cDeathPS = Instantiate(deathPSPrefab.gameObject, transform.position, Quaternion.identity);
        Destroy(cDeathPS, deathPSPrefab.main.duration);
        Destroy(gameObject);
    }
    void OnHealthChange(object sender, int healthAmount)
    {
        if (healthAmount < GameManager.instance.Health)
        {
            if (healthAmount > 0)
                audioSource.PlayOneShot(hitClip, 0.75f * GameManager.instance.GlobalVolume);
            rb.velocity = Vector2.zero;
            rb.AddForce(hitForce * new Vector2(transform.localScale.x, 1), ForceMode2D.Impulse);
            if (lastOnHit != null) StopCoroutine(lastOnHit);
            lastOnHit = StartCoroutine(OnHit());
        }
    }

    IEnumerator OnHit()
    {

        isHit = true;
        //flash
        var flickerCoroutine = StartCoroutine(AlphaFlicker());

        Coroutine movementDisableCoroutine = null;
        if (playerMovement != null) movementDisableCoroutine = StartCoroutine(DisableMovement());

        yield return new WaitForSecondsRealtime(iFramePeriod);

        //stop flicker
        StopCoroutine(flickerCoroutine);

        //reset color
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        if (movementDisableCoroutine != null) StopCoroutine(movementDisableCoroutine);
        if(isHit) isHit = false;
    }
    IEnumerator DisableMovement()
    {
        anim.SetBool(animHitString, true);
        playerMovement.MovementEnabled = false;

        yield return new WaitForSecondsRealtime(movementDisablePeriod);

        playerMovement.MovementEnabled = true;
        anim.SetBool(animHitString, false);
    }
    IEnumerator AlphaFlicker()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(flickerPeriod);
            if (spriteRenderer.color == new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f))
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .2f);
            else 
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.HealthChange -= OnHealthChange;
        GameManager.instance.LivesChange -= OnLivesChange;
    }
}

