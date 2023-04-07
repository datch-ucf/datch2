using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingPrefabOrigin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = gameObject.transform.GetComponent<Renderer>().bounds.center;
    }
}
