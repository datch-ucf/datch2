using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleHelper : MonoBehaviour
{
    /// <summary>
    /// Labels handles with correct tag
    /// LOCATION: All handle prefabs (cube, etc.)
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent != null && gameObject.transform.parent.CompareTag("Handle") == false)
        {
            gameObject.transform.parent.tag = "Handle";
        }
    }
}
