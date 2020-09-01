using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script for loading other scenes
public class SceneSwitch : MonoBehaviour
{
    public string SceneToSwitchTo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScene()
    {
        Debug.Log("I AM LOADING A NEW SCENE");
        SceneManager.LoadSceneAsync(SceneToSwitchTo, LoadSceneMode.Single);
    }
}
