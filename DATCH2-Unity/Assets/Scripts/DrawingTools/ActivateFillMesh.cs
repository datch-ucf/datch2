using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script adds a fillMesh to a selected shape and can update its color.
/// LOCATION: Fill button
/// </summary>
public class ActivateFillMesh : MonoBehaviour
{
    private GameObject gestureContainer;
    public GameObject selectedShape;
    public GameObject fillMeshPrefab;
    public Material twoSidedMat;
    SpriteRenderer colorDraggerSR;
    public Material fillColorPickerGradientMat;
    public GameObject dragger;
    Color colorDraggerColor;
    public Interactable fillButton;
    public GameObject fillButtonGameObject;
    private GameObject newFill = null;
    public AudioClip invalidFillSound;
    private AudioSource fillButtonAudio;
    public AudioClip validFillSound;
    public GameObject menuPanels;

    // Start is called before the first frame update
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
        colorDraggerSR = dragger.GetComponent<SpriteRenderer>();
        fillButton.OnClick.AddListener(() => UpdateFillColor());
        fillButtonAudio = gameObject.GetComponent<AudioSource>();
    }

    public void ActivateFillOnSelectedShape()
    {
        selectedShape = gestureContainer.GetComponent<SelectionManager>().selectedShape;

        if (selectedShape.transform.Find("FillMesh(Clone)") == null && selectedShape.transform.GetComponentInChildren<LineRenderer>().loop == true)  // Only fills closed shapes
        {
            newFill = Instantiate(fillMeshPrefab);
            newFill.GetComponent<Renderer>().material = new Material(twoSidedMat);
            newFill.transform.parent = selectedShape.transform;

            // Play valid fill sound and move to fill attributes panel if user fills a closed shape
            fillButtonAudio.PlayOneShot(validFillSound, 1);
            menuPanels.GetComponent<SlidingMenuPanels>().SlideMenuPanelForward();
        }

        // Play error sound and stay at current panel if user attempts to fill unclosed shape
        else if (selectedShape.transform.Find("FillMesh(Clone)") == null && selectedShape.transform.GetComponentInChildren<LineRenderer>().loop == false)
        {
            fillButtonAudio.PlayOneShot(invalidFillSound, 1);
        }
    }

    public void UpdateFillColor()
    {
        selectedShape = gestureContainer.GetComponent<SelectionManager>().selectedShape;

        colorDraggerColor = colorDraggerSR.color;

        if (newFill != null)
        {
            if (colorDraggerColor != newFill.GetComponent<MeshRenderer>().material.color)
            {
                selectedShape.transform.Find("FillMesh(Clone)").gameObject.GetComponent<Renderer>().material.SetColor("_Color", colorDraggerColor);
            }
        }
    }
}