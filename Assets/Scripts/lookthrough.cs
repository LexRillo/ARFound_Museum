// Original code: https://github.com/Unity-Technologies/arfoundation-samples
// Modified for alternative use of ARFoundation

using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Collections;

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
        public GameObject TransitionObject;
        private Transform handle_selected = null;
        //

        void Awake()
        {
            m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
            StaticContainer.lensIndex = 1;
            StaticContainer.slideIndex = 0;
        }

        void Update()
        {
            if (_selection != null)
            {
                _selection.GetComponent<ChangeToScriptedMat>().revertToOriginalMat();                               
                _selection = null;
            }
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;

                //if (selection.CompareTag(selectableTag))
                if (selection.GetComponent<ChangeToScriptedMat>() != null)
                {
                    selection.GetComponent<ChangeToScriptedMat>().changeMat(highlightMaterial);
                    if (selection.name == "Upper_lens_holder_microscope" && 
                        m_camera.transform.rotation.eulerAngles.x > 80 && m_camera.transform.rotation.eulerAngles.x < 90 && StaticContainer.lensIndex == 1)
                    {
                        StartCoroutine(switchscene());
                    }

                    if (selection.name == "Upper_lens_holder_microscope1" &&
                        m_camera.transform.rotation.eulerAngles.x > 80 && m_camera.transform.rotation.eulerAngles.x < 90 && StaticContainer.lensIndex == 2)
                    {
                        StartCoroutine(switchscene());
                    }

                    _selection = selection;
                }
            }

            //if (Input.touchCount == 1 && handle_selected == null)
            //{
            //    Touch touch = Input.GetTouch(0);
            //    MoveMirrorHolder_Selection(touch);
            //}else 
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                // check if it's close enough to the dial. otherwise deselect
                if (touch.phase == TouchPhase.Moved)
                {
                    MoveMirrorHolder_Rotation(touch);
                }
            }
        }

        IEnumerator switchscene()
        {
            //StaticContainer.lensIndex = index;
            TransitionObject.SetActive(true);
            Animator transitionSceneAnim = TransitionObject.GetComponent<Animator>();
            transitionSceneAnim.Play("Microscope_fade_in");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync(SceneToSwitchTo, LoadSceneMode.Single);
        }

        public Ray myRayCast()
        {
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            return ray;
        }

        public Transform getSelection()
        {
            return _selection;
        }

        public void resetPositions()
        {
            GameObject[] objs;
            GameObject[] refobjs;
            objs = GameObject.FindGameObjectsWithTag("ARContainer");
            refobjs = GameObject.FindGameObjectsWithTag("ARrefpositions");
            if (objs.Length > 0 && refobjs.Length > 0)
            {
                for (int k = 0; k < objs.Length; k++)
                {

                    //objs[i].transform.position = refobjs[i].transform.position;
                    for (int i = 0; i < objs[k].transform.childCount; i++)
                    {
                        GameObject this_child = objs[k].transform.GetChild(i).gameObject;
                        GameObject this_ref_child = refobjs[k].transform.GetChild(i).gameObject;
                        //Debug.Log("this_child "+ this_child.name + " corresponding " + refobjs[k].transform.GetChild(i).name);
                        //Debug.Log("this_transf " + this_child.transform.position + " corresponding " + refobjs[k].transform.GetChild(i).transform.position);
                        if (this_ref_child.CompareTag("refobject"))
                        {
                            this_child.transform.position = refobjs[k].transform.GetChild(i).gameObject.transform.position;
                        }
                    }

                }
            }
            StaticContainer.lensIndex = 1;
            StaticContainer.slideIndex = 0;
        }
        
        public void MoveMirrorHolder_Selection(Touch touch)
        {
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Ray ray = m_camera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.name == "microscope_mirror_holder")
            {
                //Debug.Log("This is where I tocuh " + hit.point + " object transform " + hit.transform.position);
                handle_selected = hit.transform;
                for (int i = 0; i < hit.transform.childCount; i++)
                {
                    if (hit.transform.GetChild(i).name == "HOLDER_DIAL")
                    {
                        //Vector3 center = hit.transform.GetChild(i).GetComponent<Renderer>().bounds.center;
                        
                    }
                }
            }
        }

        public void MoveMirrorHolder_Rotation(Touch touch)
        {
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Vector2 first_point = touch.position - touch.deltaPosition;
            Ray first_ray = m_camera.ScreenPointToRay(first_point);
            RaycastHit first_hit;
            if (Physics.Raycast(first_ray, out first_hit) && first_hit.transform.name == "microscope_mirror_holder")
            {
                Ray second_ray = m_camera.ScreenPointToRay(first_point);
                RaycastHit second_hit;
                if (Physics.Raycast(second_ray, out second_hit) && second_hit.transform.name == "microscope_mirror_holder")
                {
                    Vector3 center = first_hit.transform.FindChild("HOLDER_DIAL").GetComponent<Renderer>().bounds.center;

                    Vector2 first_vector = new Vector2(first_hit.point.y - center.y, first_hit.point.z - center.z);
                    Vector2 second_vector = new Vector2(second_hit.point.y - center.y, second_hit.point.z - center.z);

                    float angle = Vector2.Angle(first_vector, second_vector);
                    first_hit.transform.Rotate(0f, 0f, touch.deltaPosition.x);
                }
            }
        }

    }
}