using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ParallaxEdit : MonoBehaviour
{
    [SerializeField] ParallaxBorders parallaxManager = null;

    [Header ("Settings for On Enter")]
    [SerializeField] bool shiftBackgroundsOnEnter = false;
    [Tooltip("Only necessary if you want to shift backgrounds is set to true")]
    [SerializeField] GameObject newBackgroundOnEnter = null;
    [SerializeField] bool disableParallaxOnEnter = false;
    [SerializeField] bool overrideBorderValuesOnEnter = false;
    [Tooltip("right, left, top, bottom")]
    [SerializeField] Vector4 newBorderValuesOnEnter = new Vector4(1, 1, 1, 1);
    [SerializeField] bool changeParallaxFactorOnEnter = false;
    [Tooltip("x, y")]
    [SerializeField] Vector2 parallaxFactorOverrideOnEnter = new Vector2(0.5f, 0.5f);

    [Header("Settings for On Exit")]
    [SerializeField] bool shiftBackgroundsOnExit = false;
    [Tooltip("Only necessary if you want to shift backgrounds is set to true")]
    [SerializeField] GameObject newBackgroundOnExit = null;
    [SerializeField] bool disableParallaxOnExit = false;
    [SerializeField] bool overrideBorderValuesOnExit= false;
    [Tooltip("right, left, top, bottom")]
    [SerializeField] Vector4 newBorderValuesOnExit = new Vector4(1, 1, 1, 1);
    [SerializeField] bool changeParallaxFactorOnExit = false;
    [Tooltip("x, y")]
    [SerializeField] Vector2 parallaxFactorOverrideOnExit = new Vector2(0.5f, 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        if (parallaxManager == null)
        {
            parallaxManager = FindObjectOfType<ParallaxBorders>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (disableParallaxOnEnter) parallaxManager.CanParallax = false;
            else if (!disableParallaxOnEnter) parallaxManager.CanParallax = true;

            if (shiftBackgroundsOnEnter) parallaxManager.SetBackgroundToParallax(newBackgroundOnEnter);

            if (overrideBorderValuesOnEnter) parallaxManager.SetNewBorderValues(newBorderValuesOnEnter.x, newBorderValuesOnEnter.y, newBorderValuesOnEnter.z, newBorderValuesOnEnter.w);

            if (changeParallaxFactorOnEnter)
            {
                parallaxManager.ParallaxXFactor = parallaxFactorOverrideOnEnter.x;
                parallaxManager.ParallaxYFactor = parallaxFactorOverrideOnEnter.y;
            }

            parallaxManager.OgPos = parallaxManager.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (disableParallaxOnExit) parallaxManager.CanParallax = false;
            else if (!disableParallaxOnExit) parallaxManager.CanParallax = true;

            if (shiftBackgroundsOnExit) parallaxManager.SetBackgroundToParallax(newBackgroundOnExit);

            if (overrideBorderValuesOnExit) parallaxManager.SetNewBorderValues(newBorderValuesOnExit.x, newBorderValuesOnExit.y, newBorderValuesOnExit.z, newBorderValuesOnExit.w);

            if (changeParallaxFactorOnExit)
            {
                parallaxManager.ParallaxXFactor = parallaxFactorOverrideOnExit.x;
                parallaxManager.ParallaxYFactor = parallaxFactorOverrideOnExit.y;
            }

            parallaxManager.OgPos = parallaxManager.transform.position;
        }
    }
}
