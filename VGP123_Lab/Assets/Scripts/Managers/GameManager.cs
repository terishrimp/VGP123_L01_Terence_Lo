using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
    public event EventHandler<int> HealthChange;
    public event EventHandler<int> ScreenChange;
    public event EventHandler<bool> PauseChange;
    public event EventHandler<float> VolumeChange;
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
            instance.HealthChange?.Invoke(this, value);
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


    bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
        set
        {
            if (value == isPaused) return;
            if (value)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1f;
            }
            instance.PauseChange?.Invoke(this, value);
            isPaused = value;
        }
    }

    bool isMuted = false;
    public bool IsMuted
    {
        get { return isMuted; }
        set
        {
            if (value == isMuted) return;
            Debug.Log(value);
            isMuted = value;
        }
    }

    float globalVolume = .5f;
    public float GlobalVolume
    {
        get { return globalVolume; }
        set
        {
            if (value == globalVolume) return;
            if (value < 0) globalVolume = 0;
            else if (value > 1) globalVolume = 1;
            else globalVolume = value;
            VolumeChange?.Invoke(instance, value);
        }
    }
    Vector2 lastScreenSize = new Vector2(0, 0);
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    private void Start()
    {
        Health = MaxHealth;
        Application.targetFrameRate = 120;
    }


    private void Update()
    {
        if (lastScreenSize != new Vector2(Screen.width, Screen.height))
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            instance.ScreenChange?.Invoke(this, health);
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
            QuitGame();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (FindObjectOfType<PlayerMovement>()) { 
            IsPaused = !IsPaused;
            }
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

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}