using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for collision points on the pen tip. Used for drawing on planes.
/// LOCATION: Model (child of physicalPenTip)
/// </summary>
public class PenTipHelper : MonoBehaviour
{
    // Scene references
    public GameObject pen;
    private GameObject gestureContainer;
    private GameObject tip;
    private GameObject tipBase;

    // Pen tip variables
    public Vector3 drawingPlaneCollisionPoint;
    public GameObject drawingPlaneCollisionTransform;
    public GameObject penDistanceSlider;
    public GameObject drawingPlaneCollisionTarget;
    public bool hittingDrawingPlane = false;

    // Start is called before the first frame update
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        tip = gameObject.transform.parent.GetChild(1).gameObject;
        tipBase = gameObject.transform.parent.GetChild(2).gameObject;
    }

    private void OnTriggerStay(Collider collider)
    {
        //print("HIT: " + collider.gameObject);
        if(collider.gameObject.CompareTag("DrawingPlane"))
        {
            //print("HIT PLANE");
            hittingDrawingPlane = true;

            // Stabilizes pen collisions. Note: collider.ClosestPoint appears to produce better results than collider.ClosestPointOnBounds
            drawingPlaneCollisionPoint = new Vector3(AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).x), AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).y), AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).z));
            drawingPlaneCollisionTransform.transform.position = drawingPlaneCollisionPoint;

            // Shorten pen tip if touching drawing plane
            if(tip.transform.position != drawingPlaneCollisionPoint)
            {
                // Formula for local y scale of pen tip: physicalPenTip.transform.localScale.y = (distance from tipBase to drawingPlaneCollisionPoint)/modelScale)*10 + offset (b/c pen tip still needs to pass through plane to detect collisions)
                gameObject.transform.parent.localScale = new Vector3(gameObject.transform.parent.localScale.x, ((Vector3.Distance(tipBase.transform.position, drawingPlaneCollisionPoint)/0.3f)*10)+1.0f, gameObject.transform.parent.localScale.z);
            }

            // disable target if nearly pinching and if drawOnPlane checkbox is toggled off
            //if (gestureContainer.GetComponent<GestureDetector>().isNearlyPinching == false || collider.gameObject.GetComponent<DrawingPlaneManager>().drawOnPlane == false)
            if (collider.gameObject.GetComponent<DrawingPlaneManager>() != null && collider.gameObject.GetComponent<DrawingPlaneManager>().drawOnPlane == false)
            {
                drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = false;
            }

            //if(CoreServices.FocusProvider.PrimaryPointer.Result != null)
            //{
            //    if(CoreServices.FocusProvider.PrimaryPointer.Result.CurrentPointerTarget.CompareTag("Untagged"))
            //    {
            //        drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = false;
            //    }
            //}

            // enable target if nearly pinching and if drawOnPlane checkbox is toggled on
            //if (gestureContainer.GetComponent<GestureDetector>().isNearlyPinching == true && collider.gameObject.GetComponent<DrawingPlaneManager>().drawOnPlane == true)
            if (collider.gameObject.GetComponent<DrawingPlaneManager>().drawOnPlane == true)
            {
                drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = true;
            }

            // enable target if pinching and if drawOnPlane checkbox is toggled on
            if (gestureContainer.GetComponent<GestureDetector>().isPinching == true && collider.gameObject.GetComponent<DrawingPlaneManager>().drawOnPlane == true)
            {
                drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = true;
            }

            if(gestureContainer.GetComponent<GestureDetector>().isPinching == true)
            {
                drawingPlaneCollisionPoint = new Vector3(AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).x), AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).y), AdditionalUtilityFunctions.RoundToNearestTenth(collider.ClosestPoint(gameObject.transform.position).z));
            }

            drawingPlaneCollisionTarget.transform.position = drawingPlaneCollisionTransform.transform.position;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("DrawingPlane"))
        {
            hittingDrawingPlane = false;

            // disable canvas when pen tip is not touching drawing plane
            drawingPlaneCollisionTarget.GetComponentInChildren<Canvas>().enabled = false;

            // resize pen back to distance according to PenDistanceSlider
            penDistanceSlider.GetComponent<PenDistanceSlider>().ChangePenDistance();
        }

    }
}
