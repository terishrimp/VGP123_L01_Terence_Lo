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
        if (pauseCanvas != null)
            pauseCanvas.MyCanvasManager = this;

        if (settingsCanvas != null)
            settingsCanvas.MyCanvasManager = this;

        if (titleCanvas != null)
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

    public void ShowSingleCanvas(GameObject go)
    {
        DisableAllCanvas();
        go.SetActive(true);
    }
    public void DisableAllCanvas()
    {
        GameObject[] canvasList = {titleCanvas.gameObject,
            pauseCanvas.gameObject,
            settingsCanvas.gameObject,
            userInterfaceCanvas.gameObject };
        foreach (var canvas in canvasList)
        {
            if (canvas != null) canvas.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.PauseChange -= OnPauseChange;
    }

}
