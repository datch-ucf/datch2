using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleTooltipIconManager : MonoBehaviour
{
    public Sprite moveSprite;
    public Sprite rotateSprite;
    public Sprite scaleSprite;
    public GameObject rigRoot; // Parent of all shape handles
    private bool gotShapeHandles;
    public GameObject[] shapeHandlesArr = new GameObject[26];
    public int[] shapeHandlesIndices = new int[26];
    private int indexCount = 0;
    private bool finishedAppearDisappearCoroutine = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.transform.parent.GetChild(gameObject.transform.parent.childCount-1).gameObject.name == "rigRoot")
        {
            rigRoot = gameObject.transform.parent.GetChild(gameObject.transform.parent.childCount - 1).gameObject;
        }

        if(rigRoot != null && gotShapeHandles == false)
        {

            for (int i = 0; i < rigRoot.transform.childCount; i++)
            {
                if (rigRoot.transform.GetChild(i).name.Contains("corner") || rigRoot.transform.GetChild(i).name.Contains("midpoint") || rigRoot.transform.GetChild(i).name.Contains("face"))
                {
                    shapeHandlesIndices[indexCount] = i;
                    indexCount += 1;
                }
            }

            RefreshHandles();

            gotShapeHandles = true;
        }
        
        // Array tends to erase handles from list when bounds is disabled (and rigroot does not exist). Looks for handles again if rigroot is active again.
        if(rigRoot != null && (shapeHandlesArr[0] == null || shapeHandlesArr[shapeHandlesArr.Length-1] == null))
        {
            RefreshHandles();
        }

        if (shapeHandlesArr.Length > 0 && gameObject.transform.parent.GetComponent<HoverSelectionManager>().isHovering == true)
        {

            gameObject.transform.LookAt(Camera.main.transform);

            foreach (GameObject handle in shapeHandlesArr)
            {
                if (CoreServices.FocusProvider.PrimaryPointer.IsActive == true && handle != null)
                {
                    if (CoreServices.FocusProvider.PrimaryPointer.Result.CurrentPointerTarget.name == handle.name)
                    {
                        if (handle.name.Contains("face"))
                        {
                            //print("MOVE HANDLE");
                            gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = moveSprite; // change image to move image
                            gameObject.transform.position = handle.transform.position; // move position of canvas to near the handle
                            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z - 0.075f); // Offset for better viewability

                            if(finishedAppearDisappearCoroutine == true)
                            {
                                StartCoroutine(AppearThenDisappear());
                            }
                            
                        }

                        if (handle.name.Contains("midpoint"))
                        {
                            //print("ROT HANDLE");
                            gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = rotateSprite; // change image to rotate image
                            gameObject.transform.position = handle.transform.position; // move position of canvas to near the handle
                            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z - 0.075f); // Offset for better viewability

                            if (finishedAppearDisappearCoroutine == true)
                            {
                                StartCoroutine(AppearThenDisappear());
                            }

                        }

                        if (handle.name.Contains("corner"))
                        {
                            gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = scaleSprite; // change image to scale image
                            gameObject.transform.position = handle.transform.position; // move position of canvas to near the handle
                            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z - 0.075f); // Offset for better viewability

                            if (finishedAppearDisappearCoroutine == true)
                            {
                                StartCoroutine(AppearThenDisappear());
                            }

                        }
                    }
                }
            }
        }

        if (gameObject.transform.parent.GetComponent<HoverSelectionManager>().isHovering == false)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Image>().enabled = false;
        }

    }

    // Locates handles if rigRoot is deactivated at any point in time
    public void RefreshHandles()
    {
        for (int i = 0; i < shapeHandlesArr.Length; i++)
        {
            shapeHandlesArr[i] = rigRoot.transform.GetChild(shapeHandlesIndices[i]).gameObject;
        }
    }


    // Disables visual after a set amount of time
    IEnumerator AppearThenDisappear()
    {
        finishedAppearDisappearCoroutine = false;
        gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        gameObject.transform.GetChild(1).GetComponent<Image>().enabled = false;
        finishedAppearDisappearCoroutine = true;
    }
}
