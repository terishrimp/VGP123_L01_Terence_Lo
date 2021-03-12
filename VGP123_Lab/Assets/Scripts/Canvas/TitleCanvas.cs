using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleCanvas : BaseCanvas
{
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button settingsButton;
    private void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => GameManager.instance.NextScene());

        if (quitButton)
            quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());

        if (settingsButton)
            settingsButton.onClick.AddListener(() => myCanvasManager.ShowSingleCanvas(myCanvasManager.SettingsCanvas.gameObject));
    }
}

