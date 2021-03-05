using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    [SerializeField] int maxHealth = 3;
    int health;
    public int Health
    {
        get { return health; }
        set
        {
            if (value == health) return;
            if (value < 0)
            {
                Lives--;
            }
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                Debug.Log(health);
                health = value;
            }
        }
    }
    [SerializeField] int maxLives = 3;
    public static int lives = 3;
    public int Lives
    {
        get { return lives; }
        set
        {
            if (value == lives) return;

            if (value > maxLives)
            {
                lives = maxLives;
            }
            else if (value < 0)
            {
                lives = maxLives;
                NextScene();
            }
            else if (value < lives)
            {
                lives = value;
                ResetCurrentScene();
            }
            else
            {
                lives = value;
            }


        }
    }

    public static SceneLoader cSceneLoader;
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader[] instances = FindObjectsOfType<SceneLoader>();
        if (instances.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
        cSceneLoader = this;
        health = maxHealth;
        Application.targetFrameRate = 120;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NextScene();
        }
        else if (Input.GetKeyDown(KeyCode.Tilde))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
