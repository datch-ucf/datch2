using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateShapeButton : MonoBehaviour
{
    //public GameObject currentShape; 
    
    //public GameObject shapeDuplicates;
    public void Duplicate()
    {
        //shapeDuplicates.transform.position = Camera.main.transform.position + new Vector3(0, 0, 1); // Move shapeDuplicates (parent that holds duplicate shapes) in front of player 

        Instantiate(Camera.main.transform.GetChild(0).GetComponent<SelectionManager>().selectedShape.gameObject, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 1) + Camera.main.transform.forward * 1.25f, Quaternion.identity); // "Duplicate" the current selected shape...ISSUE: THIS CAUSES PROJECT TO FREEZE...
    }
}
