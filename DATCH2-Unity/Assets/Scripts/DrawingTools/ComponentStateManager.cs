using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;


// This script is used to track the state of selectable objects. Used for deleting GameObjects and for filling drawn shapes.

public class ComponentStateManager : MonoBehaviour
{
    //public Transform selectState; // State of a drawing component on hover (before manipulation or selection). The object's "previous state".
    // public Transform hoverState; // State of a drawing component on hover (before manipulation or selection). The object's "previous state".
    //public Transform manipulationState;  // State of a drawing component after selection/manipulation. The object's "current state".

    public Vector3 selectPos;
    public Quaternion selectRot;
    public Vector3 selectScale;

    public Vector3 manipulationPos;
    public Quaternion manipulationRot;
    //public Vector3 manipulationScale;  // Removing until scale manipulation button is more functional

    // Expand later for object transformation history and more robust undo feature
    //public List<Vector3> positionsList;
    //public List<Quaternion> rotationsList;
    //public List<Vector3> scalesList;


    // This script attaches to a selectable component and obtains its transform information.
    void Start()
    {    

    }

    void Update()
    {
        
    }

    public void UpdateSelectState()  // previous transform when an object is initally clicked before manipulation
    {
        selectPos = gameObject.transform.position;
        selectRot = gameObject.transform.rotation;
        //selectScale = gameObject.transform.localScale;  // Remove until scale manipulation button is more functional

        // For object history
        //positionsList.Add(selectPos);
        //rotationsList.Add(selectRot);
        //scalesList.Add(selectScale);
    }

    public void UpdateManipulationState()  // current transform after a maniplation 
    {

        manipulationPos = gameObject.transform.position;
        manipulationRot = gameObject.transform.rotation;
        //manipulationScale = gameObject.transform.localScale;  // Removing until scale manipulation button is more functional

        // For object history
        //positionsList.Add(manipulationPos);
        //rotationsList.Add(manipulationRot);
        //scalesList.Add(manipulationScale);
    }
}
