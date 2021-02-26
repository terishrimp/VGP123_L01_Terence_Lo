using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float parallaxXFactor = 0.5f;
    [SerializeField] float parallaxYFactor = 0.5f;
    bool isMoving = false;
    Vector3 initialPos;
    Vector3 initialTargetPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 parallaxDist = new Vector3((target.position.x - initialTargetPos.x)* parallaxXFactor, (target.position.y - initialTargetPos.y) * parallaxYFactor, target.position.z);

        if (isMoving)
        {
             transform.position = new Vector3(initialPos.x + parallaxDist.x, initialPos.y + parallaxDist.y, initialPos.z);
        }
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
        initialTargetPos = target.position;
    }
    public bool GetIsMoving()
    {
        initialPos = transform.position;
        return isMoving;
    }

    public void SetInitialPos(Vector3 pos)
    {
        initialPos = pos;
    }

    public void SetParallaxXFactor (float value)
    {
        parallaxXFactor = value;
    }

    public void SetParallaxYFactor(float value)
    {
        parallaxYFactor = value;
    }
}
