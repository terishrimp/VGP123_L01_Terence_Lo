using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBorders : MonoBehaviour
{
    [Header("Background Parallax Settings")]
    [SerializeField] GameObject backgroundToParallax;
    [SerializeField] float parallaxXFactor = 0.5f;
    [SerializeField] float parallaxYFactor = 0.5f;
    [SerializeField] bool canParallax;


    [Header("Parallax Border Settings")]
    [SerializeField] Transform target = null;
    [SerializeField] float rightBorderBuffer = 2f;
    [SerializeField] float leftBorderBuffer = -2f;
    [SerializeField] float topBorderBuffer = 5f;
    [SerializeField] float bottomBorderBuffer = -5f;

    public float RightBorderBuffer
    {
        get { return rightBorderBuffer; }
        set
        {
            if (value == rightBorderBuffer) return;
            if (value < 0) rightBorderBuffer = 0;
            else rightBorderBuffer = value;
        }
    }
    public float LeftBorderBuffer
    {
        get { return leftBorderBuffer; }

        set
        {
            if (value == leftBorderBuffer) return;
            if (value > 0) leftBorderBuffer = 0;
            else leftBorderBuffer = value;
        }
    }
    public float TopBorderBuffer
    {
        get { return topBorderBuffer; }

        set
        {
            if (value == topBorderBuffer) return;
            if (value < 0) topBorderBuffer = 0;
            else topBorderBuffer = value;
        }
    }
    public float BottomBorderBuffer
    {
        get { return bottomBorderBuffer; }

        set
        {
            if (value == bottomBorderBuffer) return;
            if (value > 0) bottomBorderBuffer = 0;
            else bottomBorderBuffer = value;
        }
    }
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
    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        ogBackgroundPos = backgroundToParallax.transform.position;

        if (target == null)
        {
            target = FindObjectOfType<PlayerMovement>().gameObject.transform;
        }
        targetPos = target.transform.position;
        if (rightBorderBuffer < 0) rightBorderBuffer = 0;
        if (leftBorderBuffer > 0) leftBorderBuffer = 0;
        if (topBorderBuffer < 0) topBorderBuffer = 0;
        if (bottomBorderBuffer > 0) bottomBorderBuffer = 0;

        transform.position = targetPos;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateBorderPos();
        UpdateParallaxBackgroundPos();
    }

    void UpdateBorderPos()
    {
        targetPos = target.transform.position;

        if (targetPos.x > transform.position.x + RightBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x + (targetPos.x - (transform.position.x + RightBorderBuffer)), transform.position.y);
        }
        else if (targetPos.x < transform.position.x + LeftBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x + (targetPos.x - (transform.position.x + LeftBorderBuffer)), transform.position.y);
        }

        if (targetPos.y > transform.position.y + TopBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (targetPos.y - (transform.position.y + TopBorderBuffer)));
        }
        else if (targetPos.y < transform.position.y + BottomBorderBuffer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (targetPos.y - (transform.position.y + BottomBorderBuffer)));
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

    public void SetBackgroundToParallax(GameObject backgroundToParallax)
    {
        this.backgroundToParallax = backgroundToParallax;
    }

    public void SetNewBorderValues(float right, float left, float top, float bottom)
    {
        RightBorderBuffer = right;
        LeftBorderBuffer = left;
        TopBorderBuffer = top;
        BottomBorderBuffer = bottom;
    }
}
