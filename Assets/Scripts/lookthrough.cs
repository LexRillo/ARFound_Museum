using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Collections;


// Original code: https://github.com/Unity-Technologies/arfoundation-samples
// This script allows smiulating a user looking through a microscope. It uses the face material sample from ARFoundation but it is heavily modified

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARSession))]
    [RequireComponent(typeof(ARFaceManager))]
    [RequireComponent(typeof(ARSessionOrigin))]
    public class lookthrough : MonoBehaviour
    {
        ARCameraManager m_CameraManager;

        // Only objects with this tag will be able available to be moved
        [SerializeField] private string selectableTag = "Selectable";
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material defaultMaterial;
        Transform _selection;

        // If the conditions are met, the interaction scene will change to the one declared on this variable
        public string SceneToSwitchTo;
        // This is used to transition from one scene to another
        public GameObject TransitionObject;
        private Transform handle_selected = null;
        //

        void Awake()
        {
            m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
            // Initializing the variables on the static class indicating the kind of eyepiece and slide that the lookthrough scene will use
            StaticContainer.lensIndex = 1;
            StaticContainer.slideIndex = 0;
        }

        void Update()
        {
            // If there is an object already selected then reset then reset the material and the selection tracking variable
            if (_selection != null)
            {
                _selection.GetComponent<ChangeToScriptedMat>().revertToOriginalMat();                               
                _selection = null;
            }

            // prepare raycast
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            RaycastHit hit;

            // check if the raycast hit an object
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;

                // If the hit object ("selection") is able to change material then continue
                if (selection.GetComponent<ChangeToScriptedMat>() != null)
                {
                    // Change the material
                    selection.GetComponent<ChangeToScriptedMat>().changeMat(highlightMaterial);
                    // If an eyepiece is in the appropriate position on the microscope and the device is on top of it the change to the "lookthrough" scene
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

            // If the user touches and slides the dial then rotate it using MoveMirrorHolder_Rotation function
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

        // function used to change scene
        IEnumerator switchscene()
        {
            // Animate the transition while loading
            TransitionObject.SetActive(true);
            Animator transitionSceneAnim = TransitionObject.GetComponent<Animator>();
            transitionSceneAnim.Play("Microscope_fade_in");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync(SceneToSwitchTo, LoadSceneMode.Single);
        }

        // unused function for raycasting
        public Ray myRayCast()
        {
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);
            return ray;
        }

        // getter for the selection variable
        public Transform getSelection()
        {
            return _selection;
        }

        // behaviour for the reset button. All the objects will return to the corresponding positions 
        public void resetPositions()
        {
            GameObject[] objs;
            GameObject[] refobjs;
            // objects displayed and interactible in the scene
            objs = GameObject.FindGameObjectsWithTag("ARContainer");
            // "clone" of the original objects which are not interactable and are use to store the correct default values of the original
            refobjs = GameObject.FindGameObjectsWithTag("ARrefpositions");

            // for each object and each child reset to the values given by refobjs
            if (objs.Length > 0 && refobjs.Length > 0)
            {
                for (int k = 0; k < objs.Length; k++)
                {
                    for (int i = 0; i < objs[k].transform.childCount; i++)
                    {
                        GameObject this_child = objs[k].transform.GetChild(i).gameObject;
                        GameObject this_ref_child = refobjs[k].transform.GetChild(i).gameObject;
                        if (this_ref_child.CompareTag("refobject"))
                        {
                            this_child.transform.position = refobjs[k].transform.GetChild(i).gameObject.transform.position;
                        }
                    }

                }
            }
            // reset the standard values of the static class
            StaticContainer.lensIndex = 1;
            StaticContainer.slideIndex = 0;
        }
        
        //public void MoveMirrorHolder_Selection(Touch touch)
        //{
        //    Camera m_camera = m_CameraManager.GetComponent<Camera>();
        //    Ray ray = m_camera.ScreenPointToRay(touch.position);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit) && hit.transform.name == "microscope_mirror_holder")
        //    {
        //        //Debug.Log("This is where I tocuh " + hit.point + " object transform " + hit.transform.position);
        //        handle_selected = hit.transform;
        //        for (int i = 0; i < hit.transform.childCount; i++)
        //        {
        //            if (hit.transform.GetChild(i).name == "HOLDER_DIAL")
        //            {
        //                //Vector3 center = hit.transform.GetChild(i).GetComponent<Renderer>().bounds.center;
                        
        //            }
        //        }
        //    }
        //}


        // function used to rotate the mirror according to the movement on the dial touched by the user
        public void MoveMirrorHolder_Rotation(Touch touch)
        {
            Camera m_camera = m_CameraManager.GetComponent<Camera>();
            // get the first touched point on the screen
            Vector2 first_point = touch.position - touch.deltaPosition;
            Ray first_ray = m_camera.ScreenPointToRay(first_point);
            RaycastHit first_hit;
            // get the projected point from the first touched point on the screen
            if (Physics.Raycast(first_ray, out first_hit) && first_hit.transform.name == "microscope_mirror_holder")
            {
                // get the projected point from the last touched point on the screen
                Ray second_ray = m_camera.ScreenPointToRay(first_point);
                RaycastHit second_hit;
                if (Physics.Raycast(second_ray, out second_hit) && second_hit.transform.name == "microscope_mirror_holder")
                {
                    // calculate the angle moved between the two points with respect to the mirror center
                    Vector3 center = first_hit.transform.FindChild("HOLDER_DIAL").GetComponent<Renderer>().bounds.center;

                    Vector2 first_vector = new Vector2(first_hit.point.y - center.y, first_hit.point.z - center.z);
                    Vector2 second_vector = new Vector2(second_hit.point.y - center.y, second_hit.point.z - center.z);

                    float angle = Vector2.Angle(first_vector, second_vector);

                    // Rotate the mirror by the calculated angle
                    first_hit.transform.Rotate(0f, 0f, touch.deltaPosition.x);
                }
            }
        }

    }
}