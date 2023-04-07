using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

/// <summary>
/// Resizes the collision box of the drawing according to its dimensions 
/// LOCATION: Shape GameObject
/// </summary>
/// 
public class ShapeColliderResizer : MonoBehaviour
{
    BoxCollider drawingCollider;
    float startPointXPos, startPointYPos, startPointZPos, endPointXPos, endPointYPos, endPointZPos; // Universal xyz positions
    public List<float> xPositionsList = new List<float>(); 
    public List<float> yPositionsList = new List<float>();
    public List<float> zPositionsList = new List<float>();
    private GameObject freedrawStartPoint, freedrawEndPoint; // Freedraw variables
    private GameObject curveStartPoint, curvePoint2, curvePoint3, curveEndPoint; // Curves variables
    private Vector3 curveMidPoint;
    

    float curvePoint2XPos, curvePoint2YPos, curvePoint2ZPos, curvePoint3XPos, curvePoint3YPos, curvePoint3ZPos; // Curves xyz positions
    public GameObject gestureContainer;



    // Start is called before the first frame update
    void Start()
    {
        gestureContainer = GameObject.Find("GestureContainer");
        drawingCollider = GetComponent<BoxCollider>();

        if (gestureContainer.GetComponent<ModeManager>().freeDraw == true) // If application is in freedraw mode...
        {
            freedrawStartPoint = gameObject.transform.GetChild(4).transform.GetChild(0).transform.gameObject; // Start point of freedrawn line
            freedrawEndPoint = gameObject.transform.GetChild(2).transform.GetChild(0).transform.gameObject; // End point of freedrawn line
        }

        if (gestureContainer.GetComponent<ModeManager>().curve == true) // If application is in freedraw mode...
        {
            curveStartPoint = gameObject.transform.GetChild(2).transform.GetChild(3).transform.gameObject; // Start point of curve 
            curveEndPoint = gameObject.transform.GetChild(2).transform.GetChild(0).transform.gameObject; // End point of curve
            curvePoint2 = gameObject.transform.GetChild(2).transform.GetChild(2).transform.gameObject; // Point 2 (this is initial peak of curve)
            curvePoint3 = gameObject.transform.GetChild(2).transform.GetChild(1).transform.gameObject; // Point 3 (this is initial trough of curve)


            //Midpoint between teh two points
            //var midPoint = ((curveStartPoint.transform.position.x + curveEndPoint.transform.position.x) / 2, (curveStartPoint.transform.position.y + curveEndPoint.transform.position.y) / 2, (curveStartPoint.transform.position.z + curveEndPoint.transform.position.z) / 2);
            //print("Midpoint: " + midPoint);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // FREEDRAW
        if (gestureContainer.GetComponent<ModeManager>().freeDraw == true) // If application is in freedraw mode...
        {
            // Start point positions
            startPointXPos = freedrawStartPoint.transform.position.x;
            startPointYPos = freedrawStartPoint.transform.position.y;
            startPointZPos = freedrawStartPoint.transform.position.z;

            // End point positions
            endPointXPos = freedrawEndPoint.transform.position.x;
            endPointYPos = freedrawEndPoint.transform.position.y;
            endPointZPos = freedrawEndPoint.transform.position.z;

            // Find scale for x-axis
            xPositionsList.Add(freedrawEndPoint.transform.position.x); // Add all y positions to the list
            var highestXpos = xPositionsList.Max(); // Highest point of drawing
            var lowestXpos = xPositionsList.Min(); // Lowest point of drawing

            // Find scale for y-axis
            yPositionsList.Add(freedrawEndPoint.transform.position.y); // Add all y positions to the list
            var highestYpos = yPositionsList.Max(); // Highest point of drawing
            var lowestYpos = yPositionsList.Min(); // Lowest point of drawing

            // Find scale for z-axis
            zPositionsList.Add(freedrawEndPoint.transform.position.z); // Add all y positions to the list
            var highestZpos = zPositionsList.Max(); // Highest point of drawing
            var lowestZpos = zPositionsList.Min(); // Lowest point of drawing

            // Use distance between two points formula 
            Vector3 difference = new Vector3(
                highestXpos - lowestXpos,
                highestYpos - lowestYpos,
                highestZpos - lowestZpos);

            float distanceX = Mathf.Sqrt(Mathf.Pow(difference.x, 2));
            float distanceY = Mathf.Sqrt(Mathf.Pow(difference.y, 2));
            float distanceZ = Mathf.Sqrt(Mathf.Pow(difference.z, 2));

            // New box collider dimensions
            Vector3 boxColliderSize = new Vector3(distanceX, distanceY, distanceZ); // Scale of new box collider
            drawingCollider.size = boxColliderSize; // Resized box collider according to drawing size
            drawingCollider.center = gameObject.transform.GetChild(3).gameObject.GetComponent<Renderer>().bounds.center; // New center of box collider
        }
        
        // CURVE
        if (gestureContainer.GetComponent<ModeManager>().curve == true) // If application is in curve mode...
        {
            // Find scale for x-axis (using current position of startpoint, endpoint, and middle of curve)
            float[] xPositionsArray = new float[3];
            xPositionsArray[0] = curveStartPoint.transform.localPosition.x;
            //xPositionsArray[1] = curvePoint2.transform.localPosition.x;
            //xPositionsArray[2] = curvePoint3.transform.localPosition.x;
            xPositionsArray[1] = gameObject.transform.GetChild(2).GetComponent<LineRenderer>().GetPosition(6).x;// Array of line renderer is always 13 in length, so midpoint is index 6
            xPositionsArray[2] = curveEndPoint.transform.localPosition.x; // Point user is grabbing 
            //curveMidPoint = gameObject.transform.GetChild(2).GetComponent<LineRenderer>().GetPosition(6); 


            var highestXpos = Mathf.Max(xPositionsArray);
            var lowestXpos = Mathf.Min(xPositionsArray);


            // Find scale for y-axis
            float[] yPositionsArray = new float[4];
            yPositionsArray[0] = curveStartPoint.transform.localPosition.y;
            //yPositionsArray[1] = curvePoint2.transform.localPosition.y;
            //yPositionsArray[2] = curvePoint3.transform.localPosition.y;
            yPositionsArray[1] = gameObject.transform.GetChild(2).GetComponent<LineRenderer>().GetPosition(6).y;
            yPositionsArray[2] = curveEndPoint.transform.localPosition.y;

            var highestYpos = Mathf.Max(yPositionsArray);
            var lowestYpos = Mathf.Min(yPositionsArray);

            // Find scale for z-axis
            float[] zPositionsArray = new float[4];
            zPositionsArray[0] = curveStartPoint.transform.localPosition.z;
            //zPositionsArray[1] = curvePoint2.transform.localPosition.z;
            //zPositionsArray[2] = curvePoint3.transform.localPosition.z;
            zPositionsArray[1] = gameObject.transform.GetChild(2).GetComponent<LineRenderer>().GetPosition(6).z;
            zPositionsArray[3] = curveEndPoint.transform.localPosition.z;

            var highestZpos = Mathf.Max(zPositionsArray);
            var lowestZpos = Mathf.Min(zPositionsArray);

            // Use distance between two points formula 
            Vector3 difference = new Vector3(
                highestXpos - lowestXpos,
                highestYpos - lowestYpos,
                highestZpos - lowestZpos);

            float distanceX = Mathf.Sqrt(Mathf.Pow(difference.x, 2));
            float distanceY = Mathf.Sqrt(Mathf.Pow(difference.y, 2));
            float distanceZ = Mathf.Sqrt(Mathf.Pow(difference.z, 2));


            // New box collider dimensions
            Vector3 boxColliderSize = new Vector3(distanceX, distanceY, distanceZ); // Scale of new box collider
            drawingCollider.size = boxColliderSize; // Resized box collider according to drawing size
            drawingCollider.center = gameObject.transform.GetChild(2).gameObject.GetComponent<Renderer>().bounds.center; // New center of box collider


            print("highestX " + highestXpos);
            print("highestY " + highestYpos);
            print("highestZ " + highestZpos);

            print("lowestX " + lowestXpos);
            print("lowestY " + lowestYpos);
            print("lowestZ " + lowestZpos);



        }

        // LINE
        if (gestureContainer.GetComponent<ModeManager>().line == true) // If application is in line mode...
        {

        }

    }
}
