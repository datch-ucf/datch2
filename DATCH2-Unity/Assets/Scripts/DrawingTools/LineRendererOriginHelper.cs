using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets default width and color values for lineRendererOrigin Prefab
/// LOCATION: LineRendererOrigin Prefab
/// </summary>
public class LineRendererOriginHelper : MonoBehaviour
{
    // Need to set these values on creation of lineRendererOrigin to keep defaults correct after every restart of app
    //void Start()
    //{
    //    print("DEFAULTS AT START");
    //    //gameObject.GetComponent<LineRenderer>().startWidth = 0.005f;
    //    //gameObject.GetComponent<LineRenderer>().endWidth = 0.005f;
    //    gameObject.GetComponent<LineRenderer>().startColor = Color.white;
    //    gameObject.GetComponent<LineRenderer>().endColor = Color.white;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnApplicationStart()
    //{
    //    print("SETTING DEFAULTS");
    //    gameObject.GetComponent<LineRenderer>().startWidth = 0.005f;
    //    gameObject.GetComponent<LineRenderer>().endWidth = 0.005f;
    //    gameObject.GetComponent<LineRenderer>().startColor = Color.white;
    //    gameObject.GetComponent<LineRenderer>().endColor = Color.white;
    //}
}
