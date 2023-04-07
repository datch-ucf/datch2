using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

// This script creates a brush point
public class FreeDraw : MonoBehaviour
{
    public GameObject brushPoint;


    void Start()
    {

    }
    public void CreateBrushPoint(MixedRealityPointerEventData eventData)
    {
        // print("Creating a brushpoint");
        var result = eventData.Pointer.Result;
        var newPointPosition = result.Details.Point;
        GameObject newBrushPoint = Instantiate(brushPoint, result.Details.Point, Quaternion.LookRotation(result.Details.Normal));
        

    }

}