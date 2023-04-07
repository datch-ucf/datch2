using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapePresetsGenerator : MonoBehaviour
{
    public LineRenderer LineRendererOriginPrefab;
    public LineRenderer outline;
    public GameObject[] pointGameObjects;
    public Transform[] points;
    public GameObject grabPointPrefab;
    private GameObject point1;
    private GameObject point2;
    private GameObject point3;
    private GameObject point4;
    public bool created;
   

    // Start is called before the first frame update
    void Start()
    {

        GenerateSquare();
    }

    // Update is called once per frame
    void Update()
    {
        //if(created)
        //{
            SetLineToShapePoints();
        //}

        for (int i = 0; i < points.Length; i++)
        {
            outline.SetPosition(i, points[i].position);
        }
       
    }

    public void SetUpLine(Transform[] points)
    {
        outline.positionCount = points.Length;
    }

    public void GenerateSquare()
    {
        outline = Instantiate(LineRendererOriginPrefab);
        outline.transform.parent = gameObject.transform;
        outline.useWorldSpace = true; // Allows the line to move when grabbed and manipulated
        outline.transform.position = new Vector3(0, 0, 0);
        outline.loop = true; // close the shape

        points = new Transform[4];

        point1 = Instantiate(grabPointPrefab);
        point1.transform.position = new Vector3(0, 0.1f, 0);
        point1.transform.parent = gameObject.transform;

        point2 = Instantiate(grabPointPrefab);
        point2.transform.position = new Vector3(0.1f, 0.1f, 0);
        point2.transform.parent = gameObject.transform;


        point3 = Instantiate(grabPointPrefab);
        point3.transform.position = new Vector3(0.1f, 0, 0);
        point3.transform.parent = gameObject.transform;


        point4 = Instantiate(grabPointPrefab);
        point4.transform.position = new Vector3(0, 0, 0);
        point4.transform.parent = gameObject.transform;

        pointGameObjects = new GameObject[]{point1, point2, point3, point4};
        points = new Transform[]{point1.transform, point2.transform, point3.transform, point4.transform};

        SetUpLine(points);
        created = true;
    }

    public void SetLineToShapePoints()
    {
        points = new Transform[] { point1.transform, point2.transform, point3.transform, point4.transform };
        SetUpLine(points);
    }
}
