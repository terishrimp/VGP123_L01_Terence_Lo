using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupNoise = null;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null) {
            ActivatePickup();
        }
    }

    protected virtual void ActivatePickup()
    {
        Destroy(gameObject);
    }
}
