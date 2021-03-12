using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] PlayerProjectile busterShot;
    [SerializeField] PlayerProjectile levelOneShot;
    [SerializeField] PlayerProjectile levelTwoShot;
    [SerializeField] Animator fireChargeAnim;
    [SerializeField] Transform shotSpawn;
    [SerializeField] float levelOneShotTimerPeriod = 1f;
    [SerializeField] float levelTwoShotTimerPeriod = 2f;
    [SerializeField] float shotCdPeriod = .25f;
    float chargeTimer = 0;

    float shotCd;
    bool canShoot = true;
    bool isShooting = false;
    bool isCharging = false;
    bool onLevelOneCharge = false;
    bool onLevelTwoCharge = false;
    Animator animator;
    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        shotCd = shotCdPeriod;
    }
    // Update is called once per frame
    void Update()
    {
        if ((playerMovement != null && playerMovement.MovementEnabled == true) || playerMovement == null)
        {
            if (shotCd < shotCdPeriod && !Input.GetButton("Fire1")) shotCd += Time.deltaTime;
            if (shotCd >= shotCdPeriod) canShoot = true;
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
                isShooting = false;
                isCharging = false;
                onLevelOneCharge = false;
                onLevelTwoCharge = false;
            }
            else if (Input.GetButtonDown("Fire1") && canShoot)
            {

                isShooting = true;
                chargeTimer = 0;
                FireShot(busterShot);

            }
            else if (Input.GetButton("Fire1") && canShoot)
            {
                chargeTimer += Time.deltaTime;
                if (chargeTimer >= levelOneShotTimerPeriod && chargeTimer < levelTwoShotTimerPeriod)
                {
                    if (!isCharging)
                    {
                        isCharging = true;
                    }
                    if (!onLevelOneCharge)
                    {
                        onLevelOneCharge = true;
                    }
                }
                else if (chargeTimer >= levelTwoShotTimerPeriod)
                {
                    if (!onLevelTwoCharge)
                    {
                        onLevelTwoCharge = true;
                    }
                }

            }
            fireChargeAnim.SetBool("onLevelOneCharge", onLevelOneCharge);
            fireChargeAnim.SetBool("onLevelTwoCharge", onLevelTwoCharge);
            fireChargeAnim.SetBool("isCharging", isCharging);
            animator.SetBool("isShooting", isShooting);
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
        }
    }
}
