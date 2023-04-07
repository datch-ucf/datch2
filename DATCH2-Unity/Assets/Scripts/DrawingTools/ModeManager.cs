using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Keeps track of which drawing mode is currently being used.
/// LOCATION: Attached to gestureContainer
/// </summary>

public class ModeManager : MonoBehaviour
{
    private GameObject gestureContainer;
    public bool open, save, clearAll, import, importImage, communicate, toggleMesh, precision, accessibility;  // File buttons
    public bool freeDraw = true, line, curve, drawingPlane, undo, delete, pegGrid, fill, measure;  // Tools buttons 
    public GameObject currentModeIndicator; // Note: FreeDraw mode should be active (true) at default; default value for bool is false for all other modes
    // Add trigger states for attributes buttons as well

    void Start()
    {
        gestureContainer = gameObject;
        currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: FreeDraw";

    }
    public void SetDrawingMode()
    {
        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "OpenButton")
        {
            open = true;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;
       

            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Open";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "SaveButton")
        {
            open = false;
            save = true;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;
            // precision = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Save";
        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "ClearAllButton")
        {
            open = false;
            save = false;
            clearAll = true;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Clear All";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "DeleteButton")
        {
            open = false;
            save = false;
            clearAll = false; 
            delete = true;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Delete";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "ImportButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = true;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Import";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "ImportImageButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = true;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Import Image";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "CommunicateButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = true;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Communicate";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "Toggle Mesh")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = true;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Toggle Mesh";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "PrecisionButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = true;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Precision";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "AccessibilityButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = true;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Accessibility";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "Free Draw")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            freeDraw = true;
            line = false;
            curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Free Draw";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "Line")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            freeDraw = false;
            line = true;
            curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Line";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "Curve")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            freeDraw = false;
            line = false;
            curve = true;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Curve";
        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "DrawingPlane")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = true;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Drawing Plane";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "UndoButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = true;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Undo";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "Delete")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = true;
            pegGrid = false;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Delete";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "PegGridButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false; 
            delete = false;
            pegGrid = true;
            fill = false;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Marker Peg Grid";

        }

        if (gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "FillButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = true;
            measure = false;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Fill";
        }

        if(gestureContainer.GetComponent<SelectionManager>().activatedButtonGameObject.name == "MeasureButton")
        {
            open = false;
            save = false;
            clearAll = false;
            delete = false;
            import = false;
            importImage = false;
            communicate = false;
            toggleMesh = false;
            precision = false;
            accessibility = false;


            // These three modes may need to remain true in some cases.
            // freeDraw = false;
            // line = false;
            // curve = false;
            drawingPlane = false;
            undo = false;
            delete = false;
            pegGrid = false;
            fill = false;
            measure = true;

            currentModeIndicator.GetComponent<TMP_Text>().text = "Mode: Measure";
        }
    }
}
