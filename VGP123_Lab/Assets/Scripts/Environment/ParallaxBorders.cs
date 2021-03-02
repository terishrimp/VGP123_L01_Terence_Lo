using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBorders : MonoBehaviour
{
    [SerializeField] PlayerMovement player = null;
    [SerializeField] float rightBorderBuffer = 2f;
    [SerializeField] float leftBorderBuffer = -2f;
    [SerializeField] float topBorderBuffer = 5f;
    [SerializeField] float bottomBorderBuffer = 5f;
    [SerializeField] bool parallaxOnXAxis = false;
    [SerializeField] bool positiveValue = true;

    public Vector3 playerPos;
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
        }
        playerPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        position = transform.position;
        if (parallaxOnXAxis)
        {
            if (positiveValue)
            {
                if (playerPos.x >= transform.position.x)
                {
                    transform.parent.position = new Vector2(transform.parent.position.x + (playerPos.x - transform.position.x), transform.parent.position.y);
                }
            }
            else
            {
                if (playerPos.x <= transform.position.x)
                {
                    transform.parent.position = new Vector2(transform.parent.position.x + (playerPos.x - transform.position.x), transform.parent.position.y);
                }
            }
        }
        else
        {
            if (positiveValue)
            {
                if (playerPos.y >= transform.position.y)
                {
                    transform.parent.position = new Vector2(transform.parent.position.x, transform.parent.position.y + (playerPos.y - transform.position.y));
                }
            }
            else
            {
                if (playerPos.y <= transform.position.y)
                {
                    transform.parent.position = new Vector2(transform.parent.position.x, transform.parent.position.y + (playerPos.y - transform.position.y));
                }
            }
        }
    }

    
}
