using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables shape bounding box if lineRenderer is closed
/// LOCATION: shape prefab
/// </summary>
public class ShapeBoundsDisplayHelper : MonoBehaviour
{
    private GameObject gestureContainer;
    public bool shapeIsClosed;

    private void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponentInChildren<LineRenderer>().loop == true && gestureContainer.GetComponent<SelectionManager>().selectedShape.gameObject == gameObject)
        {
            print("LOOPED>> SHOW");
            shapeIsClosed = true;
            //gameObject.GetComponent<BoundsControl>().BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateByProximityAndPointer;
            gameObject.GetComponent<BoundsControl>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else if (gameObject.GetComponentInChildren<LineRenderer>().loop == false && gestureContainer.GetComponent<SelectionManager>().selectedShape.gameObject == gameObject)
        {
            print("NOT LOOPED >> DISABLE");
            shapeIsClosed = false;
            //gameObject.GetComponent<BoundsControl>().BoundsControlActivation = Microsoft.M ixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateManually;
            gameObject.GetComponent<BoundsControl>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
