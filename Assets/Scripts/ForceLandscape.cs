using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that forces the the engine to show in the content Landscape mode and Requests access to the camera (used for testing on iOS devices)
public class ForceLandscape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("Camera found and authorized");
        }
        else
        {
            Debug.Log("Camera not found or not authorized");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
