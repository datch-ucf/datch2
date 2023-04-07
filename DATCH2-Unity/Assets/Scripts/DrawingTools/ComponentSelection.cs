using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

// This script is used to track the user's selections. Will be used for selecting components to delete and for filling drawn shapes.

public class ComponentSelection : MonoBehaviour
{
    public GameObject selectedComponent;
    //public GameObject previouslySelectedComponent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectComponent(MixedRealityPointerEventData eventData)
    {
        selectedComponent = eventData.Pointer.Result.PreviousPointerTarget;
        print("Selected Component: " + selectedComponent.name);
    }
}
