using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateSurfaceCheckbox : MonoBehaviour
{
   
    public void ToggleManipulateSurface()
    {
        GameObject activeDrawingPlane = GameObject.FindGameObjectWithTag("DrawingPlane");
        // Later, set so that checkbox only appears if drawingPlane is present in scene
        //if(activeDrawingPlane == null)
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    gameObject.SetActive(true);
        //}
      
        if (gameObject.transform.GetChild(0).GetChild(5).gameObject.activeInHierarchy == true)
        {
            activeDrawingPlane.GetComponent<DrawingPlaneManager>().drawOnPlane = false;
            activeDrawingPlane.GetComponent<ObjectManipulator>().enabled = true;
        }
        else if (gameObject.transform.GetChild(0).GetChild(5).gameObject.activeInHierarchy == false)
        {
            activeDrawingPlane.GetComponent<DrawingPlaneManager>().drawOnPlane = true;
            activeDrawingPlane.GetComponent<ObjectManipulator>().enabled = false;
        }
    }

}
