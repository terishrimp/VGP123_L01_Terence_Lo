using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupNoise = null;

    [SerializeField] PlayerMovement player = null;

    GameObject collectableList;
    private void Start()
    { 
        if(GameObject.FindGameObjectWithTag("collectableList") != null) { 
        collectableList = GameObject.FindGameObjectWithTag("collectableList");
        }
        else
        {
            var newList = Instantiate(new GameObject("Collectables"), new Vector3(0, 0, 0), Quaternion.identity);
            newList.tag = "collectableList";
            collectableList = newList;
        }

        transform.SetParent(collectableList.transform);
    }
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
