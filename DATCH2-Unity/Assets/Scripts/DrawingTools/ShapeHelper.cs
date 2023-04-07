using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to resize the bounding box of a shape based on the size of the linerenderer
public class ShapeHelper : MonoBehaviour
{
    public GameObject currentShape;
    private GameObject lineDrawing;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {      

    }

    public void DisableFillMeshOnManipulated()
    {
        if (currentShape.transform.GetChild(0).tag == "Fill")
        {
            currentShape.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false; // disable fill mesh if shape has been manipulated
        }
    }
}
