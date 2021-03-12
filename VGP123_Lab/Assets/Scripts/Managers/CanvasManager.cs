using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{

    [SerializeField] TitleCanvas titleCanvas;
    public TitleCanvas TitleCanvas
    {
        get { return titleCanvas; }
    }

    [SerializeField] PauseCanvas pauseCanvas;
    public PauseCanvas PauseCanvas
    {
        get { return pauseCanvas; }
    }

    [SerializeField] SettingsCanvas settingsCanvas;
    public SettingsCanvas SettingsCanvas
    {
        get { return settingsCanvas; }
    }

    [SerializeField] UserInterfaceCanvas userInterfaceCanvas;
    public UserInterfaceCanvas UserInterfaceCanvas
    {
        get { return UserInterfaceCanvas; }
    }
    [SerializeField] bool isInLevel = false;

    void Start()
    {
        if (!pauseCanvas.MyCanvasManager)
            pauseCanvas.MyCanvasManager = this;

        if (!settingsCanvas.MyCanvasManager)
            settingsCanvas.MyCanvasManager = this;

        if (!titleCanvas.MyCanvasManager)
            titleCanvas.MyCanvasManager = this;

        if (!isInLevel) ShowSingleCanvas(titleCanvas.gameObject);
        else ShowSingleCanvas(userInterfaceCanvas.gameObject);

        GameManager.instance.PauseChange += OnPauseChange;
    }
    void OnPauseChange(object sender, bool isPaused)
    {
        if (isPaused) ShowSingleCanvas(pauseCanvas.gameObject);
        else ShowSingleCanvas(userInterfaceCanvas.gameObject);
    }
    void SetListActive(GameObject[] canvasList, bool isActive)
    {
        foreach (GameObject go in canvasList) go.SetActive(isActive);
    }

    public void ShowSingleCanvas(GameObject go)
    {
        DisableAllCanvas();
        go.SetActive(true);
    }
    public void DisableAllCanvas()
    {
        SetListActive(new GameObject[4] {
            titleCanvas.gameObject, 
            pauseCanvas.gameObject, 
            settingsCanvas.gameObject, 
            userInterfaceCanvas.gameObject
        }, 
            false);
    }

    private void OnDestroy()
    {
        GameManager.instance.PauseChange -= OnPauseChange;
    }

}
