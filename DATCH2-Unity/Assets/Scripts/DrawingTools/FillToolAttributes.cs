using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

// This script controls all attributes of line and curve objects (weight, color, etc.) and the line weight slider

public class FillToolAttributes : MonoBehaviour
{
    SpriteRenderer colorDraggerSR;
    public Material fillColorPickerGradientMat;
    public GameObject dragger;
    public GameObject fillMeshPrefab;
    Color colorDraggerColor;
    GameObject selectedShape;
    public GameObject gestureContainer;
    public Material twoSidedMat;
    public Interactable fillButton;
    public GameObject fillButtonGameObject;

    void Start()
    {
        colorDraggerSR = dragger.GetComponent<SpriteRenderer>();
        fillButton.OnClick.AddListener(() => UpdateFillColor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFillColor()
    {
        if(gestureContainer.GetComponent<SelectionManager>().selectedComponent.transform.parent.gameObject != null)
        {
            selectedShape = fillButtonGameObject.GetComponent<ActivateFillMesh>().selectedShape;
        }
        colorDraggerColor = colorDraggerSR.color;

        if (colorDraggerColor != selectedShape.transform.GetChild(0).GetComponent<MeshRenderer>().material.color)
        {
            selectedShape.transform.GetChild(0).GetComponent<Renderer>().material = new Material(twoSidedMat);
            selectedShape.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", colorDraggerColor);
        }
    }
}
