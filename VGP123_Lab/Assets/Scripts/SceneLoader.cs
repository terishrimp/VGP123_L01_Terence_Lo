using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
    public event EventHandler<int> HealthChange;
    public event EventHandler<int> ScreenChange;

    [SerializeField] int minHealth = 16;
    public int MinHealth
    {
        get { return minHealth; }
    }

    [SerializeField] int maxHealth = 16;
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    int health = 16;
    public int Health
    {
        get { return health; }
        set
        {
            if (value == health) return;
            if (value <= 0 || value > maxHealth)
            {
                health = maxHealth;
                if (value <= 0)
                {
                    //use coroutine to display death animation
                    Lives--;
                }
            }
            else
            {
                health = value;
            }
            instance.HealthChange?.Invoke(instance.HealthChange, value);
        }
    }


    [SerializeField] int maxLives = 3;

    int lives = 3;
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
                instance.NextScene();
            }
            else if (value < lives)
            {
                lives = value;
                instance.ResetCurrentScene();
            }
            else
            {
                lives = value;
            }


        }
    }

    public static SceneLoader instance;

    Vector2 lastScreenSize = new Vector2(0, 0);
    // Start is called before the first frame update
    void Awake()
    {
        SceneLoader[] instances = FindObjectsOfType<SceneLoader>();
        if (instances.Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
        instance = this;
        Health = MaxHealth;
        Application.targetFrameRate = 120;
    }


    private void Update()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            instance.ScreenChange?.Invoke(instance.ScreenChange, health);
        }

        if (Input.GetKeyDown(KeyCode.F1))
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