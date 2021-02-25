using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
    
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ParallaxController : MonoBehaviour
{
    [SerializeField] bool overrideParallaxValue;
    [SerializeField] float parallaxXFactor = 0.5f;
    [SerializeField] float parallaxYFactor = 0.5f;
    [SerializeField] ParallaxManager parallaxBackground = null;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (overrideParallaxValue)
            {
                parallaxBackground.SetParallaxXFactor(parallaxXFactor);
                parallaxBackground.SetParallaxYFactor(parallaxYFactor);
                parallaxBackground.SetInitialPos(parallaxBackground.transform.position);
            }
                parallaxBackground.SetIsMoving(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parallaxBackground.SetIsMoving(false);
        }
    }



    public void SetOverrideParallaxValue(bool value)
    {
        overrideParallaxValue = value;
    }
    public bool GetOverrideParallaxValue()
    {
        return overrideParallaxValue;
    }

    public void SetParallaxXFactor (float value)
    {
        parallaxXFactor = value;
    }

    public float GetParallaxXFactor()
    {
        return parallaxXFactor;
    }
    public void SetParallaxYFactor(float value)
    {
        parallaxYFactor = value;
    }
    public float GetParallaxYFactor()
    {
        return parallaxYFactor;
    }
    public void SetParallaxBackground(ParallaxManager parallaxBackground)
    {
        this.parallaxBackground = parallaxBackground;
    }

    public ParallaxManager GetParallaxBackground()
    {
        return parallaxBackground;
    }
}


[CustomEditor(typeof(ParallaxController))]
public class ParallaxControllerEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var parallaxController = target as ParallaxController;

        parallaxController.SetParallaxBackground(EditorGUILayout.ObjectField("Parallax Background", parallaxController.GetParallaxBackground(), typeof(ParallaxManager), true) as ParallaxManager);

        parallaxController.SetOverrideParallaxValue(GUILayout.Toggle(parallaxController.GetOverrideParallaxValue(), "Override Parallax Value"));

        if (parallaxController.GetOverrideParallaxValue()) {
            parallaxController.SetParallaxXFactor(EditorGUILayout.FloatField("Parallax X Factor", parallaxController.GetParallaxXFactor()));
            parallaxController.SetParallaxYFactor(EditorGUILayout.FloatField("Parallax Y Factor", parallaxController.GetParallaxYFactor()));
        }

    }
}