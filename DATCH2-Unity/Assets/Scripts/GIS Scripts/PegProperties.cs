using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes properties (e.g. color) of individual marker peg in the MarkerPegsGrid. 
/// LOCATION: Peg prefab
/// </summary>

public class PegProperties : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePegColor()
    {
        // If the marked peg is showing -- red
        if(gameObject.GetComponent<MeshRenderer>().enabled == true)
        {
            // Hide marked peg (show unmarked peg -- yellow)
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            // Show marked peg -- red
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        
    }
}
