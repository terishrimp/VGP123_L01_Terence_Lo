using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ParallaxEdit : MonoBehaviour
{
    [SerializeField] ParallaxBorders parallaxManager = null;
    [SerializeField] bool shiftBackgrounds = false;
    [Tooltip("Only necessary if you want to shift backgrounds is set to true")]
    [SerializeField] GameObject newBackground = null;
    [SerializeField] bool disableParallax = false;
    [SerializeField] bool overrideBorderValues = false;
    [Tooltip("right, left, top, bottom")]
    [SerializeField] Vector4 newBorderValues = new Vector4(1, 1, 1, 1);
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
            if (disableParallax) parallaxManager.CanParallax = false;
            else if (!disableParallax) parallaxManager.CanParallax = true;

            if (shiftBackgrounds) parallaxManager.SetBackgroundToParallax(newBackground);

            if (overrideBorderValues) parallaxManager.SetNewBorderValues(newBorderValues.x, newBorderValues.y, newBorderValues.z, newBorderValues.w);
        }
    }
}
