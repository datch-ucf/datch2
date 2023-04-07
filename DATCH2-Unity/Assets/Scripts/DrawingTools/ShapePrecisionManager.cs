using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;

public class ShapePrecisionManager : MonoBehaviour
{
    /// <summary>
    /// Allows for more precision when manipulating shape by snapping. These elements are directly on shape.
    /// LOCATION: Shape prefab 
    /// </summary>

    public bool snappingEnabled = false;

    // Rotation Precision Variables
    public int rotSnapIncrement;
    private string rotTextX, rotTextY, rotTextZ;
    private TMP_Text rotationTextOutput;
    private Vector3 amountRotated, initialRot, amountRotatedNeg;
    private float initialRotAmt = 0, maxRotAmt = 360;
    public ConstraintSource grabbedHandleConstraintSource = new ConstraintSource();
    private GameObject rotationAnglesDisplayCanvas, grabbedHandle;
    public GameObject radialButton1, radialButton2, radialButton3, rotationIncrementsButtons, precisionMenuExtended, gestureContainer;
    public GameObject precisionSettingsManager;


    // Start is called before the first frame update
    void Start()
    {
        amountRotated.x = 0;
        amountRotated.y = 0;
        amountRotated.z = 0;
        rotationTextOutput = GameObject.Find("SnappingRotationTextCanvas").transform.GetComponentInChildren<TMP_Text>();

        precisionSettingsManager = GameObject.Find("PrecisionSettingsManager");
        gestureContainer = precisionSettingsManager.GetComponent<PrecisionSettings>().gestureContainer.gameObject;
        precisionMenuExtended = precisionSettingsManager.GetComponent<PrecisionSettings>().precisionMenuExtended.gameObject;
        rotationIncrementsButtons = precisionSettingsManager.GetComponent<PrecisionSettings>().rotationIncrementsButtons.gameObject;
        radialButton1 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton1.gameObject;
        radialButton2 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton2.gameObject;
        radialButton3 = precisionSettingsManager.GetComponent<PrecisionSettings>().radialButton3.gameObject;
        rotSnapIncrement = 0;

        ///NOTE: MAKE SURE THE RADIAL BUTTONS ACTUALLY CHANGE THE CURRENT SELECTED SHAPE'S ROT SNAPPING VALUE (MAYBE STILL PREVENT FROM CLICKING IF A SHAPE IS NOT CURRENTLY SELECTED
    }

    void Update()
    {
        // ROTATION PRECISION (SNAPPING & SHOWING ANGLES)

        if (snappingEnabled)
        {
            // Position text near grabbed handle position
            if (grabbedHandle != null)
            {
                rotationTextOutput.transform.position = grabbedHandle.transform.position;
                //rotationAnglesDisplayCanvas.transform.position = grabbedHandle.transform.position;
            }
        }
    }

    // Runs after everything else in update has run
    void LateUpdate()
    {
        if (snappingEnabled == true && gameObject.CompareTag("Shape"))
        {

            // Allows rotation snapping
            gameObject.transform.localEulerAngles = new Vector3(
            (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));



            // Show how much the shape has rotated
            amountRotated = gameObject.transform.localEulerAngles - initialRot;
            amountRotatedNeg = new Vector3(amountRotated.x - 360, amountRotated.y - 360, amountRotated.z - 360);


            //Show "+" in front if amountRotated is positive
            if (amountRotated.x > 0)
            {
                rotTextX = "x: +" + Mathf.Round(amountRotated.x).ToString() + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)"; // Convert the amount rotated to string (Rounding to prevent accidental decimals)
                rotTextY = "";
                rotTextZ = "";
            }

            else if (amountRotated.x == -360)
            {
                rotTextX = "x: +" + 0 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)";
            }

            else if (amountRotated.x < -360)
            {
                rotTextX = "x: +" + Mathf.Round(amountRotated.x).ToString() + 360 + "\u00B0" + "; (" + Mathf.Round(amountRotatedNeg.x).ToString() + "\u00B0)";
            }

            // Display one angle at a time
            else
            {
                rotTextX = "";
            }

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

            rotationTextOutput.SetText(rotTextX + "\n" + rotTextY + "\n" + rotTextZ);
        }

        if (snappingEnabled == true && gameObject.CompareTag("DrawingPlane"))
        {

            // Allows rotation snapping
            gameObject.transform.localEulerAngles = new Vector3(
            (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));
        }
    }

    // Placed on RotateStarted() in Bounds Control component. Allows snapped rotation only when using the rotation handles.
    public void EnableRotSnapping()
    {
        if (rotSnapIncrement != 0)
        {
            initialRot = gameObject.transform.localEulerAngles;
            grabbedHandle = Camera.main.transform.GetChild(0).GetComponent<SelectionManager>().selectedComponent;
            grabbedHandleConstraintSource.sourceTransform = grabbedHandle.transform;


            rotTextX = "x: 0\u00B0";
            rotTextY = "y: 0\u00B0";
            rotTextZ = "z: 0\u00B0";

            snappingEnabled = true;
        }
    }

    public void DisableRotSnapping()
    {
        initialRot = new Vector3(0, 0, 0);


        rotationTextOutput.SetText("");
        snappingEnabled = false;

        // Prevents shape from snapping back to unsnapped position
        gameObject.transform.localEulerAngles = new Vector3(
            (Mathf.Round(transform.localEulerAngles.x / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.y / rotSnapIncrement) * rotSnapIncrement),
            (Mathf.Round(transform.localEulerAngles.z / rotSnapIncrement) * rotSnapIncrement));

        amountRotated.x = 0;
        amountRotated.y = 0;
        amountRotated.z = 0;

        
    }

    // Rotation snapping settings for each radial button
    public void RotSnapSetting1()
    {
        rotSnapIncrement = 10;
    }

    public void RotSnapSetting2()
    {
        rotSnapIncrement = 15;
    }

    public void RotSnapSetting3()
    {
        rotSnapIncrement = 45;
    }


    // Used to correct to proper rotation snapping OnHover()
    public void ChangeRotSnapSetting()
    {
        if (rotationIncrementsButtons.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 10 && precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingCheckbox.gameObject.activeInHierarchy == true)
        {
            rotSnapIncrement = 10;
        }

        if (rotationIncrementsButtons.transform.GetChild(1).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 15 && precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingCheckbox.gameObject.activeInHierarchy == true)
        {
            rotSnapIncrement = 15;
        }

        if (rotationIncrementsButtons.transform.GetChild(2).GetChild(0).GetChild(3).gameObject.activeInHierarchy == true && rotSnapIncrement != 45 && precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingCheckbox.gameObject.activeInHierarchy == true)
        {
            rotSnapIncrement = 45;
        }

        // Turn off rotation snapping if box is not chec
        if (precisionSettingsManager.GetComponent<PrecisionSettings>().rotSnappingCheckbox.gameObject.activeInHierarchy == false)
        {
            snappingEnabled = false;
            rotSnapIncrement = 0;
        }
    }

    public void TogglePointsSnapping()
    {
        // This should activate a checkbox that toggles snapping of shape points
    }

    //public void GenerateTapeMeasure()
    //{
    //    print("Instantiate a tape measure");
    //}

}
