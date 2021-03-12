using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Vector2 hitForce = new Vector2(-5f, 5f);
    [SerializeField] float iFramePeriod = 1f;
    [SerializeField] float movementDisablePeriod = .5f;
    [SerializeField] float flickerPeriod = 0.05f;
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
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        //make sure there is only one spriteRenderer to look at
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (hitForce.x > 0) hitForce.x = 0;
        if (hitForce.y < 0) hitForce.y = 0;
        if (movementDisablePeriod > iFramePeriod) movementDisablePeriod = iFramePeriod;

        anim = GetComponent<Animator>();
        GameManager.instance.HealthChange += OnHealthChange;
    }

    void OnHealthChange(object sender, int healthAmount)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(hitForce * new Vector2 (transform.localScale.x, 1), ForceMode2D.Impulse);

        if (lastOnHit != null) StopCoroutine(lastOnHit);
        lastOnHit = StartCoroutine(OnHit());
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
    }
}

