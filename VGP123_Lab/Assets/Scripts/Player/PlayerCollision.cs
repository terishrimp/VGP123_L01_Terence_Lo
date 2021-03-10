using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyProjectile"))
        {
            var cEnemyProjectile = collision.GetComponent<EnemyProjectile>();
            SceneLoader.instance.Health -= cEnemyProjectile.GetDamage();
            if(SceneLoader.instance.Health < 0) { Destroy (gameObject);}
        }
    }
}
