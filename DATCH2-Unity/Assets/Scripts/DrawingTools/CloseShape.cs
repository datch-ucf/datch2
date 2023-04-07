using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manually closes and fills the selected shape on button press.
/// LOCATION: Close shape button
/// </summary>

public class CloseShape : MonoBehaviour
{
    public GameObject selectedShape;
    public string selectedShapeType;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Call OnClick
    // When you manually close selected shape, it will also fill.
    public void CloseSelectedShape()
    {
        selectedShape = Camera.main.transform.GetChild(0).GetComponent<SelectionManager>().selectedShape;
        selectedShapeType = Camera.main.transform.GetChild(0).GetComponent<SelectionManager>().selectedShapeType;

        if (selectedShapeType == "FreeDraw")
        {
            selectedShape.GetComponentInChildren<DrawFreeDrawLine>().manualClose = true;
            StartCoroutine(FillAfterDelay());  // Wait to fill shape until after it is closed
        }

        if (selectedShapeType == "PolyLine")
        {
            selectedShape.GetComponentInChildren<DrawPolyLine>().manualClose = true;
            StartCoroutine(FillAfterDelay());  // Wait to fill shape until after it is closed
        }

        if (selectedShapeType == "Curve")
        {
            selectedShape.GetComponentInChildren<DrawBezierCurve4P>().manualClose = true;
            StartCoroutine(FillAfterDelay());  // Wait to fill shape until after it is closed
        }
    }

    IEnumerator FillAfterDelay()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.GetComponent<ActivateFillMesh>().ActivateFillOnSelectedShape();  // Wait to fill shape until after it is closed
    }
}
