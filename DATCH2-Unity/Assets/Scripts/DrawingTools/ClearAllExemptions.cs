using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prevents specific gameobjects from reactivating when ClearAllButton is pressed
/// LOCATION: Timeline GameObject
/// </summary>
public class ClearAllExemptions : MonoBehaviour
{
    public static int timesReloaded;
    private GameObject welcomeText;
    private GameObject startingInstructions;

    // Start is called before the first frame update
    void Start()
    {
        welcomeText = Camera.main.transform.GetChild(0).gameObject;
        startingInstructions = Camera.main.transform.GetChild(1).gameObject;
        // Prevent welcome screen and instructinos from showing when user selects ClearAll button
        if (timesReloaded > 0)
        {
            Destroy(welcomeText);
            Destroy(startingInstructions);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
