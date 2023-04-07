using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DESCRIPTION: This script holds useful functions that can be used in other scripts.
/// LOCATION: Script is static. Do not attach to a gameObject.
/// </summary>

public static class AdditionalUtilityFunctions
{
     public static float Remap (this float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }

    // Global scale is read-only. This function is a workaround. Assign the returned value to a shape's localScale
    public static Vector3 SetGlobalScale(GameObject shape, Transform transform, Vector3 globalScale)
    {
        shape.transform.localScale = Vector3.one;
        shape.transform.localScale = new Vector3(globalScale.x / shape.transform.lossyScale.x, globalScale.y / shape.transform.lossyScale.y, globalScale.z / shape.transform.lossyScale.z);
        return transform.localScale;
    }

    public static Vector3 Midpoint(GameObject obj1, GameObject obj2)
    {
        Vector3 midpoint = new Vector3((obj1.transform.position.x + obj2.transform.position.x) / 2, (obj1.transform.position.y + obj2.transform.position.y) / 2, (obj1.transform.position.z + obj2.transform.position.z) / 2);
        return midpoint;
    }

    public static float RoundToNearestTenth(float value)
    {
        return (value * 10f) / 10f;
    }

    public static float RoundToNearestHundredth(float value)
    {
        return (Mathf.Round(value * 100f) / 100f);
    }
}
