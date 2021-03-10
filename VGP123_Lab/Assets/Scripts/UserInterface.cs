using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
public class UserInterface : MonoBehaviour
{
    [SerializeField] float lifeBarYOffset = 36f;
    [SerializeField] float healthYOffset = 36f;
    [SerializeField] Transform healthPointOrigin;
    [SerializeField] Image lifeBar = null;
    [SerializeField] Image healthPointPrefab = null;
    List<Image> healthPointImages = new List<Image>();

    PixelPerfectCamera ppCam;
    Camera main;
    float lifeBarOgHeight;
    Vector3 healthPointOgScale;
    Vector3 lifeBarOgScale;

    private void Awake()
    {

        lifeBarOgHeight = lifeBar.rectTransform.sizeDelta.y;
        lifeBarOgScale = lifeBar.rectTransform.localScale;
        healthPointOgScale = healthPointPrefab.rectTransform.localScale;

    }
    // Start is called before the first frame update
    void Start()
    {
        ppCam = Camera.main.GetComponent<PixelPerfectCamera>();

        SceneLoader.instance.HealthChange += OnHealthChange;
        SceneLoader.instance.ScreenChange += OnScreenChange;

        //set lifebar height according to max health
        lifeBar.rectTransform.sizeDelta = new Vector2(lifeBar.rectTransform.sizeDelta.x,
    lifeBarOgHeight + (SceneLoader.instance.MaxHealth - SceneLoader.instance.MinHealth) * lifeBarYOffset);

        var updatedLifeBarPosition = new Vector3(lifeBar.rectTransform.position.x,
            lifeBar.rectTransform.position.y + ((SceneLoader.instance.MaxHealth - SceneLoader.instance.MinHealth) * lifeBarYOffset * ppCam.pixelRatio / 2)
            , lifeBar.rectTransform.position.z);

        lifeBar.rectTransform.position = updatedLifeBarPosition;

        StartCoroutine(LateStart());
        //scale to pixel ratio

    }
    

    //not the best approach but necessary to obtain correct value for ppCam.pixelRatio
    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.0001f);
        UpdateHealth(SceneLoader.instance.Health);
        lifeBar.rectTransform.localScale = lifeBarOgScale * ppCam.pixelRatio;
    }
    private void Update()
    {
        Debug.Log(ppCam.pixelRatio);
    }

    void OnScreenChange(object sender, int healthAmount)
    {
        UpdateHealth(healthAmount);
        lifeBar.rectTransform.localScale = lifeBarOgScale * ppCam.pixelRatio;
    }
    void OnHealthChange(object sender, int healthAmount)
    {
        UpdateHealth(healthAmount);
    }
    void UpdateHealth(int healthAmount)
    {
        lifeBar.rectTransform.localScale = lifeBarOgScale * ppCam.pixelRatio;
        foreach (var image in healthPointImages)
        {
            Destroy(image.gameObject);
        }
        healthPointImages.Clear();
        for (int i = 0; i < healthAmount; i++)
        {
            var cHealthpoint = Instantiate(healthPointPrefab, healthPointOrigin.position + new Vector3(0f, i * healthYOffset * ppCam.pixelRatio, 0f), Quaternion.identity);
            cHealthpoint.transform.SetParent(transform);
            cHealthpoint.rectTransform.localScale = healthPointOgScale * ppCam.pixelRatio;
            healthPointImages.Add(cHealthpoint);
        }


    }

    private void OnDestroy()
    {
        SceneLoader.instance.HealthChange -= OnHealthChange;
        SceneLoader.instance.ScreenChange -= OnScreenChange;
    }
}
