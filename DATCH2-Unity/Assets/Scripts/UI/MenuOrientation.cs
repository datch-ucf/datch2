using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Microsoft.MixedReality.Toolkit.Utilities;

/// <summary>
/// Sets which hand menu should be constrained to. Also stores handedness information.
/// LOCATION: Menu. Handedness button will reference this script OnClick.
/// </summary>
public class MenuOrientation : MonoBehaviour
{
    public bool defaultMenuOrientation = true;  // Default hand orientation is when menu is constrained to left hand
    public Handedness dominantHand = Handedness.Right;
    private GameObject pen;
    private GameObject menu;

    void Start()
    {
        pen = Camera.main.transform.GetChild(1).gameObject;
        menu = GameObject.Find("Menu");
    }

    private void Update()
    {
        
    }

    // Switches dominant hand on press
    public void ToggleHandSwitch()
    {
        if (defaultMenuOrientation == true) // if menu tracked to left hand, switch to right hand
        {
            gameObject.GetComponent<SolverHandler>().TrackedHandedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(-0.12f, -0.194f, -0.154f); // repositions menu with correct offset
            gameObject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(-57.93f, 0.0f, 0.0f); // repositions menu with correct offset
            defaultMenuOrientation = false;
            dominantHand = Handedness.Left;
            pen.GetComponent<SolverHandler>().TrackedHandedness = gameObject.GetComponent<MenuOrientation>().dominantHand;
        }

        else if (defaultMenuOrientation == false) // if menu tracked to right hand, switch to left hand
        {
            gameObject.GetComponent<SolverHandler>().TrackedHandedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left;
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0.169f, -0.194f, -0.154f); // repositions menu with correct offset
            gameObject.transform.GetChild(0).transform.localRotation = Quaternion.Euler(-57.93f, 0.0f, 0.0f); // repositions menu with correct offset
            defaultMenuOrientation = true;
            dominantHand = Handedness.Right;
            pen.GetComponent<SolverHandler>().TrackedHandedness = gameObject.GetComponent<MenuOrientation>().dominantHand;
        }
    }
}
