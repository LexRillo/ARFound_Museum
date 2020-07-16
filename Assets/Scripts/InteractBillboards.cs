using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARSession))]
[RequireComponent(typeof(ARSessionOrigin))]
public class InteractBillboards : MonoBehaviour
{
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

    private void showBillboard(Touch touch)
    {
        Ray ray = m_camera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //GameObject touched_obj = hit.transform.gameObject;
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
            if (current_billboard != null)
            {
                current_billboard.SetActive(false);
            }
        }
    }

    // Code adapted from: https://wiki.unity3d.com/index.php/CameraFacingBillboard
    private void RotateTowardsCamera()
    {
        current_billboard.transform.LookAt(transform.position + m_camera.transform.rotation * Vector3.forward, m_camera.transform.rotation * Vector3.up);
    }
}
