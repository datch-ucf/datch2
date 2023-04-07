using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows the user to change the distance of the current pointer's raycast
/// LOCATION: 
/// </summary>


public class RaycastLengthManager : MonoBehaviour
{
    public bool raycast = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(raycast == true)
        {
            print("Calling on updated");
            UpdateRayCast();
        }
    }

    public void UpdateRayCast()
    {
        CoreServices.FocusProvider.PrimaryPointer.BaseCursor.DefaultCursorDistance = 10;
        raycast = false;
    }

    //IEnumerator DelayedRayCastChange()
    //{
    //    print("Changing");
    //    CoreServices.FocusProvider.PrimaryPointer.BaseCursor.DefaultCursorDistance = 0;
    //    yield return new WaitForSeconds(2);
    //    print("Changing Again");
    //    CoreServices.FocusProvider.PrimaryPointer.BaseCursor.DefaultCursorDistance = 10;
    //    raycast = false;

    //}
}
