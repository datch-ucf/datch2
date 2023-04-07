using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script prevents issues with point sizing when shape scale is changed with bounds handles
/// LOCATION: BrushPoints, GrabPoints
/// </summary>
public class PointScaleManager : MonoBehaviour
{
    private Vector3 scale;

    private void Start()
    {
        scale = gameObject.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = AdditionalUtilityFunctions.SetGlobalScale(gameObject, gameObject.transform, scale);
    }
}
