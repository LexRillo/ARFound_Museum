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
        //

        void Awake()
        {
            m_CameraManager = GetComponent<ARSessionOrigin>().camera?.GetComponent<ARCameraManager>();
            StaticContainer.lensIndex = 1;
            StaticContainer.slideIndex = 0;
        }

        void Update()
        {
            Debug.Log("lensIndex" + StaticContainer.lensIndex + StaticContainer.slideIndex);
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
    }
}