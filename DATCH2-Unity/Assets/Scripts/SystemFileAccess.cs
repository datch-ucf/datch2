using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using TMPro;
using UnityEngine.Rendering;

#if ENABLE_WINMD_SUPPORT
    using Windows.Storage;
    using Windows.Storage.Streams;
    using Windows.Storage.Pickers;
#endif

/// <summary>
/// Provides access to the OneDrive folder
/// LOCATION: Import button?
/// </summary>

public class SystemFileAccess : MonoBehaviour
{
    // Image plane variables
    private string fileName;
    private Texture2D selectedTexture;
    private string selectedImagePath;
    private float selectedTextureRatio;
    private Vector3 textureBasedPlaneScale;
    private float scaleFactor = 0.0005f;
    public GameObject imagePlanePrefab;
    public GameObject generatedImagePlane;
    public List<GameObject> generatedImagePlanes;

    // Output text file variables
    public string logFileName;
    public string currentLogTime = "";
    public string previousLogTime = "";
    public bool logFolderIsSelected = false;

    // if running on HoloLens, declare FileOpenPicker variable. If not running on HoloLens, variable is not declared.
#if ENABLE_WINMD_SUPPORT
        private FileOpenPicker openPicker;
#endif

    void Start()
    {
        // Names the log file when the application starts. A new log file is created each day.
        NameLogTextFile();
        // A new file is created with a unique name each time the app starts.
        //logFileName = "DATCH_LOG - " + System.DateTime.Now.ToString();

        // Create the log text file at start of the application. Text file is created in the local folder.
        //CreateLogTextFile();
        //hololensStatus.GetComponentInChildren<TextMeshProUGUI>().text = "Called CreateLogTextFile()";
        //hololensStatus.GetComponentInChildren<TextMeshProUGUI>().text = logFileName;

#if ENABLE_WINMD_SUPPORT
         UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
         {
             NameLogTextFile();
         }, false);
#endif
    }

    // Use to select a folder for exporting text file (implement later)
    public void SelectFolder()
    {
#if ENABLE_WINMD_SUPPORT
         UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
         {
             var folderPicker = new Windows.Storage.Pickers.FolderPicker();
             folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
             folderPicker.FileTypeFilter.Add("*");
    
             Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
             if (folder != null)
             {
                 // Application now has read/write access to all contents in the picked folder
                 // (including other sub-folder contents)
                 Windows.Storage.AccessCache.StorageApplicationPermissions.
                 FutureAccessList.AddOrReplace("PickedFolderToken", folder);
             }
         }, false);
#endif
    }

    // Use to import image for and generate a new image plane
    public void SelectImageFile()
    {
#if ENABLE_WINMD_SUPPORT
         // Async events in HoloLens itself
         UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
         {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
            filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            // Only show the following file types
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");
    
            Windows.Storage.StorageFile file = await filePicker.PickSingleFileAsync();
            
            // Async events in application environment (Unity). You can call Unity functions here.
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                GenerateNewImagePlane(file.Path);
            }, false);

         }, false);
#endif
    }

    private void GetSelectedImage(string selectedImagePath)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(selectedImagePath);
        selectedTexture = new Texture2D(2, 2); //mock size 2x2. Does not matter here because LoadImage will overwrite.
        selectedTexture.LoadImage(bytes);
        generatedImagePlane.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = selectedTexture;

        // Resize plane according to texture dimensions
        selectedTextureRatio = (float)selectedTexture.height / (float)selectedTexture.width;  // Ratio of image (height/width)
        textureBasedPlaneScale = new Vector3((float)selectedTexture.width * selectedTextureRatio * scaleFactor, 1, (float)selectedTexture.height * selectedTextureRatio * scaleFactor); // Scale factor makes the image a more manageable scale for user to grab and manipulate
        generatedImagePlane.transform.GetChild(0).localScale = textureBasedPlaneScale;  // Resize plane mesh based on dimensions of texture
    }

    private void GenerateNewImagePlane(string selectedImagePath)
    {
        //Generate a new image plane
        generatedImagePlane = Instantiate(imagePlanePrefab, new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z) + Camera.main.transform.forward * 0.5f, Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0));
        generatedImagePlane.transform.Rotate(0, 180, 0);
        generatedImagePlanes.Add(generatedImagePlane);

        GetSelectedImage(selectedImagePath);

        //// Positions image plane in front of user and perpendicular to floor
        //generatedImagePlane.transform.LookAt(Camera.main.transform); // drawing plane faces camera
        ////transform.LookAt(2 * generatedImagePlane.transform.position - Camera.main.transform.position);
        //generatedImagePlane.transform.localEulerAngles = new Vector3(0.0f, generatedImagePlane.transform.localEulerAngles.y, 0.0f); // sets plane perpendicular to floor
        //generatedImagePlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 1.0f; // Pushes plane forward to just in front of the user
        //generatedImagePlane.transform.Rotate(90, 0, 0);
        //generatedImagePlane.transform.position = new Vector3(generatedImagePlane.transform.position.x, Camera.main.transform.position.y, generatedImagePlane.transform.position.z); // reposition height of plane based on camera's y position (as opposed to fwd vector)

        // Positions drawing plane in front of user and perpendicular to floor
        //generatedImagePlane.transform.LookAt(Camera.main.transform); // drawing plane faces camera
        //generatedImagePlane.transform.localEulerAngles = new Vector3(0.0f, generatedImagePlane.transform.localEulerAngles.y, 0.0f); // sets drawing plane perpendicular to floor
        //generatedImagePlane.transform.position = Camera.main.transform.localPosition + Camera.main.transform.forward * 0.75f; // Pushes drawing plane forward to just in front of the user
        //generatedImagePlane.transform.position = new Vector3(generatedImagePlane.transform.position.x, Camera.main.transform.position.y, generatedImagePlane.transform.position.z); // reposition height of plane based on camera's y position (as opposed to fwd vector)

        // Enable Proximity Effect for Handles
        generatedImagePlane.transform.GetChild(0).GetComponent<BoundsControl>().HandleProximityEffectConfig.ProximityEffectActive = true;

        // Store object in a reference list in DrawObjGenerator
        //Camera.main.transform.GetChild(0).GetComponent<DrawObjGenerator>().imagePlane = generatedImagePlane;
        //Camera.main.transform.GetChild(0).GetComponent<DrawObjGenerator>().imagePlanesList.Add(generatedImagePlane);
    }

    private void NameLogTextFile()
    {
        //logFileName = "DATCH_LOG - " + System.DateTime.Now.ToLongDateString() + "-" + System.DateTime.Now.ToLongTimeString() + ".txt";
        logFileName = "DATCH_LOG - " + System.DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
        //print(logFileName);
    }

    private void WriteToTextFile()
    {
#if ENABLE_WINMD_SUPPORT
// Async events in application environment (Unity). You can call Unity functions here.
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                logFolderIsSelected = true;
                previousLogTime = currentLogTime;
                currentLogTime = previousLogTime + "\n" + "LOG_TIME - " + System.DateTime.Now.ToString();
            }, false);
// Async events in HoloLens itself
    UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
    {
        if (Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.ContainsItem("PickedFolderToken")) 
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync("PickedFolderToken");
            Windows.Storage.StorageFile logFile = await storageFolder.CreateFileAsync(logFileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(logFile, currentLogTime);
        }
    }, false);
#endif
    }

    public void SaveLogToSelectedFolder()
    {
        // Only open file picker if first time writing to log
        //if(logFolderIsSelected == false)
        //{
            SelectFolder();
            WriteToTextFile();
        //}
    }
}
