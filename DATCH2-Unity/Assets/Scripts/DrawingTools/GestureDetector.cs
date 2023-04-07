using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all hand/finger poses and tracks pinching and grabbing
/// LOCATION: GestureContainer (child of camera)
/// </summary>
public class GestureDetector : MonoBehaviour
{
    // Gesture Detection Variables
    private float pinchDistance;
    private float indexFingerCurl;
    private float middleFingerCurl;
    private float ringFingerCurl;
    private float pinkyFingerCurl;
    private float thumbFingerCurl;
    private bool isIndexFingerGrabbing;
    private bool isMiddleFingerGrabbing;

    private const float pinchThreshold = 0.7f;
    private const float grabThreshold = 0.3f;

    public bool isPinching = false;
    public bool isGrabbing = false;

    private GameObject menu;
    private GameObject pen;

    void Start()
    {
        menu = GameObject.Find("Menu");
        pen = Camera.main.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting pinch status for dominant hand
        pinchDistance = HandPoseUtils.CalculateIndexPinch(menu.GetComponent<MenuOrientation>().dominantHand); // Setting a threshold with this if statement
        indexFingerCurl = HandPoseUtils.IndexFingerCurl(menu.GetComponent<MenuOrientation>().dominantHand);
        middleFingerCurl = HandPoseUtils.MiddleFingerCurl(menu.GetComponent<MenuOrientation>().dominantHand);
        ringFingerCurl = HandPoseUtils.RingFingerCurl(menu.GetComponent<MenuOrientation>().dominantHand);
        pinkyFingerCurl = HandPoseUtils.PinkyFingerCurl(menu.GetComponent<MenuOrientation>().dominantHand);
        thumbFingerCurl = HandPoseUtils.ThumbFingerCurl(menu.GetComponent<MenuOrientation>().dominantHand);
        isIndexFingerGrabbing = HandPoseUtils.IsIndexGrabbing(menu.GetComponent<MenuOrientation>().dominantHand);
        isMiddleFingerGrabbing = HandPoseUtils.IsMiddleGrabbing(menu.GetComponent<MenuOrientation>().dominantHand);

        // Grab is detected if not pinching and if middle, ring, and thumb are all within the grabThreshold position
        // Ignoring pinky here, but if issues arise, check to see if pinkyFingerCurl is necessary.
        if (isPinching == false && (middleFingerCurl > grabThreshold && ringFingerCurl > grabThreshold && thumbFingerCurl > grabThreshold))
        {
            isGrabbing = true;
        }

        else
        {
            isGrabbing = false;
        }

        // included indexFingerCurl bool to ensure that pinch detection isn't lost at certain angles
        if (isGrabbing == false && ((pinchDistance > pinchThreshold) || (indexFingerCurl > 0.3f)))
        {
            isPinching = true;
        }

        else
        {
            isPinching = false;
        }
    }
}
