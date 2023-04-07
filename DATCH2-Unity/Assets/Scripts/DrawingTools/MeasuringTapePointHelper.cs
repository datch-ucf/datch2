using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringTapePointHelper : MonoBehaviour
{
    /// <summary>
    /// Allows startPoint and endPoint of measuring tape to snap to any object (with a collider) in scene (i.e. shapes, points, other measuring tapes).
    /// Location: Measuring tape startPoint and endPoint
    /// </summary>

    public GameObject nearestLinkedObject;
    public bool linkedToObject = false;
    private GameObject gestureContainer;

    private void Start()
    {
        gestureContainer = Camera.main.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        // Point linking -- keeps the current point attached to the nearest linked point (detach with a swift jerk to break connection)
        if(linkedToObject == true)
        {
            gameObject.transform.position = nearestLinkedObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enables point linking
        //if (other.gameObject.layer == 31 || other.CompareTag("Shape") || other.CompareTag("FreeDrawPoint") || other.CompareTag("GrabPoint"))

        if ((other.gameObject.layer == 31 || other.CompareTag("Shape") || other.CompareTag("FreeDrawPoint") || other.CompareTag("GrabPoint") || other.CompareTag("MeasuringTape")) && other.transform.parent != gameObject.transform.parent && gameObject == gestureContainer.GetComponent<SelectionManager>().selectedComponent) // Last condition only enables linking of currently selected point (deliberately placed)
        {
            if(other.CompareTag("MeasuringTape"))
            {
                print("Hit measuring tape");
                print("Parent: " + other.transform.parent.gameObject);
                if(other.transform.parent == null)
                {
                    print("parent == null");
                }
                if(other.transform.parent.gameObject == null)
                {
                    print("parentgo==null");
                }
            }
            print("COLLIDED WITH: " + other.gameObject);
            nearestLinkedObject = other.gameObject;
            gameObject.transform.position = nearestLinkedObject.transform.position;
            linkedToObject = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Disables point linking
        if (other == nearestLinkedObject)
        {
            print("Disable linking: " + other);
            nearestLinkedObject = null;
            linkedToObject = false;
        }
    }
}

