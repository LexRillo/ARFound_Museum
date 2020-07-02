using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARSession))]
    [RequireComponent(typeof(ARFaceManager))]
    [RequireComponent(typeof(ARSessionOrigin))]
    public class lookthrough_Debug : MonoBehaviour
    {
        [SerializeField]
        Text m_FaceInfoText;

        public Text faceInfoText
        {
            get => m_FaceInfoText;
            set => m_FaceInfoText = value;
        }

        ARSession m_Session;

        ARCameraManager m_CameraManager;

        StringBuilder m_Info = new StringBuilder();


        //
        [SerializeField] private string selectableTag = "Selectable";
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material defaultMaterial;
        Transform _selection;
        public string SceneToSwitchTo;
        //

        void Awake()
        {
            m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
        }

        void Update()
        {
            m_Info.Clear();

            if (_selection != null)
            {
                if (_selection.childCount == 0)
                {
                    var selectionRenderer = _selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = defaultMaterial;
                    }
                }
                else
                {
                    for (int i = 0; i < _selection.childCount; i++)
                    {
                        var s = _selection.GetChild(i);
                        var selectionRenderer = s.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = defaultMaterial;
                        }
                    }
                }
                
                _selection = null;
            }
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            //Ray ray = m_camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            m_Info.Append($"The ray has position {ray.origin}\n");
            m_Info.Append($"The ray has direction {ray.direction}\n");
            m_Info.Append($"The ray has direction {m_camera.transform.rotation.eulerAngles}\n");
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                m_Info.Append($"The Raycast finds something and has this transform: {selection.ToString()}\n");
                if (selection.CompareTag(selectableTag))
                {
                    m_Info.Append($"does selectable have children? {selection.childCount}\n");
                    if (selection.childCount == 0) {
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = highlightMaterial;
                        }
                    }
                    else
                    {
                        for(int i = 0; i < selection.childCount; i++)
                        {
                            var s = selection.GetChild(i);
                            var selectionRenderer = s.GetComponent<Renderer>();
                            if (selectionRenderer != null)
                            {
                                selectionRenderer.material = highlightMaterial;
                            }
                        }
                    }

                    if (selection.name == "Upper_lens_holder_microscope" && 
                        m_camera.transform.rotation.eulerAngles.x > 80 && m_camera.transform.rotation.eulerAngles.x < 90)
                    {
                        m_Info.Append($"You are on top of it\n");
                    }

                    _selection = selection;
                }
            }

            if (m_CameraManager)
            {
                m_Info.Append($"Requested camera facing direction: {m_CameraManager.requestedFacingDirection}\n");
                m_Info.Append($"Current camera facing direction: {m_CameraManager.currentFacingDirection}\n");
            }

            if (m_FaceInfoText)
            {
                m_FaceInfoText.text = m_Info.ToString();
            }
        }
        public void switchscene()
        {
            SceneManager.LoadSceneAsync(SceneToSwitchTo, LoadSceneMode.Single);
        }
    }
}