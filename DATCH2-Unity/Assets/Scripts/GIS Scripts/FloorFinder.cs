using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFinder : MonoBehaviour
{
    public GameObject rayOrigin1;
    public GameObject rayOrigin2;
    public GameObject rayOrigin3;
    public GameObject rayOrigin4;

    float ray1HitPos;
    float ray2HitPos;
    float ray3HitPos;
    float ray4HitPos;
    float calculatedRayHitPos;

    float[] hitPositionsArr;

    public float floorLvl;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray1HitPos = CastRayFromObj(rayOrigin1);
        ray2HitPos = CastRayFromObj(rayOrigin2);
        ray3HitPos = CastRayFromObj(rayOrigin3);
        ray4HitPos = CastRayFromObj(rayOrigin4);

        

        hitPositionsArr = new float[] {ray1HitPos, ray2HitPos, ray3HitPos, ray4HitPos};

        floorLvl = Mathf.Min(hitPositionsArr);


    }

    // Shoot raycasts from empty gameObjects around user to find floor level
    public float CastRayFromObj(GameObject rayOrigin)
    {
        Ray ray = new Ray(rayOrigin.transform.position, Vector3.down);

        RaycastHit hitData;

        Physics.Raycast(ray, out hitData); // Shoot out the ray

        Debug.DrawRay(ray.origin, ray.direction * 10);

        calculatedRayHitPos = hitData.point.y; // Elevation level (y pos) this ray hits the ground

        if(hitData.collider != null)
        {
            if (hitData.collider.tag == "Menu")
            {
                calculatedRayHitPos = 10; // Large outlier to prevent menu from being considered an option for floor level
            }

        }

        return calculatedRayHitPos;

    }
}
