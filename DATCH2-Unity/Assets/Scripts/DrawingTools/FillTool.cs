using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class FillTool: MonoBehaviour
{
    // INFO CREATED FOR FILL MESH
    // Mesh will be created from vertices list (referred to as gameObject.GetComponent<MeshFilter>().mesh)
    public Vector3[] vertices;  // List of vertices that will affect the shape of the mesh
    public int[] triangles;  // Order of triangles to be drawn within mesh

    // INFO OBTAINED FROM SELECTED SHAPE
    public GameObject selectedShape; // shape prefab that holds brushPoints/grabPoints and drawing prefab
    private Vector3[] vertsInShape;  // the number of vertices
    public Vector3 centerPoint; // marks a vertex in middle of shape
    public GameObject gestureContainer;
    private List<int> newTriangles = new List<int>();  // List of triangles is not immutable. Data is passed into triangles array after the final list is created.
    private int triangleAmount = 0;

    void Start()
    {
        gestureContainer = GameObject.Find("GestureContainer");
        selectedShape = gameObject.transform.parent.gameObject;
        gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        

        vertsInShape = new Vector3[selectedShape.GetComponentInChildren<LineRenderer>().positionCount];

        for (int i = 0; i < selectedShape.GetComponentInChildren<LineRenderer>().positionCount; i++)
        {
            vertsInShape[i] = selectedShape.GetComponentInChildren<LineRenderer>().GetPosition(i);
        }

        CreateShape();
        UpdateMesh();
    }

    void Update()
    {
        UpdateMesh();
        
        // Keeps fillMesh centered within lineRenderer
        gameObject.transform.position = new Vector3(0, 0, 0.005f);  // Offsetting on Z axis slightly to reduce z-fighting between fillMesh and lineRenderer
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.localScale = AdditionalUtilityFunctions.SetGlobalScale(gameObject, gameObject.transform, new Vector3(1.0f, 1.0f, 1.0f));
    }

    public void CreateNewMesh()
    {
        gestureContainer = GameObject.Find("GestureContainer");
        selectedShape = gameObject.transform.parent.gameObject;


        GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().mesh = gameObject.GetComponent<Mesh>();
        GetComponent<MeshFilter>().mesh = new Mesh();


        vertsInShape = new Vector3[selectedShape.GetComponentInChildren<LineRenderer>().positionCount];

        for (int i = 0; i < selectedShape.GetComponentInChildren<LineRenderer>().positionCount; i++)
        {
            vertsInShape[i] = selectedShape.GetComponentInChildren<LineRenderer>().GetPosition(i);
        }

        CreateShape();

    }

    // Note: Num triangles must match num vertices. Num triangles must be a multiple of 3.
    public void CreateShape()
    {
        vertices = vertsInShape;

        vertices[1] = CalcCenterPoint(vertices); // Must draw point at index 1 at the center of the shape

        triangleAmount = vertsInShape.Length - 2;  // This is the number of triangles to draw. Making it slightly less than actual number of triangles needed for shape to function with for loop

        // Triangles are the order in which to draw the points that make up each triangle in a shape. Triangles are drawn clockwise in Unity.
        for (int i = 0; i< triangleAmount; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }

        triangles = newTriangles.ToArray();

        gameObject.GetComponent<MeshFilter>().mesh.vertices = vertsInShape;
        gameObject.GetComponent<MeshFilter>().mesh.triangles = triangles;
    }

    public void UpdateMesh()
    {
        gameObject.GetComponent<MeshFilter>().mesh.Clear();

        vertsInShape = new Vector3[selectedShape.GetComponentInChildren<LineRenderer>().positionCount];

        for (int i = 0; i < selectedShape.GetComponentInChildren<LineRenderer>().positionCount; i++)
        {
            vertsInShape[i] = selectedShape.GetComponentInChildren<LineRenderer>().GetPosition(i);
        }

        gameObject.GetComponent<MeshFilter>().mesh.vertices = vertsInShape;
        gameObject.GetComponent<MeshFilter>().mesh.triangles = triangles;
    }

    public Vector3 CalcCenterPoint(Vector3[] pointsList)
    {
        for(int i = 0; i < pointsList.Length; i++)
        {
            centerPoint += pointsList[i]/pointsList.Length;
        }

        return centerPoint;
    }

}
