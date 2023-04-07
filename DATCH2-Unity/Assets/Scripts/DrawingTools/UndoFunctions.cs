using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UndoFunctions : MonoBehaviour
{
    private GameObject gestureContainer;
    public GameObject selectedComponent;

    private Vector3 selectedComponentPreviousPos;
    private Quaternion selectedComponentPreviousRot;
    //private Vector3 selectedComponentPreviousScale;
    private Vector3 selectedComponentCurrentPos;
    private Quaternion selectedComponentCurrentRot;
    //private Vector3 selectedComponentCurrentScale;

    // Stores the transform data of a selected component. This script can be expanded later to include object history or a redo function if needed.
    // Script currently only stores the current and previous position of a gameObject.
    void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Undo should not be used for freeDrawing shapes or brushPoints
    public void Undo()
    {
        selectedComponent = gestureContainer.GetComponent<SelectionManager>().selectedComponent;

        if (selectedComponent.tag != "BrushPoint")
        {
            selectedComponent = gestureContainer.GetComponent<SelectionManager>().selectedComponent;  // Refers to a specific grab point

            if (selectedComponent.tag == "ManipulationButton")
            {
                selectedComponent = selectedComponent.transform.parent.parent.gameObject;  // Refers to shape (grandparent of manipulation button)

            }

            selectedComponentPreviousPos = selectedComponent.GetComponent<ComponentStateManager>().selectPos;
            selectedComponentCurrentPos = selectedComponent.GetComponent<ComponentStateManager>().manipulationPos;
            selectedComponentPreviousRot = selectedComponent.GetComponent<ComponentStateManager>().selectRot;
            selectedComponentCurrentRot = selectedComponent.GetComponent<ComponentStateManager>().manipulationRot;
            //selectedComponentPreviousScale = selectedComponent.GetComponent<ComponentStateManager>().selectScale;
            //selectedComponentCurrentScale = selectedComponent.GetComponent<ComponentStateManager>().manipulationScale;
        }
        if (selectedComponentPreviousPos != selectedComponentCurrentPos)
        {
            selectedComponent.transform.position = new Vector3(selectedComponentPreviousPos.x, selectedComponentPreviousPos.y, selectedComponentPreviousPos.z);
            //print("Changed pos");
        }

        if (selectedComponentPreviousRot != selectedComponentCurrentRot)
        {
            selectedComponent.transform.rotation = Quaternion.Euler(selectedComponentPreviousRot.x, selectedComponentPreviousRot.y, selectedComponentPreviousRot.z);
            //print("changed rot");
        }

        //if (selectedComponentPreviousScale != selectedComponentCurrentScale)
        //{
        //    selectedComponent.transform.localScale = new Vector3(selectedComponentPreviousScale.x, selectedComponentPreviousScale.y, selectedComponentPreviousScale.z);
        //    //print("changed scale");
        //}

    }

}
