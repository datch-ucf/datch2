using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds a shapeHolder parent for each created shape
public class ShapeHolderGenerator : MonoBehaviour
{
    public GameObject shapeHolderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newShapeHolder = Instantiate(shapeHolderPrefab);
        newShapeHolder.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
        gameObject.transform.SetParent(newShapeHolder.transform);
    }
}
