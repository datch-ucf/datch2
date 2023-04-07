using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate different tools on button press (e.g. measuringCube, measuringTape)
/// LOCATION: ToolsGenerator empty gameObject 
/// </summary>

public class ToolsGenerator : MonoBehaviour
{

    // Measuring Tools Variables
    public GameObject measuringCube;
    public GameObject measuringTape;
    public GameObject generatedMeasuringCube;
    public GameObject generatedMeasuringTape;
    public List<GameObject> generatedMeasuringCubes;
    public List<GameObject> generatedMeasuringTapes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Measuring Tools
    public void GenerateMeasuringCube()
    {
        generatedMeasuringCube = Instantiate(measuringCube, new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z) + Camera.main.transform.forward * 1.25f, Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0)); // Add to the forward to place measuring cube in front of user
        generatedMeasuringCubes.Add(generatedMeasuringCube);
    }

    public void GenerateMeasuringTape()
    {
        generatedMeasuringTape = Instantiate(measuringTape, new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z) + Camera.main.transform.forward * 0.5f, Quaternion.Euler(0, Camera.main.transform.localEulerAngles.y, 0));
        generatedMeasuringTapes.Add(generatedMeasuringTape);
    }

}
