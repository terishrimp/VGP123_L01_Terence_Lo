using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LumberJackLog : EnemyProjectile
{
    bool canBeShot = false;
    public bool CanBeShot
    {
        get { return canBeShot; }
        set
        {
            if (value == canBeShot) return;
            canBeShot = value;
        }
    }

    bool isShot = false;
    public bool IsShot
    {
        get { return isShot; }
        set
        {
            if (value == isShot) return;
            isShot = value;
            if (isShot)
            {
                Destroy(gameObject, lifeTime);
            }
        }
    }

    Vector3 ogPos;

    public Vector3 OgPos
    {
        get { return ogPos; }
    }
    protected override void Awake()
    {
        if (projectileSpeed < 0)
        {
            projectileSpeed *= -1;
        }
        ogPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isShot)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.isKinematic = true;
            rb.velocity = new Vector2(-projectileSpeed * Time.deltaTime, 0);
        }

        if (rb.isKinematic == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        int maskCheck = 1 << collision.gameObject.layer & layerMasksToIgnore.value;
        if (maskCheck == 0)
        {
            if (collision.GetComponent<PlayerCollision>() != null)
            {
                var player = collision.GetComponent<PlayerCollision>();
                if (!player.IsHit && IsShot)
                {
                    SceneLoader.instance.Health -= damage;
                    Destroy(gameObject);
                }
            }
        }
    }

    public override int GetDamage()
    {
        return base.GetDamage();
    }
}
