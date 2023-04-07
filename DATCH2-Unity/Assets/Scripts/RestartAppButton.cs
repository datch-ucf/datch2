using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartAppButton : MonoBehaviour
{
    public static RestartAppButton Instance; // Creates an instance of this script (only one instance of this script can be open at a time)
    public GameObject gestureContainer;
    public GameObject pen;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        //if (instance != null && instance != this)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    instance = this;
        //}

    }

    void Start()
    {
        //// If there's already an instance of this script, destroy that instance 
        //if(Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //// If not, make this the instance and don't destroy it
        //Instance = this; 
        //GameObject.DontDestroyOnLoad(this.gameObject);

    }

    public void RestartScene()
    {
        //pen = Camera.main.transform.GetChild(1).gameObject;
        //gestureContainer = Camera.main.transform.GetChild(0).gameObject;

        ClearAllExemptions.timesReloaded++;
        //DontDestroyOnLoad(pen);
        //DontDestroyOnLoad(gestureContainer);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        // ISSUE: gestureContainer and pen are deleting on restart
        //Instantiate(gestureContainer, Camera.main.transform);
        //gestureContainer.transform.SetSiblingIndex(0);
        //Instantiate(pen, Camera.main.transform);
        //pen.transform.SetSiblingIndex(1);


    }
}
