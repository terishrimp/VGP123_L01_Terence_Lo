using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip levelOneMusic;
    [SerializeField] AudioClip gameOverMusic;
    AudioSource audioSource;
    public static SoundManager instance;
    Camera camToFollow;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
        audioSource.volume = GameManager.instance.GlobalVolume;
        camToFollow = Camera.main;
        GameManager.instance.VolumeChange += OnVolumeChange;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.instance.MuteChange += OnMuteChange;
        CheckForMusic(SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = camToFollow.transform.position;
    }

    void OnMuteChange(object sender, bool isMuted)
    {
        audioSource.mute = isMuted;
    }
    void OnVolumeChange(object sender, float volume)
    {
        audioSource.volume = volume;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeCamToFollow(scene);
        CheckForMusic(scene);
    }

    void ChangeCamToFollow(Scene scene)
    {
        camToFollow = Camera.main;
    }
    void CheckForMusic(Scene scene)
    {
        switch (scene.name)
        {
            case "SnowMountain":
                audioSource.clip = levelOneMusic;
                break;
            case "Title":
                audioSource.clip = menuMusic;
                break;
            case "GameOver":
                audioSource.clip = gameOverMusic;
                break;
            default:
                audioSource.clip = null;
                break;
        }
        if (audioSource)
            audioSource.Play();
    }

    private void OnDestroy()
    {
        GameManager.instance.MuteChange -= OnMuteChange;
        GameManager.instance.VolumeChange -= OnVolumeChange;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
