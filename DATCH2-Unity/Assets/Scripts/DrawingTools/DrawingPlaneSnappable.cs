using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows this gameObject to snap to the drawing plane grid. The object must also has isTrigger checked on its collider. 
/// LOCATION: Snappable gameObject (i.e. drawing point)
/// </summary>

public class DrawingPlaneSnappable : MonoBehaviour
{
    public GameObject pen;
    public GameObject drawingPlaneCollisionTarget;

    // Start is called before the first frame update
    void Start()
    {
        pen = Camera.main.transform.GetChild(1).gameObject;
        drawingPlaneCollisionTarget = pen.GetComponent<PenTool>().physicalPenTip.GetComponent<PenTipHelper>().drawingPlaneCollisionTarget;
}

    // Update is called once per frame
    void Update()
    {

    }

    // Snap this gameObject to gridPoint whenever it almost touches gridpoint intersection
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("SnapPoint"))
        {
            gameObject.transform.position = other.transform.position;
            drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SnapPoint"))
        {
            drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = false;
        }
    }


}
