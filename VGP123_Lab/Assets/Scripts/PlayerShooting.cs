using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] PlayerProjectile busterShot;
    [SerializeField] Transform shotSpawn;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire1")) {
            if (Mathf.Sign(transform.localScale.x) == -1)
            {
             var cBusterShot =  Instantiate(busterShot, shotSpawn.position, Quaternion.identity);
                cBusterShot.SetMovingRight(false);
            }
            else
            {
                var cBusterShot = Instantiate(busterShot, shotSpawn.position, Quaternion.identity);
                cBusterShot.SetMovingRight(true);
            }
        }
    }
}
