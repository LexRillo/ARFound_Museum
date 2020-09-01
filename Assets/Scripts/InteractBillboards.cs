using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//  Script that describes the behaviour of the billboards from the visualization scene
[RequireComponent(typeof(ARSession))]
[RequireComponent(typeof(ARSessionOrigin))]
public class InteractBillboards : MonoBehaviour
{
    // get the camera position and direction
    ARCameraManager m_CameraManager;
    Camera m_camera;
    GameObject current_billboard = null;
    // Start is called before the first frame update
    void Awake()
    {
        m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
        m_camera = m_CameraManager.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the user taps the section of the microscope it will show the corresponding billboard
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            showBillboard(touch);
        }

        if (current_billboard != null)
        {
            RotateTowardsCamera();
        }
    }

    // Display the billboard and enable the audio interaction on the billboard
    private void showBillboard(Touch touch)
    {
        Ray ray = m_camera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        //Raycast from the camera to the scene
        if (Physics.Raycast(ray, out hit))
        {
            // if ray hits the audio tag, then play it
            if (hit.transform.tag == "Billboard_Sound")
            {
                hit.transform.GetComponent<AudioSource>().Play();
            }

            // iterate through the children to check if the collided object contains a billboard
            for (int i = 0; i < hit.transform.childCount; i++)
            {
                if (hit.transform.GetChild(i).tag == "Billboard")
                {
                    if (current_billboard != null)
                    {
                        current_billboard.SetActive(false);
                    }
                    current_billboard = hit.transform.GetChild(i).gameObject;
                    current_billboard.SetActive(true);
                }
            }
        }
        else
        {
            // If there was no hit and there was a billboard being shown, then make billboard invisible
            if (current_billboard != null)
            {
                current_billboard.SetActive(false);
            }
        }
    }

    // This is used to make sure that the billboard is readable from the camera position and angle
    // Code adapted from: https://wiki.unity3d.com/index.php/CameraFacingBillboard
    private void RotateTowardsCamera()
    {
        current_billboard.transform.LookAt(transform.position + m_camera.transform.rotation * Vector3.forward, m_camera.transform.rotation * Vector3.up);
    }
}
