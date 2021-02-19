using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float lifeTime = 3f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetComponent<PlayerProjectile>() == null && collision.CompareTag("pickup"))
        {
            Destroy(gameObject);
        }
    }
}
