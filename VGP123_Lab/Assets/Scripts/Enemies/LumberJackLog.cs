using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class LumberJackLog : MonoBehaviour
{

    [SerializeField] float shotSpeed = 400f;
    [SerializeField] float lifetime = 4f;
    bool canBeShot = false;
    bool isShot = false;
    Rigidbody2D rb;
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
                Destroy(gameObject, lifetime);
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
    private void Awake()
    {
        if (shotSpeed < 0)
        {
            shotSpeed *= -1;
        }
        ogPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShot)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.isKinematic = true;
            rb.velocity = new Vector2(-shotSpeed * Time.deltaTime, 0);
        }

        if(rb.isKinematic == false)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

    }

}
