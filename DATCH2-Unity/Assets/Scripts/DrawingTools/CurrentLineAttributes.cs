using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

/// <summary>
/// Controls all attributes of line and curve objects (weight, color, etc.) and the line weight slider
/// LOCATION:Container (grandchild of LineMenuExtended)
/// **Note: LineRendererOrigin and BezierCurve4P must be changed to white manually for now.
/// </summary>

public class CurrentLineAttributes : MonoBehaviour
{
    private float defaultLineWidth = 0.005f;
    
    public LineRenderer lineRendererOrigin;

    public LineRenderer curvePrefab;

    Color colorDraggerColor;

    SpriteRenderer colorDraggerSR;

    public Material colorPickerGradientMat;
                
    public GameObject slider;

    public TMP_Text sliderText;

    public GameObject weightSliderIcon;

    public GameObject dragger;

    private GameObject selectedShape;

    private GameObject gestureContainer;

    private void Awake()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        colorDraggerSR = dragger.GetComponent<SpriteRenderer>();
        SwitchAllLinesToDefault();
    }
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        colorDraggerSR = dragger.GetComponent<SpriteRenderer>();
        SwitchAllLinesToDefault();
    }

    // Update is called once per frame
    void Update()
    {
        if(gestureContainer.GetComponent<SelectionManager>().selectedShape != null)
        {
            selectedShape = gestureContainer.GetComponent<SelectionManager>().selectedShape;
        }
        colorDraggerColor = colorDraggerSR.color;

        // Changes the color of freeDraw lines and polyLine lines
        if (colorDraggerColor != lineRendererOrigin.startColor)
        {
            ChangeLineColor();
        }

        // Changes the color of curves
        if (colorDraggerColor != curvePrefab.startColor)
        {
            ChangeCurveColor();
        }
    }

    // Changes the weight of freeDraw lines and polyLine lines
    public void ChangeLineWeight()
    {
        if (gestureContainer.GetComponent<ModeManager>().freeDraw == true || gestureContainer.GetComponent<ModeManager>().line == true)
        {
            //selectedShape = gestureContainer.GetComponent<SelectionManager>().selectedShape;

            lineRendererOrigin.startWidth = AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0025f, 0.01f);
            lineRendererOrigin.endWidth = AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0025f, 0.01f);
            // Updates the slider UI values with an arbitrary number and icon that changes in size
            sliderText.text = Mathf.Clamp(((float)System.Math.Round(AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0f, 10.0f), 2)), 0.1f, 10.0f).ToString();

            var scaleValue = Mathf.Clamp(slider.GetComponent<PinchSlider>().SliderValue / 20.0f, 0.01f, slider.GetComponent<PinchSlider>().SliderValue / 20.0f);
            //weightSliderIcon.transform.localScale = new Vector3(slider.GetComponent<PinchSlider>().SliderValue/20.0f, slider.GetComponent<PinchSlider>().SliderValue/20.0f, slider.GetComponent<PinchSlider>().SliderValue/20.0f);
            weightSliderIcon.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);  // Clamping this value for a more aesthetically pleasing visual indicator

            //// Update current selected shape attributes
            //if (selectedShape != null)
            //{
            //    if (selectedShape.transform.GetChild(1).tag == "BrushPoint" || selectedShape.transform.GetChild(1).tag == "GrabPoint")
            //    {
            //        selectedShape.GetComponentInChildren<LineRenderer>().startWidth = lineRenderer.startWidth;
            //        selectedShape.GetComponentInChildren<LineRenderer>().endWidth = lineRenderer.endWidth;
            //    }
            //}
        }
    }

    public void ChangeCurveWeight()
    {
        if (gestureContainer.GetComponent<ModeManager>().curve == true)
        {
            curvePrefab.startWidth = AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0025f, 0.01f);
            curvePrefab.endWidth = AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0025f, 0.01f);

            // Updates the slider UI values with an arbitrary number and icon that changes in size
            sliderText.text = Mathf.Clamp(((float)System.Math.Round(AdditionalUtilityFunctions.Remap(slider.GetComponent<PinchSlider>().SliderValue, 0.0f, 1.0f, 0.0f, 10.0f), 2)), 0.1f, 10.0f).ToString();

            var scaleValue = Mathf.Clamp(slider.GetComponent<PinchSlider>().SliderValue / 20.0f, 0.01f, slider.GetComponent<PinchSlider>().SliderValue / 20.0f);
            weightSliderIcon.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);  // Clamping this value for a more aesthetically pleasing visual indicator

            //// Update current selected shape attributes
            //if (selectedShape != null)
            //{
            //    if (selectedShape.transform.GetChild(1).tag == "Curve")
            //    {
            //        selectedShape.transform.GetChild(1).GetComponent<LineRenderer>().startWidth = curvePrefab.startWidth;
            //        selectedShape.transform.GetChild(1).GetComponent<LineRenderer>().endWidth = curvePrefab.endWidth;
            //    }
            //}
            
        }
    }


    public void ChangeLineColor()
    {
        // update this script later to change the specific instance of the line instead of the lineRendererOrigin prefab
        lineRendererOrigin.startColor = colorDraggerColor;
        lineRendererOrigin.endColor = colorDraggerColor;

        //if (selectedShape != null)
        //{
        //    if (selectedShape.transform.GetChild(1).tag == "BrushPoint" || selectedShape.transform.GetChild(1).tag == "GrabPoint")
        //    {
        //        selectedShape.GetComponentInChildren<LineRenderer>().startColor = lineRendererOrigin.startColor;
        //        selectedShape.GetComponentInChildren<LineRenderer>().endColor = lineRendererOrigin.endColor;
        //    }
        //}
    }

    public void ChangeCurveColor()
    {
        curvePrefab.startColor = colorDraggerColor;
        curvePrefab.endColor = colorDraggerColor;

        //if(selectedShape != null)
        //{
        //    if (selectedShape.transform.GetChild(1).tag == "Curve")
        //    {
        //        selectedShape.transform.GetChild(1).GetComponent<LineRenderer>().startColor = lineRendererOrigin.startColor;
        //        selectedShape.transform.GetChild(1).GetComponent<LineRenderer>().endColor = lineRendererOrigin.endColor;
        //    }
        //}
    }

    public void SwitchAllLinesToDefault()
    {
        dragger.GetComponent<SpriteRenderer>().color = Color.white;
        lineRendererOrigin.startWidth = defaultLineWidth;
        lineRendererOrigin.endWidth = defaultLineWidth;
        lineRendererOrigin.startColor = Color.white;
        lineRendererOrigin.endColor = Color.white;

        curvePrefab.startWidth = defaultLineWidth;
        curvePrefab.endWidth = defaultLineWidth;
        curvePrefab.startColor = Color.white;
        curvePrefab.endColor = Color.white;
        colorPickerGradientMat.SetColor("_Color", Color.white);

        //slider.GetComponent<PinchSlider>().SliderValue = defaultLineWidth

    }

    void OnApplicationQuit()
    {
        SwitchAllLinesToDefault();
    }

}
