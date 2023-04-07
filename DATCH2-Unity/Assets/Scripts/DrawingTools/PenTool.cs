using Microsoft.MixedReality.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Handedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness;
using Microsoft.MixedReality.Toolkit.Input;

public class PenTool : MonoBehaviour
{
    public GameObject menu;

    // Changing script to use a GameObject instead of a raycast for efficiency
    public GameObject physicalPenTip;
    public GameObject tip;
    public GameObject tipBase;
    public float penTipDistance = 0.0f;

    /// <summary>
    /// Virtual "pen" that displays when drawing modes are activated. Pen handedness is controlled by the DrawObjGenerator
    /// LOCATION: Pen prefab
    /// </summary>
    void Start()
    {
        menu = GameObject.Find("Menu");

        // Tracks pen to dominant hand
        gameObject.GetComponent<SolverHandler>().TrackedHandedness = menu.GetComponent<MenuOrientation>().dominantHand;
        tip = physicalPenTip.transform.GetChild(1).gameObject;
    }

    //// Pen stabilizer (smooths pen movement/positioning to prevent issues with hand tracking)
    //void Update()
    //{
    //    if(gameObject.GetComponentInChildren<MeshRenderer>().enabled == true)
    //    {
    //        gameObject.transform.position = new Vector3(RoundToNearestTenth(gameObject.transform.position.x), RoundToNearestTenth(gameObject.transform.position.y), RoundToNearestTenth(gameObject.transform.position.z));
    //    }

    //}

    public float RoundToNearestTenth(float value)
    {
        return (value * 10f) / 10f;
    }

    public float RoundToNearestHundredth(float value)
    {
        return(Mathf.Round(value * 100f) / 100f);
    }

}
