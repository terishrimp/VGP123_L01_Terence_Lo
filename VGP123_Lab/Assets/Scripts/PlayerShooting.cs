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
<<<<<<< Updated upstream
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
=======
        //add cooldown to shooting so player can't spam
        if (shotCd < shotCdPeriod) shotCd += Time.deltaTime;
        if (shotCd > shotCdPeriod) canShoot = true;

        if (Input.GetButtonUp("Fire1") && canShoot)
        {
            if (chargeTimer < levelOneShotTimerPeriod)
                FireShot(busterShot);
            else if (chargeTimer >= levelOneShotTimerPeriod && chargeTimer < levelTwoShotTimerPeriod)
                FireShot(levelOneShot);
            else if (chargeTimer >= levelTwoShotTimerPeriod)
                FireShot(levelTwoShot);
            shotCd = 0;
            canShoot = false;
        }
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            chargeTimer = 0;
            FireShot(busterShot);

        }
        if (Input.GetButton("Fire1") && canShoot)
        {
            chargeTimer += Time.deltaTime;
        }
    }


    void FireShot(PlayerProjectile projectile)
    {
        if (Mathf.Sign(transform.localScale.x) == -1)
        {
            var cBusterShot = Instantiate(projectile, shotSpawn.position, Quaternion.identity);
            cBusterShot.SetMovingRight(false);
        }
        else
        {
            var cBusterShot = Instantiate(projectile, shotSpawn.position, Quaternion.identity);
            cBusterShot.SetMovingRight(true);
>>>>>>> Stashed changes
        }
    }
}
