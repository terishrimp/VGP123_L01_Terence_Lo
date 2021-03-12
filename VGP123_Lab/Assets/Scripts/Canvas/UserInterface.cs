using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
public class UserInterface : MonoBehaviour
{
    [SerializeField] Image lifeImagePrefab = null;

    [SerializeField] Image lifeBar = null;
    [SerializeField] float lifeBarYOffset = 36f;

    [SerializeField] float healthYOffset = 36f;
    [SerializeField] Image healthPointPrefab = null;
    [SerializeField] Transform healthPointOrigin;

    List<Image> healthPointImages = new List<Image>();
    List<Vector3> childOgScaleList = new List<Vector3>();
    List<RectTransform> childRectTransformList = new List<RectTransform>();
    PixelPerfectCamera ppCam;
    float lifeBarOgHeight;
    Vector3 healthPointOgScale;

    // Start is called before the first frame update
    void Start()
    {
        ppCam = Camera.main.GetComponent<PixelPerfectCamera>();
        GameManager.instance.HealthChange += OnHealthChange;
        GameManager.instance.ScreenChange += OnScreenChange;

        if (lifeBar != null)
        {
            lifeBarOgHeight = lifeBar.rectTransform.sizeDelta.y;
            //set lifebar height according to max health
            lifeBar.rectTransform.sizeDelta = new Vector2(lifeBar.rectTransform.sizeDelta.x,
        lifeBarOgHeight + (GameManager.instance.MaxHealth - GameManager.instance.MinHealth) * lifeBarYOffset);

            var updatedLifeBarPosition = new Vector3(lifeBar.rectTransform.position.x,
                lifeBar.rectTransform.position.y + ((GameManager.instance.MaxHealth - GameManager.instance.MinHealth) * lifeBarYOffset * ppCam.pixelRatio / 2)
                , lifeBar.rectTransform.position.z);

            lifeBar.rectTransform.position = updatedLifeBarPosition;
        }

        if(healthPointPrefab != null)
        {
            healthPointOgScale = healthPointPrefab.rectTransform.localScale;
        }

        foreach(Transform child in transform)
        {
            childRectTransformList.Add(child.GetComponent<RectTransform>()) ;
            childOgScaleList.Add(childRectTransformList[0].localScale);
        }
        StartCoroutine(LateStart());
        //scale to pixel ratio

    }
    

    //not the best approach but necessary to obtain correct value for ppCam.pixelRatio
    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.0001f);
        UpdateUI(GameManager.instance.Health);
    }

    void OnScreenChange(object sender, int healthAmount)
    {
        UpdateUI(healthAmount);
    }
    void OnHealthChange(object sender, int healthAmount)
    {

        UpdateHealth(healthAmount);
    }
    void UpdateHealth(int healthAmount)
    {
        if (healthPointPrefab != null && healthPointOrigin != null)
        {
            foreach (var image in healthPointImages)
            {
                Destroy(image.gameObject);
            }
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

    void UpdateUI(int healthAmount)
    {
        for (int i = 0; i < childRectTransformList.Count; i++)
        {
            ScaleToPixelRatio(childRectTransformList[i], childOgScaleList[0]);
        }

        UpdateHealth(healthAmount);
    }

    private void OnDestroy()
    {
        GameManager.instance.HealthChange -= OnHealthChange;
        GameManager.instance.ScreenChange -= OnScreenChange;
    }

    void ScaleToPixelRatio(RectTransform rTransform, Vector3 originalScale)
    {
        rTransform.localScale = originalScale * ppCam.pixelRatio;
    }

}
