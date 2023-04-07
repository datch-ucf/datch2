using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingPlaneSnappingCheckbox : MonoBehaviour
{
    public GameObject drawingPlane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallToggleSnapping()
    {
        drawingPlane = Camera.main.transform.GetChild(0).GetComponent<DrawObjGenerator>().drawingPlane;

        if(drawingPlane != null)
        {
            drawingPlane.transform.GetChild(0).GetComponent<DrawingPlaneManager>().ToggleGridPointSnapping();
        }
    }

}
