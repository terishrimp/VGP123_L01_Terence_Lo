using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    GameManager sceneLoader;

    private void Start()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            sceneLoader = FindObjectOfType<GameManager>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneLoader != null && collision.CompareTag("Player"))
        {
            GameManager.instance.ResetCurrentScene();
        }
    }
}
