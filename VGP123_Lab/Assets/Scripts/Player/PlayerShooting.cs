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

    [Header("Sounds")]
    [SerializeField] AudioClip busterShotClip;
    [SerializeField] AudioClip levelOneShotClip;
    [SerializeField] AudioClip levelTwoShotClip;
    [SerializeField] AudioClip initialChargeClip;
    [SerializeField] AudioClip chargeLoopClip;
    float chargeTimer = 0;
    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = GameManager.instance.IsMuted;
        audioSource.volume *= GameManager.instance.GlobalVolume;
    }
    // Update is called once per frame
    void Update()
    {
        if (isCharging && audioSource.clip == initialChargeClip && chargeTimer > audioSource.clip.length)
        {
            audioSource.clip = chargeLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        if ((playerMovement != null && playerMovement.MovementEnabled == true) || playerMovement == null)
        {

            if (shotCd < shotCdPeriod && !Input.GetButton("Fire1")) shotCd += Time.deltaTime;
            if (shotCd >= shotCdPeriod) canShoot = true;
            if (Input.GetButtonUp("Fire1") && canShoot)
            {
                if (audioSource.clip == chargeLoopClip || audioSource.clip == initialChargeClip)
                    audioSource.Stop();
                if (chargeTimer < levelOneShotTimerPeriod)
                {
                    FireShot(busterShot);
                    audioSource.PlayOneShot(busterShotClip, 0.75f * GameManager.instance.GlobalVolume);
                }
                else if (chargeTimer >= levelOneShotTimerPeriod && chargeTimer < levelTwoShotTimerPeriod)
                {
                    FireShot(levelOneShot);
                    audioSource.PlayOneShot(levelOneShotClip, 0.75f * GameManager.instance.GlobalVolume);
                }
                else if (chargeTimer >= levelTwoShotTimerPeriod)
                {
                    FireShot(levelTwoShot);
                    audioSource.PlayOneShot(levelTwoShotClip, 0.75f * GameManager.instance.GlobalVolume);
                }
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
                audioSource.PlayOneShot(busterShotClip, 0.75f * GameManager.instance.GlobalVolume);
                audioSource.clip = initialChargeClip;
                audioSource.Play();

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
