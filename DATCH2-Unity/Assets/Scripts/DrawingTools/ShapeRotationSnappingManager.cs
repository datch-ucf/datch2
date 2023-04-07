using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ShapeRotationSnappingManager : MonoBehaviour
{
    /// <summary>
    /// Allows for more precision (via snapping) when rotating a shape or drawing plane. Refers to many variables from PrecisionSettings.cs script.
    /// LOCATION: Shape prefab 
    /// </summary>

    public bool rotSnappingEnabled;
    public int rotSnapIncrement;
    private string rotTextX, rotTextY, rotTextZ;
    private TMP_Text rotationTextOutput;
    private Vector3 amountRotated, initialRot, amountRotatedNeg;
    private float initialRotAmt = 0, maxRotAmt = 360;
    public ConstraintSource grabbedHandleConstraintSource = new ConstraintSource();
    private GameObject grabbedHandle;
    public GameObject radialButton1, radialButton2, radialButton3, rotationIncrementsButtons, precisionMenuExtended, gestureContainer;
    public GameObject precisionSettingsManager;
    public GameObject rotationSnapCanvas;


    // Start is called before the first frame update
    void Start()
    {
        precisionSettingsManager = GameObject.Find("PrecisionSettingsManager");
        radialButton1 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton1.gameObject;
        radialButton2 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton2.gameObject;
        radialButton3 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton3.gameObject;
        rotSnapIncrement = precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnapIncrement; // Starting rotation snapping increment
        rotSnappingEnabled = precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingEnabled;
        rotationSnapCanvas = gameObject.transform.GetChild(1).gameObject; // added
        //rotationSnapCanvas = GameObject.Find("RotationSnappingTextCanvas").gameObject;
        rotationTextOutput = rotationSnapCanvas.transform.GetComponentInChildren<TMP_Text>();
        gestureContainer = precisionSettingsManager.GetComponent<PrecisionSettings>().gestureContainer.gameObject;
        precisionMenuExtended = precisionSettingsManager.GetComponent<PrecisionSettings>().precisionMenuExtended.gameObject;
        rotationIncrementsButtons = precisionSettingsManager.GetComponent<PrecisionSettings>().rotationIncrementsButtons.gameObject;
        // Retrieving all necessary variables
        amountRotated.x = 0;
        amountRotated.y = 0;
        amountRotated.z = 0;
        rotationSnapCanvas.SetActive(false);


    }

    void Update()
    {

        // Check if snapping is enabled and how much shape should snap (MAY NEED TO CHANGE LATER FOR OPTIMIZATION)
        rotSnapIncrement = precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnapIncrement;
        rotSnappingEnabled = precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingEnabled;

        // ROTATION PRECISION (SNAPPING)       
        // Lock canvas to center of shape
        rotationSnapCanvas.transform.position = gameObject.GetComponent<Collider>().bounds.center;
        rotationSnapCanvas.transform.localPosition = new Vector3(rotationSnapCanvas.transform.localPosition.x, rotationSnapCanvas.transform.localPosition.y, rotationSnapCanvas.transform.localPosition.z + 0.25f);

        // Rotate canvas towards camera/user
        rotationSnapCanvas.transform.LookAt(2 * transform.position - Camera.main.transform.position); // Allow the text to always face the user


    }

    // Runs after everything else in update has run. This allows the delayed snapping when shape is rotated
    void LateUpdate()
    {
        if (rotSnappingEnabled == true && gameObject.CompareTag("Shape") || rotSnappingEnabled == true && gameObject.CompareTag("DrawingPlane"))
        {

            // Round rotation to allow for snapping
            gameObject.transform.localEulerAngles = new Vector3(
            (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));



            // Show how much the shape has rotated
            amountRotated = gameObject.transform.localEulerAngles - initialRot;
            amountRotatedNeg = new Vector3(amountRotated.x - 360, amountRotated.y - 360, amountRotated.z - 360);

            // x-axis Rotation
            //Show "+" in front if amountRotated is positive
            if (amountRotated.x > 0)
            {
                rotTextX = "x: +" + Mathf.Round(amountRotated.x).ToString() + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)"; // Convert the amount rotated to string (Rounding to prevent accidental decimals)
                rotTextY = "";
                rotTextZ = "";
            }

            // If shape has rotated -360 degrees, display this as 0 degrees
            else if (amountRotated.x == -360)
            {
                rotTextX = "x: +" + 0 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)";
            }

            // If shape has rotated less than -360 degrees, start over count and continue adding (to mimic going around unit circle).
            else if (amountRotated.x < -360)
            {
                rotTextX = "x: +" + Mathf.Round(amountRotated.x).ToString() + 360 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)";
            }

            // Display one angle at a time
            else
            {
                rotTextX = "";
            }

            // y-axis Rotation
            if (amountRotated.y > 0)
            {
                rotTextY = "y: +" + Mathf.Round(amountRotated.y).ToString() + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.y).ToString() + "\u00B0)"; // Convert the amount rotated to string (Rounding to prevent accidental decimals)
                rotTextX = "";
                rotTextZ = "";
            }
            else if (amountRotated.y == -360)
            {
                rotTextY = "y: +" + 0 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.y).ToString() + "\u00B0)";
            }
            else if (amountRotated.y < -360)
            {
                rotTextY = "y: +" + Mathf.Round(amountRotated.y).ToString() + 360 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.y).ToString() + "\u00B0)";
            }

            else
            {
                rotTextY = "";
            }

            if (amountRotated.z > 0)
            {
                rotTextZ = "z: +" + Mathf.Round(amountRotated.z).ToString() + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.z).ToString() + "\u00B0)"; // Convert the amount rotated to string (Rounding to prevent accidental decimals)
                rotTextX = "";
                rotTextY = "";
            }
            else if (amountRotated.z == -360)
            {
                rotTextX = "z: +" + 0 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.z).ToString() + "\u00B0)";
            }
            else if (amountRotated.z < -360)
            {
                rotTextZ = "z: +" + Mathf.Round(amountRotated.z).ToString() + 360 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.z).ToString() + "\u00B0)";
            }
            else
            {
                rotTextZ = "";
            }

            // Rotation will display as specified:
            rotationTextOutput.SetText(rotTextX + "\n" + rotTextY + "\n" + rotTextZ);

        }

        // Snapping for drawing plane
        //if (precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingEnabled == true && gameObject.CompareTag("DrawingPlane"))
        //{

        //    // Allows rotation snapping
        //    gameObject.transform.localEulerAngles = new Vector3(
        //    (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
        //    (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
        //    (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));
        //}
    }

    // Location: Placed on RotateStarted() in Bounds Control component of Shape prefab. Allows snapped rotation only when using the rotation handles.
    // Occurs when rotation snapping checkbox is checked OnClick()
    public void StartRotSnapping()
    {
        rotationSnapCanvas.SetActive(true);

        initialRot = gameObject.transform.localEulerAngles;
        grabbedHandle = Camera.main.transform.GetChild(0).GetComponent<SelectionManager>().selectedComponent;
        grabbedHandleConstraintSource.sourceTransform = grabbedHandle.transform;


        rotTextX = "x: 0\u00B0";
        rotTextY = "y: 0\u00B0";
        rotTextZ = "z: 0\u00B0";

    }

    // Location: Placed on RotateStarted() in Bounds Control component of Shape prefab. Allows snapped rotation only when using the rotation handles.
    // Occurs when rotation snapping checkbox is unchecked OnClick() 
    public void StopRotSnapping()
    {
        initialRot = new Vector3(0, 0, 0);


        // ------------------ Commenting out the following to prevent issue with shape suddenly snapping out of place ------------------
        // Prevents shape from snapping back to unsnapped position
        //gameObject.transform.localEulerAngles = new Vector3(
        //    (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
        //    (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
        //    (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));

        amountRotated.x = 0;
        amountRotated.y = 0;
        amountRotated.z = 0;

        rotationTextOutput.SetText("");
        rotationSnapCanvas.SetActive(false);

    }
}
