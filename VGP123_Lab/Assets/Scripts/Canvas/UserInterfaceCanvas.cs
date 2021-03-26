using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TMPro;
public class UserInterfaceCanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lifeCounter = null;
    [SerializeField] float textScaleFactor = 2.3f;

    [SerializeField] Image healthBar = null;
    [SerializeField] float healthBarYOffset = 36f;

    [SerializeField] float healthYOffset = 36f;
    [SerializeField] Image healthPointPrefab = null;
    [SerializeField] Transform healthPointOrigin;

    List<Image> healthPointImages = new List<Image>();
    List<Vector3> childOgScaleList = new List<Vector3>();
    List<RectTransform> childRectTransformList = new List<RectTransform>();

    float lifeCounterOgFontSize;
    PixelPerfectCamera ppCam;
    public PixelPerfectCamera PPCam
    {
        get { return ppCam; }
    }
    float lifeBarOgHeight;
    Vector3 healthPointOgScale;

    // Start is called before the first frame update
    void Start()
    {
        ppCam = Camera.main.GetComponent<PixelPerfectCamera>();
        GameManager.instance.HealthChange += OnHealthChange;
        GameManager.instance.ScreenChange += OnScreenChange;
        GameManager.instance.LivesChange += OnLivesChange;

        if (lifeCounter) lifeCounterOgFontSize = lifeCounter.fontSize;

        if (healthBar)
        {
            lifeBarOgHeight = healthBar.rectTransform.sizeDelta.y;
            //set lifebar height according to max health
            healthBar.rectTransform.sizeDelta = new Vector2(healthBar.rectTransform.sizeDelta.x,
        lifeBarOgHeight + (GameManager.instance.MaxHealth - GameManager.instance.MinHealth) * healthBarYOffset);

            var updatedLifeBarPosition = new Vector3(healthBar.rectTransform.position.x,
                healthBar.rectTransform.position.y + ((GameManager.instance.MaxHealth - GameManager.instance.MinHealth) * healthBarYOffset * ppCam.pixelRatio / 2)
                , healthBar.rectTransform.position.z);

            healthBar.rectTransform.position = updatedLifeBarPosition;
        }

        if (healthPointPrefab) healthPointOgScale = healthPointPrefab.rectTransform.localScale;

        foreach (Transform child in transform)
        {
            childRectTransformList.Add(child.GetComponent<RectTransform>());
            childOgScaleList.Add(childRectTransformList[0].localScale);
        }
        StartCoroutine(LateStart());
        //scale to pixel ratio

    }

    void OnEnable()
    {
        StartCoroutine(LateStart());
    }

    //not the best approach but necessary to obtain correct value for ppCam.pixelRatio
    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.0002f);
        UpdateUI(GameManager.instance.Health, GameManager.instance.Lives);
    }

    void OnScreenChange(object sender, int healthAmount, int livesAmount)
    {
        UpdateUI(healthAmount, livesAmount);
    }
    void OnHealthChange(object sender, int healthAmount)
    {
        UpdateHealth(healthAmount);
    }
    void OnLivesChange(object sender, int livesAmount)
    {
        UpdateLives(livesAmount);
    }
    void UpdateHealth(int healthAmount)
    {
        if (healthPointPrefab != null && healthPointOrigin != null)
        {
            foreach (var image in healthPointImages) Destroy(image.gameObject);
            healthPointImages.Clear();

            for (int i = 0; i < healthAmount; i++)
            {
                var cHealthpoint = Instantiate(healthPointPrefab, healthPointOrigin.position + new Vector3(0f, i * healthYOffset * ppCam.pixelRatio, 0f), Quaternion.identity);
                cHealthpoint.transform.SetParent(transform);
                ScaleToPixelRatio(cHealthpoint.rectTransform, healthPointOgScale);
                healthPointImages.Add(cHealthpoint);
            }
        }
    }

    void UpdateLives(int livesAmount)
    {
        if (lifeCounter) lifeCounter.fontSize = lifeCounterOgFontSize * (ppCam.pixelRatio / textScaleFactor);
        lifeCounter.text = livesAmount.ToString();
    }

    void UpdateUI(int healthAmount, int livesAmount)
    {
        for (int i = 0; i < childRectTransformList.Count; i++) ScaleToPixelRatio(childRectTransformList[i], childOgScaleList[0]);
        UpdateLives(livesAmount);
        UpdateHealth(healthAmount);
    }

    void ScaleToPixelRatio(RectTransform rTransform, Vector3 originalScale)
    {
        rTransform.localScale = originalScale * ppCam.pixelRatio;
    }
    private void OnDestroy()
    {
        GameManager.instance.HealthChange -= OnHealthChange;
        GameManager.instance.ScreenChange -= OnScreenChange;
        GameManager.instance.LivesChange -= OnLivesChange;
    }


}
