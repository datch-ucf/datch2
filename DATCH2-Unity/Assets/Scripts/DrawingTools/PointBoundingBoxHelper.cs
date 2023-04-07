using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates bounding box and box collider for shape when hoving over points within the shape
/// LOCATION: FreeDrawGenerator, FreeDrawPoint, PolyLineGenerator, GrabPoint, EndGrabPoint (i.e. points on all drawing types)
/// </summary>
public class PointBoundingBoxHelper : MonoBehaviour
{
    private GameObject shape; // the shape parented to the point/grabPoint/freeDrawPoint

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // activate OnHoverEnter()
    public void DeactivateBoundingBox()
    {
        StartCoroutine(WaitToDeactivateBoundingBox());
    }

    IEnumerator WaitToDeactivateBoundingBox()
    {
        yield return new WaitForSeconds(0.001f);
        shape = gameObject.transform.parent.gameObject;
        if (shape.tag == "Curve") // if parent is a curve instead of shape, get parent of the curve
        {
            shape = shape.transform.parent.gameObject;
        }
        shape.GetComponent<BoundsControl>().enabled = false;
        shape.GetComponent<BoxCollider>().enabled = false;

        // Prevents users from accidentally moving or rotating the shape when moving a point within it
        shape.GetComponent<MoveAxisConstraint>().enabled = true;
        shape.GetComponent<RotationAxisConstraint>().enabled = true;
    }

    // activate OnHoverExit()
    public void ReactivateBoundingBox()
    {
        shape = gameObject.transform.parent.gameObject;
        if (shape.tag == "Curve") // if parent is a curve instead of shape, get parent of the curve
        {
            shape = shape.transform.parent.gameObject;
        }
        shape.GetComponent<BoundsControl>().enabled = true;
        shape.GetComponent<BoxCollider>().enabled = true;

        // Allows users to move or rotate the point's shape again
        shape.GetComponent<MoveAxisConstraint>().enabled = false;
        shape.GetComponent<RotationAxisConstraint>().enabled = false;

    }
}
