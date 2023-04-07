using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows miniPegGrid to be used like a joystick to move the pegMarkerGrid. pegMarkerGrid moves in corresponding direction when miniPegGrid placed ontop of that button (relative to camera).
/// LOCATION: MiniPegGridComponents gameObject
/// </summary>

public class DPadControls : MonoBehaviour
{
    private GameObject forward, backward, left, right, forwardLeft, forwardRight, backwardLeft, backwardRight, center;
    private GameObject pegGrid;
    private GameObject miniGridDPad;
    private GameObject gestureContainer;
    private GameObject menu;

    // Start is called before the first frame
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        menu = GameObject.Find("Menu");
        pegGrid = GameObject.Find("PegGrid");
        miniGridDPad = menu.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetChild(2).gameObject;


        forward = miniGridDPad.transform.GetChild(3).gameObject;
        backward = miniGridDPad.transform.GetChild(5).gameObject;
        left = miniGridDPad.transform.GetChild(1).gameObject;
        right = miniGridDPad.transform.GetChild(7).gameObject;
        forwardLeft = miniGridDPad.transform.GetChild(0).gameObject;
        forwardRight = miniGridDPad.transform.GetChild(6).gameObject;
        backwardLeft = miniGridDPad.transform.GetChild(2).gameObject;
        backwardRight = miniGridDPad.transform.GetChild(8).gameObject;
        center = miniGridDPad.transform.GetChild(4).gameObject;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == forward)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.01f); // Moves relative to initial pegGrid rotation
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.forward * 0.01f; // Move relative to camera
        }
        if (other.gameObject == backward)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.01f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) - Camera.main.transform.forward * 0.01f;
        }
        if (other.gameObject == left)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.right * -0.01f;
        }
        if (other.gameObject == right)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.right * 0.01f;
        }
        if (other.gameObject == forwardLeft)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.01f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * 0.01f + Camera.main.transform.right * -0.01f);
        }
        if (other.gameObject == forwardRight)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.01f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * 0.01f + Camera.main.transform.right * 0.01f);
        }
        if (other.gameObject == backwardLeft)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.01f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * -0.01f + Camera.main.transform.right * -0.01f);
        }
        if (other.gameObject == backwardRight)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.01f, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.01f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * -0.01f + Camera.main.transform.right * 0.01f);
        }
        if (other.gameObject == center)
        {
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z);
        }
    }

    // ** LATER, ONLY LET IT ROTATE WHEN IN CENTER

    // Move peg Grid by half a meter (precision): Call OnTouchStart (in Inspector)
    // Only change position if user is not pinching (not moving the miniPegMarkerGrid), which prevents accidental activations
    public void precisionMoveForward()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.forward * 0.5f;
        }
    }
 
    public void precisionMoveBackward()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.forward * -0.5f;
        }
    }

    public void precisionMoveLeft()
    {
        //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.5f, pegGrid.transform.position.y, pegGrid.transform.position.z);
        pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.right * -0.5f;
    }

    public void precisionMoveRight()
    {
        //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.5f, pegGrid.transform.position.y, pegGrid.transform.position.z);
        pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + Camera.main.transform.right * 0.5f;
    }

    public void precisionMoveForwardLeft()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.5f, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * 0.5f + Camera.main.transform.right * -0.5f);
        }
    }

    public void precisionMoveForwardRight()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.51f, pegGrid.transform.position.y, pegGrid.transform.position.z + 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * 0.5f + Camera.main.transform.right * 0.5f);
        }
    }

    public void precisionMoveBackwardLeft()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x - 0.5f, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * -0.5f + Camera.main.transform.right * -0.5f);
        }
    }

    public void precisionMoveBackwardRight()
    {
        if (gestureContainer.GetComponent<GestureDetector>().isPinching == false)
        {
            //pegGrid.transform.position = new Vector3(pegGrid.transform.position.x + 0.5f, pegGrid.transform.position.y, pegGrid.transform.position.z - 0.5f);
            pegGrid.transform.position = new Vector3(pegGrid.transform.position.x, pegGrid.transform.position.y, pegGrid.transform.position.z) + (Camera.main.transform.forward * -0.5f + Camera.main.transform.right * 0.5f);

        }
    }

}
