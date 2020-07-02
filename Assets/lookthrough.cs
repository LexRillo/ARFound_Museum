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
    public class lookthrough : MonoBehaviour
    {
        ARCameraManager m_CameraManager;

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
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {
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
                        switchscene(0);
                    }

                    if (selection.name == "Upper_lens_holder_microscope1" &&
                        m_camera.transform.rotation.eulerAngles.x > 80 && m_camera.transform.rotation.eulerAngles.x < 90)
                    {
                        switchscene(1);
                    }

                    _selection = selection;
                }
            }
        }

        public void switchscene(int index)
        {
            StaticContainer.lensIndex = index;
            SceneManager.LoadSceneAsync(SceneToSwitchTo, LoadSceneMode.Single);
        }

        public Transform getSelection()
        {
            return _selection;
        }
    }
}