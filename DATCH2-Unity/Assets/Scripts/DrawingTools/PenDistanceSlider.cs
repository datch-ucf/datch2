using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Allows slider to influence the length of the pen tool.
/// LOCATION: Pen Length Slider (attached to menu)
/// </summary>
public class PenDistanceSlider : MonoBehaviour
{
    private GameObject pen;
    private GameObject physicalPenTip;
    private TextMesh penDistanceSliderText;
    public float exactPenDistance;
    private float sliderVal;
    public float adjustedSliderVal;
    private float yOffset = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Locating correct GameObjects here just in case public variables do not save after merge
        pen = Camera.main.transform.GetChild(1).gameObject;
        physicalPenTip = pen.transform.GetChild(0).GetChild(0).gameObject;
        penDistanceSliderText = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMesh>();
        
        // Set pen distance to 0 at start
        physicalPenTip.transform.localScale = new Vector3(physicalPenTip.transform.localScale.x, 0, physicalPenTip.transform.localScale.z);

        exactPenDistance = 0.003000006f; // initializing here to prevent error

        ChangePenDistance(); // Initial setup function call
    }

    void Update()
    {
        // changes the value of the penDistance variable to the correct actual distance value
        exactPenDistance = pen.GetComponent<PenTool>().penTipDistance = Vector3.Distance(pen.GetComponent<PenTool>().tip.transform.position, pen.GetComponent<PenTool>().tipBase.transform.position);
    }
    
    // Function is called when slider value is changed
    public void ChangePenDistance()
    {
        if(pen != null)
        {
            // Use the following two lines instead of noted lines in DrawObjGenerator to ensure pen tip is showing when manipulating slider
            pen.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            physicalPenTip.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            sliderVal = gameObject.GetComponent<PinchSlider>().SliderValue;
            adjustedSliderVal = AdditionalUtilityFunctions.Remap(sliderVal, 0.0f, 1.0f, 0.1f, (10.0f / 3.0f));  // adjust mapping of values to work properly with slider (1000/3 makes slider max = 10 m)
            pen.GetComponent<PenTool>().penTipDistance = adjustedSliderVal; // will need to adjust later to work with correct distance value. Some disparity because pen tip is scaled instead of translated
            physicalPenTip.transform.localScale = new Vector3(physicalPenTip.transform.localScale.x, adjustedSliderVal, physicalPenTip.transform.localScale.z);
            penDistanceSliderText.text = AdditionalUtilityFunctions.RoundToNearestTenth(exactPenDistance*100).ToString("F0") + " cm";  // convert to cm and round to nearest whole number
        }

        // make box collider larger if pen tip is not extended and if box collider size has not already been adjusted
        if (exactPenDistance <= 0.1f && physicalPenTip.transform.GetChild(0).GetComponent<BoxCollider>().size != new Vector3(physicalPenTip.transform.GetChild(0).localScale.x, physicalPenTip.transform.GetChild(0).localScale.y + 75.0f, physicalPenTip.transform.GetChild(0).localScale.z))
        {
            // Minimal distance (not extended), so make box collider larger
            physicalPenTip.transform.GetChild(0).GetComponent<BoxCollider>().size = new Vector3(physicalPenTip.transform.GetChild(0).localScale.x, physicalPenTip.transform.GetChild(0).localScale.y + 75.0f, physicalPenTip.transform.GetChild(0).localScale.z);
        }
        // otherwise, make box collider the size of pen tip model (if it isn't already the size of the pen tip model)
        else if (exactPenDistance > 0.1f && physicalPenTip.transform.GetChild(0).GetComponent<BoxCollider>().size != physicalPenTip.transform.GetChild(0).localScale)
        {
            // Non-minimal distance (extended), so make box collider the size of the pen tip model
            physicalPenTip.transform.GetChild(0).GetComponent<BoxCollider>().size = new Vector3(physicalPenTip.transform.GetChild(0).localScale.x, 1.0f, physicalPenTip.transform.GetChild(0).localScale.z);
        }
    }
}
