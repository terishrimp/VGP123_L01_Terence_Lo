using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBorders : MonoBehaviour
{
    [Header("Background Parallax Settings")]
    [SerializeField] SpriteRenderer backgroundToParallax;
    [SerializeField] float parallaxXFactor = 0.5f;
    [SerializeField] float parallaxYFactor = 0.5f;
    [SerializeField] bool canParallax;


    [Header("Parallax Border Settings")]
    [SerializeField] Transform target = null;
    [SerializeField] float rightBorderBuffer = 2f;
    [SerializeField] float leftBorderBuffer = -2f;
    [SerializeField] float topBorderBuffer = 5f;
    [SerializeField] float bottomBorderBuffer = - 5f;

    public bool CanParallax
    {
        get { return canParallax; }
        set
        {
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

    public Vector3 playerPos;
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        ogBackgroundPos = backgroundToParallax.transform.position;

        if (target == null)
        {
            target = FindObjectOfType<PlayerMovement>().gameObject.transform;
        }
        playerPos = target.transform.position;

        if (rightBorderBuffer < 0) rightBorderBuffer = 0;
        if (leftBorderBuffer > 0) leftBorderBuffer = 0;
        if (topBorderBuffer < 0) topBorderBuffer = 0;
        if (bottomBorderBuffer > 0) bottomBorderBuffer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateBorderPos();
        UpdateParallaxBackgroundPos();
    }

    void UpdateBorderPos()
    {
        playerPos = target.transform.position;
        position = transform.position;

        if (playerPos.x > transform.position.x + rightBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x + (playerPos.x - (transform.position.x + rightBorderBuffer)), transform.position.y);
        }
        else if (playerPos.x < transform.position.x + leftBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x + (playerPos.x - (transform.position.x + leftBorderBuffer)), transform.position.y);
        }

        if (playerPos.y > transform.position.y + topBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (playerPos.y - (transform.position.y + topBorderBuffer)));
        }
        else if (playerPos.y < transform.position.y + bottomBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (playerPos.y - (transform.position.y + bottomBorderBuffer)));
        }
    }

    void UpdateParallaxBackgroundPos()
    {
        if (CanParallax)
        {
            Vector2 parallaxDistance = new Vector2((transform.position.x - ogPos.x) * parallaxXFactor, (transform.position.y - ogPos.y) * parallaxYFactor);
            backgroundToParallax.transform.position = new Vector2(ogBackgroundPos.x + parallaxDistance.x, ogBackgroundPos.y + parallaxDistance.y);
        }
    }
}
