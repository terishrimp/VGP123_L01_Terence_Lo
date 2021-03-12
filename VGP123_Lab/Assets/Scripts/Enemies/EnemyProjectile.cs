using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        gameObject.tag = "enemyProjectile";
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        int maskCheck = 1 << collision.gameObject.layer & layerMasksToIgnore.value;
        if (maskCheck == 0)
        {
            if (collision.GetComponent<PlayerCollision>() != null)
            {
                var player = collision.GetComponent<PlayerCollision>();
                if (!player.IsHit) {
                    GameManager.instance.Health -= damage;
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
