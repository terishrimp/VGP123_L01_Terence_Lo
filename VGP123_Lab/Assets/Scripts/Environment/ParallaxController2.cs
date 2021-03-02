using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController2 : MonoBehaviour
{
    [SerializeField] SpriteRenderer backgroundToParallax;
    [SerializeField] float parallaxXFactor = 0.5f;
    [SerializeField] float parallaxYFactor = 0.5f;
    [SerializeField] bool canParallax;

    public bool CanParallax { get { return canParallax; }
        set {
            if (value == canParallax) return;

            canParallax = value;

            if (canParallax)
            {
                ogBackgroundPos = backgroundToParallax.transform.position;
                ogPos = transform.position;
            }
        }
    }
    Vector3 ogBackgroundPos;
    Vector3 ogPos;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        ogBackgroundPos = backgroundToParallax.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CanParallax = true;
        }   
        if (CanParallax) { 
        
        Vector2 parallaxDistance = new Vector2 ((transform.position.x - ogPos.x) * parallaxXFactor, (transform.position.y - ogPos.y) * parallaxYFactor);
        backgroundToParallax.transform.position = new Vector2(ogBackgroundPos.x + parallaxDistance.x, ogBackgroundPos.y + parallaxDistance.y);
        }
    }
}
