using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] protected LayerMask layerMasksToIgnore;
    [SerializeField] protected int damage = 1;

    private Rigidbody2D rb;
    private bool movingRight = false;
    Vector3 ogScale;
    // Start is called before the first frame update
    void Start()
    {
        ogScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight)
        {
            rb.velocity = transform.right * projectileSpeed * Time.deltaTime;
        }
        else
        {
            transform.localScale = new Vector3(ogScale.x * -1, ogScale.y, ogScale.z);
            rb.velocity = -transform.right * projectileSpeed * Time.deltaTime;
        }

    }

    public void SetMovingRight(bool value)
    {
        movingRight = value;
    }

    public bool GetMovingRight()
    {
        return movingRight;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " has hit the Trigger " + collision.gameObject.name);
    }


    public int GetDamage()
    {
        return damage;
    }
}
