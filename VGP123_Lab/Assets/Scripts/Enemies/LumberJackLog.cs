using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class LumberJackLog : EnemyProjectile
{

    bool canBeShot = false;
    bool isShot = false;
    Vector3 ogPos;
    public bool CanBeShot
    {
        get { return canBeShot; }
        set
        {
            if (value == canBeShot) return;
            canBeShot = value;
        }
    }
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
    public Rigidbody2D Rb
    {
        get { return rb; }
    }
    public Vector3 OgPos
    {
        get { return ogPos; }
    }
    protected override void Awake()
    {
        ogScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();

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

}
