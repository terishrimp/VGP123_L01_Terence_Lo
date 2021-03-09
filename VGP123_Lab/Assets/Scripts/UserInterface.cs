using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] float lifeBarYOffset = 36f;
    [SerializeField] float healthYOffset = 36f;
    [SerializeField] Transform healthPointOrigin;
    [SerializeField] Image lifeBarPrefab = null;
    [SerializeField] Image healthPointPrefab = null;
    List<GameObject> healthPointImages = new List<GameObject>();


    float lifeBarOgHeight;
    // Start is called before the first frame update
    void Start()
    {
        lifeBarOgHeight = lifeBarPrefab.rectTransform.sizeDelta.y;
        SceneLoader.HealthChange += OnHealthChange;
        UpdateHealth(SceneLoader.Health);
        lifeBarPrefab.rectTransform.sizeDelta = new Vector2(lifeBarPrefab.rectTransform.sizeDelta.x,
    lifeBarOgHeight + (SceneLoader.MaxHealth - SceneLoader.MinHealth) * lifeBarYOffset);

        var updatedLifeBarPosition = new Vector3(lifeBarPrefab.rectTransform.position.x,
            lifeBarPrefab.rectTransform.position.y + ((SceneLoader.MaxHealth - SceneLoader.MinHealth) * lifeBarYOffset / 2)
            , lifeBarPrefab.rectTransform.position.z);

        lifeBarPrefab.rectTransform.position = updatedLifeBarPosition;
    }


    void OnHealthChange(object sender, int e)
    {
        UpdateHealth(e);
    }
    void UpdateHealth(int healthAmount)
    {
        foreach (var image in healthPointImages)
        {
            Destroy(image.gameObject);
        }
        healthPointImages.Clear();
        for (int i = 0; i < healthAmount; i++)
        {
            var cHealthpoint = Instantiate(healthPointPrefab, healthPointOrigin.position + new Vector3(0f, i * healthYOffset, 0f), Quaternion.identity);
            cHealthpoint.transform.SetParent(transform);
            healthPointImages.Add(cHealthpoint.gameObject);
        }


    }

    private void OnDestroy()
    {
        SceneLoader.HealthChange -= OnHealthChange;
    }
}
