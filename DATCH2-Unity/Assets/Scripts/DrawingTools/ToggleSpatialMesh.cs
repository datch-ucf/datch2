using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

/// <summary>
/// Show and hide the spatial mesh (2 variations)
/// LOCATION: ToggleMesh button in file menu
/// </summary>
public class ToggleSpatialMesh : MonoBehaviour
{
    public Material pulseMat;
    public Material wireframeMat;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Spatial Mesh has been replaced with scene understanding
    public void ToggleShowHide()
    {
        // Get the first Mesh Observer available, generally we have only one registered
        var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

        //print("Current: " + observer.DisplayOption);

        //if (observer.DisplayOption == SpatialAwarenessMeshDisplayOptions.Visible)
        //{
        //    observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.None;
        //}

        //else if (observer.DisplayOption == SpatialAwarenessMeshDisplayOptions.Occlusion)
        //{
        //    observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.None;
        //}
        if (observer.VisibleMaterial == wireframeMat)
        {
            observer.VisibleMaterial = pulseMat;
        }

        else if (observer.VisibleMaterial == pulseMat)
        {
            observer.VisibleMaterial = wireframeMat;
        } 
    }
}
