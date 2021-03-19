using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupNoise = null;

    [SerializeField] PlayerMovement player = null;

    SpriteRenderer spriteRenderer;
    GameObject collectableList;
    Animator animator;
    AudioSource audioSource;
    Rigidbody2D rb;
    bool activated = false;
    private void Start()
    {

        if (GameObject.FindGameObjectWithTag("collectableList") != null)
        {
            collectableList = GameObject.FindGameObjectWithTag("collectableList");
        }
        else
        {
            var newList = Instantiate(new GameObject("Collectables"), new Vector3(0, 0, 0), Quaternion.identity);
            newList.tag = "collectableList";
            collectableList = newList;
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        audioSource.mute = GameManager.instance.IsMuted;
        audioSource.volume *= GameManager.instance.GlobalVolume;
        transform.SetParent(collectableList.transform);
    }

    private void Update()
    {

        if (activated)
        {
            spriteRenderer.enabled = false;
            animator.enabled = false;
            rb.simulated = false;
            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            ActivatePickup();
        }
    }

    protected virtual void ActivatePickup()
    {
        activated = true;
        audioSource.PlayOneShot(pickupNoise, .75f * GameManager.instance.GlobalVolume);


    }
}
