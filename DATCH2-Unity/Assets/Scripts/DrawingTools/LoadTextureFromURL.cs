using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

public class LoadTextureFromURL : MonoBehaviour
{
    public string textureURL = "";
    public GameObject imagePlanePrefab;
    private GameObject createdImagePlane;
    public Material urlImageMat;
    public Texture obtainedTexture;
    public float obtainedTextureRatio;
    public Vector3 textureBasedPlaneScale;
    public float scaleFactor = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DownloadImage(textureURL, imagePlanePrefab));

        // Test textures
        ////textureURL = "https://i.pinimg.com/originals/ad/51/10/ad5110cbe5c560cb916d137c6eb181b7.png";
        ////textureURL = "https://t3.ftcdn.net/jpg/03/15/34/90/360_F_315349043_6ohfFyx37AFusCKZtGQtJR0jqUxhb25Y.jpg";
        //textureURL = "https://turkisharchaeonews.net/sites/tan/files/pictures/sites/kerkenes/kerkenes_07.jpg";
    }


    IEnumerator DownloadImage()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(textureURL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            obtainedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;  // Get texture from URL
            createdImagePlane.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = obtainedTexture;  // Apply texture to material on plane

            // Resize plane according to texture dimensions
            obtainedTextureRatio = (float)obtainedTexture.height / (float)obtainedTexture.width;  // Ratio of image (height/width)
            textureBasedPlaneScale = new Vector3((float)obtainedTexture.width * obtainedTextureRatio * scaleFactor, 1, (float)obtainedTexture.height * obtainedTextureRatio * scaleFactor); // Scale factor makes the image a more manageable scale for user to grab and manipulate
            createdImagePlane.transform.GetChild(0).localScale = textureBasedPlaneScale;  // Resize plane mesh based on dimensions of texture


            //Reposition Slider
            //createdImagePlane.transform.GetChild(0).GetChild(0).position = new Vector3(createdImagePlane.transform.GetChild(0).GetComponent<BoxCollider>().bounds.min.z, createdImagePlane.transform.GetChild(0).GetChild(0).position.y, createdImagePlane.transform.GetChild(0).GetChild(0).position.x);
        }
    }

    // For now, only allow creation of one image plane at a time
    public void GenerateNewImagePlane()
    {
        // Delete image plane if one already exists
        if (createdImagePlane != null)
        {
            Destroy(createdImagePlane);
        }

        //Generate a new image plane
        createdImagePlane = Instantiate(imagePlanePrefab);
        
        
        
        StartCoroutine(DownloadImage());
     
        // Positions image plane in front of user and perpendicular to floor
        createdImagePlane.transform.LookAt(Camera.main.transform); // drawing plane faces camera
        //transform.LookAt(2 * createdImagePlane.transform.position - Camera.main.transform.position);
        createdImagePlane.transform.localEulerAngles = new Vector3(0.0f, createdImagePlane.transform.localEulerAngles.y, 0.0f); // sets plane perpendicular to floor
        createdImagePlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 1.0f; // Pushes plane forward to just in front of the user
        createdImagePlane.transform.Rotate(90, 0, 0);
        createdImagePlane.transform.position = new Vector3(createdImagePlane.transform.position.x, Camera.main.transform.position.y, createdImagePlane.transform.position.z); // reposition height of plane based on camera's y position (as opposed to fwd vector)

        // Enable Proximity Effect for Handles
        createdImagePlane.transform.GetChild(0).GetComponent<BoundsControl>().HandleProximityEffectConfig.ProximityEffectActive = true;

        // Store object in a reference list in DrawObjGenerator
        //Camera.main.transform.GetChild(0).GetComponent<DrawObjGenerator>().imagePlane = createdImagePlane;
        //Camera.main.transform.GetChild(0).GetComponent<DrawObjGenerator>().imagePlanesList.Add(createdImagePlane);

        
    }
}