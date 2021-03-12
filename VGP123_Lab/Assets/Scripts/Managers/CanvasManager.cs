using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{

    [SerializeField] TitleCanvas titleCanvas;
    [SerializeField] PauseCanvas pauseCanvas;
    [SerializeField] SettingsCanvas settingsCanvas;
    [SerializeField] UserInterface userInterfaceCanvas;
    [SerializeField] bool isInLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!pauseCanvas.MyCanvasManager)
        pauseCanvas.MyCanvasManager = this;
        if(!settingsCanvas.MyCanvasManager)
        settingsCanvas.MyCanvasManager = this;
        if(!titleCanvas.MyCanvasManager)
        titleCanvas.MyCanvasManager = this;
        if (!isInLevel)
        {
            ShowTitle();
        }
        else
        {
            ShowUserInterface();
        }
        
        GameManager.instance.PauseChange += OnPauseChange;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnPauseChange(object sender, bool isPaused)
    {
        if (isPaused)
        {
            ShowPause();
        }
        else
        {
            ShowUserInterface();
        }
    }
    void SetListActive (GameObject[] canvasList, bool isActive)
    {
        foreach(GameObject go in canvasList)
        {
            go.SetActive(isActive);
        }
    }

    public void ShowTitle()
    {
        DisableAll();
        titleCanvas.gameObject.SetActive(true);
    }
    public void ShowSettings()
    {
        DisableAll();
        settingsCanvas.gameObject.SetActive(true);
    }

    public void ShowPause()
    {
        DisableAll();
        pauseCanvas.gameObject.SetActive(true);
    }

    public void ShowUserInterface()
    {
        DisableAll();
        userInterfaceCanvas.gameObject.SetActive(true);
    }
    public void DisableAll()
    {
        SetListActive(new GameObject[4] { titleCanvas.gameObject, pauseCanvas.gameObject, settingsCanvas.gameObject, userInterfaceCanvas.gameObject}
, false);
    }

    private void OnDestroy()
    {
        GameManager.instance.PauseChange -= OnPauseChange;
    }

}
