using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerProjectile : Projectile
{
    private void Start()
    {
        gameObject.tag = "playerProjectile";
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        int maskCheck = 1 << collision.gameObject.layer & layerMasksToIgnore.value;
        if (maskCheck == 0)
        {
            Destroy(gameObject);
        }
    }
}
