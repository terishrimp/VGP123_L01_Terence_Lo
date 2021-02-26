using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    SceneLoader sceneLoader;

    private void Start()
    {
        if (FindObjectOfType<SceneLoader>() != null)
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneLoader != null && collision.CompareTag("Player"))
        {
            sceneLoader.ResetCurrentScene();
        }
    }
}
