using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;

public class PrecisionSettings : MonoBehaviour
{
    /// <summary>
    /// Keeps track of key variables and global settings for precision (i.e. rotation snapping). These elements are present in scene (mainly menu). Holds variables for ShapeRotationSnappingManager.cs.
    /// LOCATION: PrecisionSettingsManager gameObject
    /// </summary>


    // Rotation Precision Variables
    public GameObject radialButton1, radialButton2, radialButton3, rotationIncrementsButtons;
    public GameObject precisionMenuExtended;
    public GameObject gestureContainer;
    public GameObject rotSnappingCheckbox;
    public bool rotSnappingEnabled;
    public int rotSnapIncrement;

    // Start is called before the first frame update
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        precisionMenuExtended = GameObject.Find("Menu").transform.GetChild(0).GetChild(1).GetChild(2).GetChild(15).gameObject;
        rotationIncrementsButtons = precisionMenuExtended.transform.GetChild(0).GetChild(0).GetChild(3).gameObject;
        radialButton1 = rotationIncrementsButtons.transform.GetChild(0).gameObject;
        radialButton2 = rotationIncrementsButtons.transform.GetChild(1).gameObject;
        radialButton3 = rotationIncrementsButtons.transform.GetChild(2).gameObject;
        rotSnappingCheckbox = precisionMenuExtended.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(5).gameObject;
        rotSnappingEnabled = false;
    }


    // Turns rotation snapping on and off when user presses checkbox
    public void ToggleRotSnapping()
    {
        // Toggle rotation snapping OFF
        if (rotSnappingEnabled == true)
        {
            rotSnappingEnabled = false;
            //rotSnapIncrement = 0; // Just in case measure
        }

        // Toggle rotation snapping ON 
        else
        {
            rotSnappingEnabled = true;

            //// Update the rotation snapping increment to match selected radial button (just in case measure)
            //if(rotationIncrementsButtons.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true)
            //{
            //    rotSnapIncrement = 10;
            //}

            //if (rotationIncrementsButtons.transform.GetChild(1).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true)
            //{
            //    rotSnapIncrement = 15;
            //}

            //if (rotationIncrementsButtons.transform.GetChild(2).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true)
            //{
            //    rotSnapIncrement = 45;
            //}
        }
    }

    // Rotation snapping settings for each radial button
    public void RotSnapSetting1()
    {
        rotSnapIncrement = 10;
    }

    public void RotSnapSetting2()
    {
        rotSnapIncrement = 15;
    }

    public void RotSnapSetting3()
    {
        rotSnapIncrement = 45;
    }


    //// Used to correct to proper rotation snapping OnHover()
    //public void ChangeRotSnapSetting()
    //{
    //    if (rotationIncrementsButtons.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 10 && rotSnappingCheckbox.gameObject.activeInHierarchy == true)
    //    {
    //        rotSnapIncrement = 10;
    //    }

    //    if (rotationIncrementsButtons.transform.GetChild(1).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 15 && rotSnappingCheckbox.gameObject.activeInHierarchy == true)
    //    {
    //        rotSnapIncrement = 15;
    //    }

    //    if (rotationIncrementsButtons.transform.GetChild(2).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 45 && rotSnappingCheckbox.gameObject.activeInHierarchy == true)
    //    {
    //        rotSnapIncrement = 45;
    //    }
    //}

}
