using Microsoft.MixedReality.Toolkit.Rendering;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImagePlaneAttributes : MonoBehaviour
{
    public float alphaValue;
    public GameObject opacityValue;

    // Cursor display variables
    public GameObject[] cursorVisuals;
    public bool firstShowCursor;
    public bool isHovering;

    // Removing image background
    //public GameObject testPlane;
    public Texture2D textureNoBG;
    public Texture2D originalTexture;

    // Start is called before the first frame update
    void Start()
    {
        originalTexture = gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;
    }

    public void Update()
    {
        if (isHovering == true)
        {
            // update stored cursorVisuals array if hovering over shape (whether selected or unselected)
            cursorVisuals = GameObject.FindGameObjectsWithTag("CursorVisual");

            // Cursor visuals do not show properly on first hover. This flag prevents the issue.
            if (firstShowCursor == false)
            {
                ShowCursor();
                firstShowCursor = true;
            }

            ShowCursor();
        }
    }

    public void ChangeOpacity(SliderEventData eventData)
    {
        // Save slider value as alpha value (runs from 0 to 1)
        alphaValue = eventData.Slider.SliderValue;
        // Update material alpha value when slider is changed
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(gameObject.GetComponent<Renderer>().material.color.r, gameObject.GetComponent<Renderer>().material.color.g, gameObject.GetComponent<Renderer>().material.color.b, alphaValue));
        // Change displayed opacity value as a percentage
        opacityValue.GetComponent<TextMesh>().text = "Opacity: " + (alphaValue*100).ToString("F0") + "%";
    }

    public void CheckIsHovering()
    {
        isHovering = true;
    }

    // Call OnHoverExit
    public void UnCheckIsHovering()
    {
        isHovering = false;
    }


    public void ShowCursor()
    {
        foreach (GameObject visual in cursorVisuals)
        {
            visual.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideCursor()
    {
        foreach (GameObject visual in cursorVisuals)
        {
            visual.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void RemoveImageBG()
    {
        originalTexture = gameObject.GetComponent<Renderer>().material.mainTexture as Texture2D;

        // This only works properly if Read/Write enabled is set to true in the texture settings.
        // If you create a texture from a script, however, Read/Write enabled should already be set to true by default.

        Color[] texturePixels = originalTexture.GetPixels(0, 0, originalTexture.width, originalTexture.height);

        for (int i = 0; i < texturePixels.Length; i++)
        {
            if (texturePixels[i] == Color.white)
            {
                texturePixels[i] = new Color(0, 0, 0, 0);
            }
        }

        textureNoBG = new Texture2D(originalTexture.width, originalTexture.height);
        textureNoBG.SetPixels(texturePixels);
        textureNoBG.Apply();
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", textureNoBG);
    }

    public void ToggleImgBackground()
    {
        if(textureNoBG == null)
        {
            RemoveImageBG();
        }
        
        // If the texture with no background has already been generated, allow user to toggle background on and off
        else if(textureNoBG != null)
        {
            // Switch to original texture
            if (gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex") == textureNoBG)
            {
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", originalTexture);
            }

            // Switch to no bg texture
            else if(gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex") == originalTexture)
            {
                gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", textureNoBG);
            }
        }
        
    }
}
