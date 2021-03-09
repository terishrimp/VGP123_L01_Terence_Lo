using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
    public static event EventHandler<int> HealthChange;

    static int minHealth = 16;

    public static int MinHealth
    {
        get { return minHealth; }
    }
    static int maxHealth = 16;
    public static int MaxHealth
    {
        get { return maxHealth; }
    }

    static int health;
    public static int Health
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
            HealthChange?.Invoke(SceneLoader.HealthChange, value);
        }
    }

    static int maxLives = 3;
    static int lives = 3;
    public static int Lives
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
    void Awake()
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Health--;
        }
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
    public static void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
