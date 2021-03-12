using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Canvas))]
public class BaseCanvas : MonoBehaviour
{

    protected Canvas myCanvas;
    protected CanvasManager myCanvasManager;
    public CanvasManager MyCanvasManager
    {
        get { return myCanvasManager; }
        set
        {
            if (value == myCanvasManager) return;
            myCanvasManager = value;
        }
    }
    public Canvas MyCanvas
    {
        get { return myCanvas; }
    }

    protected virtual void Awake()
    {
        if (!myCanvas)
            myCanvas = GetComponent<Canvas>();

        if (!myCanvasManager)
            myCanvasManager = transform.parent.GetComponent<CanvasManager>();

    }
}
