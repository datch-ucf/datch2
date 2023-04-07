using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit;

/// <summary>
/// Change color scheme of menu for different themes.
/// LOCATION: Each theme button  (e.g. DefaultButton, OutdoorButton) in the ThemeMenuExtended button collection
/// </summary>
public class ThemeSettings : MonoBehaviour
{
    // Changing backplate color of main menus
    public Material defaultBackplateMaterial;
    public Texture2D defaultSpectrumMap;
    public Texture2D indoorSpectrumMap;
    public Texture2D outdoorSpectrumMap;
    // Changing text and icon color
    public Material buttonIconMaterial;
    public Material customTextMaterial;
    public Material customTextMaterialBlack;
    //public GameObject themeButton;
    public TMP_Text[] textMeshProObjects;
    //public GameObject[] iconMaterials;
    public GameObject menu;

    void Start()
    {
        menu = gameObject.transform.parent.parent.parent.gameObject;
        textMeshProObjects = menu.GetComponentsInChildren<TMP_Text>();

    }

    void OnAwake()
    {

        SwitchToDefault();
    }

    public void SwitchToDefault()
    {
        defaultBackplateMaterial.SetTexture("_IridescentSpectrumMap", defaultSpectrumMap);
        buttonIconMaterial.SetColor("_Color", Color.white);
        //customTextMaterial.SetColor("_FaceColor", Color.white);

        foreach(TMP_Text text in textMeshProObjects)
        {
            text.color = Color.white;
        }

    }

    public void SwitchToOutdoor()
    {
        defaultBackplateMaterial.SetTexture("_IridescentSpectrumMap", indoorSpectrumMap);
        buttonIconMaterial.SetColor("_Color", Color.white);
        //customTextMaterial.SetColor("_FaceColor", Color.white);
        //customTextMaterial = customTextMaterialBlack;

        foreach (TMP_Text text in textMeshProObjects)
        {
            text.color = Color.white;
        }
    }

    public void SwitchToIndoor()
    {
        defaultBackplateMaterial.SetTexture("_IridescentSpectrumMap", outdoorSpectrumMap);
        buttonIconMaterial.SetColor("_Color", Color.black);
        //customTextMaterial.SetColor("_FaceColor", Color.black);

        foreach (TMP_Text text in textMeshProObjects)
        {
            text.color = Color.black ;
        }
        //customTextMaterial = customTextMaterialBlack;
        //themeButton.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.black);
        //themeButton.GetComponent<Renderer>().material.SetColor("_Color", Color.black);

        //print("shaared mat: " + themeButton.GetComponent<Renderer>().sharedMaterial);
        //print("shaared mat name: " + themeButton.GetComponent<Renderer>().sharedMaterial.name);

    }

    void OnApplicationQuit()
    {
        SwitchToDefault();
    }

}
