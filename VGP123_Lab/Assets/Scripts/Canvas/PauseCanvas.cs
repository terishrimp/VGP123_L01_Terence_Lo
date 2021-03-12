using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseCanvas : BaseCanvas
{
    [SerializeField] Button returnButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button mainMenuButton;
    private void Start()
    {
        if (returnButton)
            returnButton.onClick.AddListener(() => GameManager.instance.IsPaused = false);

        if (settingsButton)
            settingsButton.onClick.AddListener(() => myCanvasManager.ShowSettings());

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(() => GameManager.instance.LoadTitle());


    }
}
