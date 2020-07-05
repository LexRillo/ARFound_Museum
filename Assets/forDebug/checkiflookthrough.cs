//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using System.Security.Cryptography;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Text;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;

////if it triggers collider and the rotation of the screen is 1.5 on the x then change scene
//public class checkiflookthrough : MonoBehaviour
//{
//    [SerializeField] private string selectableTag = "Selectable";
//    [SerializeField] private Material highlightMaterial;
//    [SerializeField] private Material defaultMaterial;

//    ARSessionOrigin _m_origin;
//    [SerializeField] Text InfoText;
//    StringBuilder m_Info = new StringBuilder();

//    Camera m_camera;

//    void Start()
//    {
//        m_camera = _m_origin.camera?.GetComponent<ARCameraManager>().GetComponent<Camera>();
//        InfoText.text = "Is this even running?";
//    }
//    private Transform _selection;

//    // Update is called once per frame
//    void Update()
//    {
//        m_Info.Clear();
//        if (_selection != null)
//        {
//            m_Info.Append($"Found a selection\n");
//            var selectionRenderer = _selection.GetComponent<Renderer>();
//            selectionRenderer.material = defaultMaterial;
//            _selection = null;
//        }
//        else
//        {
//            m_Info.Append($"No selection found yet\n");
//        }
//        // Casting a ray from the center of the screen
//        //Debug.Log(new Vector3(0.5f, 0.5f, 0f));
//        //Debug.Log(new Vector3(Screen.width / 2, Screen.height / 2, 0));Input.mousePosition
//        //Debug.Log(Input.mousePosition);
//        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
//        m_Info.Append($"The ray has position {ray.origin}\n");
//        m_Info.Append($"The ray has direction {ray.direction}\n");
//        //Ray ray = m_camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

//        RaycastHit hit;
//        //Debug.DrawRay(new Vector3(Screen.width / 2, Screen.height / 2, 0), ray.direction, Color.red);
//        if (Physics.Raycast(ray, out hit))
//        {
//            Debug.Log("There was a hit");
//            m_Info.Append($"There was a hit\n");
//            var selection = hit.transform;
//            if (selection.CompareTag(selectableTag))
//            {
//                var selectionRenderer = selection.GetComponent<Renderer>();
//                if (selectionRenderer != null)
//                {
//                    selectionRenderer.material = highlightMaterial;
//                }
//                _selection = selection;
//            }
//        }
//        InfoText.text = m_Info.ToString();
//    }

//    public Transform getSelection()
//    {
//        return _selection;
//    }
//}
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARSession))]
[RequireComponent(typeof(ARFaceManager))]
[RequireComponent(typeof(ARSessionOrigin))]
public class checkiflookthrough : MonoBehaviour
{
    [SerializeField]
    Text m_FaceInfoText;

    public Text faceInfoText
    {
        get => m_FaceInfoText;
        set => m_FaceInfoText = value;
    }

    ARSession m_Session;

    ARFaceManager m_FaceManager;

    ARCameraManager m_CameraManager;

    StringBuilder m_Info = new StringBuilder();

    void Awake()
    {
        m_FaceManager = GetComponent<ARFaceManager>();
        m_Session = GetComponent<ARSession>();
        m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
    }

    void OnEnable()
    {
        // Detect face tracking with world-facing camera support
    }

    void Update()
    {
        m_Info.Clear();

        if (m_FaceManager.subsystem != null)
        {
            m_Info.Append($"Supported number of tracked faces: {m_FaceManager.supportedFaceCount}\n");
            m_Info.Append($"Max number of faces to track: {m_FaceManager.currentMaximumFaceCount}\n");
            m_Info.Append($"Number of tracked faces: {m_FaceManager.trackables.count}\n");
            string my_text = "Nothing";
            // tooclose when z is 0.2
            if (m_FaceManager.trackables.count > 0)
            {
                foreach (var face in m_FaceManager.trackables)
                {
                    //if (face.transform.position.z > 0.2)
                    //{
                    //    my_text = "Far";
                    //}
                    //else
                    //{
                    //    my_text = "Close";
                    //}
                    my_text = face.transform.position.ToString();
                }
            }
            m_Info.Append($"Distance from face: {my_text}\n");
            var camera = m_CameraManager.GetComponent<Camera>();
            m_Info.Append($"Camera position: {camera.transform.position.ToString()}\n");
            m_Info.Append($"Camera rotation: {camera.transform.rotation.ToEulerAngles().ToString()}\n");
        }

        if (m_CameraManager)
        {
            m_Info.Append($"Requested camera facing direction: {m_CameraManager.requestedFacingDirection}\n");
            m_Info.Append($"Current camera facing direction: {m_CameraManager.currentFacingDirection}\n");
        }

        m_Info.Append($"Requested tracking mode: {m_Session.requestedTrackingMode}\n");
        m_Info.Append($"Current tracking mode: {m_Session.currentTrackingMode}\n");
    }
}
