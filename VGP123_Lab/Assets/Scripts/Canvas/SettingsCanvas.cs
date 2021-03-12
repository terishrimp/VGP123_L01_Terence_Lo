using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SettingsCanvas : BaseCanvas
{

    [SerializeField] Slider volSlider;
    [SerializeField] Toggle volToggle;
    [SerializeField] TextMeshProUGUI volText;
    [SerializeField] Button exitButton;

    private void Start()
    {
        if (exitButton)
            exitButton.onClick.AddListener(() => myCanvasManager.ShowTitle());
        if (volSlider)
            volSlider.normalizedValue = GameManager.instance.GlobalVolume;
        if (volToggle)
            volToggle.isOn = GameManager.instance.IsMuted;
            volToggle.onValueChanged.AddListener((value) => GameManager.instance.IsMuted = value);

    }

    private void Update()
    {
        if (volSlider)
            GameManager.instance.GlobalVolume = volSlider.normalizedValue;
        if (volText)
            volText.text = Math.Round(volSlider.normalizedValue, 2).ToString();

    }
}
