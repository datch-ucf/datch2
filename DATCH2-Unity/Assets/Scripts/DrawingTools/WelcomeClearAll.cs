using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeClearAll : MonoBehaviour
{
    public static int timesReloaded;
    public GameObject welcomeText;
    

    // Start is called before the first frame update
    void Start()
    {
        // Prevent welcome screen from showing when user selects ClearAll button
        if (timesReloaded > 0)
        {
            Destroy(welcomeText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
