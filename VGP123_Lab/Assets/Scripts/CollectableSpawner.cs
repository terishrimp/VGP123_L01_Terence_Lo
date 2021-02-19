using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] Pickup[] pickupList;
    [SerializeField] float minSpawnTime = 2f;
    [SerializeField] float maxSpawnTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPickups());
    }

    IEnumerator SpawnPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            var randomNum = Random.Range(0, pickupList.Length);
            Debug.Log(randomNum);
            Instantiate(pickupList[randomNum].gameObject, transform.position, Quaternion.identity);
        }
    }
}
