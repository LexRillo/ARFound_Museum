using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Linq;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARSession))]
    [RequireComponent(typeof(ARSessionOrigin))]
    public class ShowInteractable : MonoBehaviour
    {
        //ARTrackedImageManager m_ImageManager;

        ////
        //[SerializeField] private Material noInteractionMaterial;
        //[SerializeField] private Material interactionMaterial;
        //[SerializeField] string[] nameInteractiveArray;
        //private GameObject clone;
        //ARTrackedImage trackedImage;
        ////

        //void Awake()
        //{
        //    m_ImageManager = GetComponent<ARTrackedImageManager>();
        //    //m_ImageManager.trackedImagePrefab
        //    //ARTrackedImage trim = m_ImageManager.trackables.t
        //}

        //public void showIteract()
        //{
        //    if (m_ImageManager.trackedImagePrefab.activeSelf == true)
        //    {
        //        clone = Instantiate(m_ImageManager.trackedImagePrefab.gameObject, trackedImage.transform.position, trackedImage.transform.rotation);
        //        changeAllOBjWithMaterial(clone, noInteractionMaterial, nameInteractiveArray, interactionMaterial);
        //        for (int i = 0; i < m_ImageManager.trackedImagePrefab.transform.childCount; i++)
        //        {
        //            var s = m_ImageManager.trackedImagePrefab.transform.GetChild(i).gameObject;
        //            s.SetActive(false);
        //        }
        //    }
        //}

        //public void returnInteract()
        //{
        //    if (m_ImageManager.trackedImagePrefab.activeSelf == true)
        //    {
        //        Destroy(clone);
        //        for (int i = 0; i < m_ImageManager.trackedImagePrefab.transform.childCount; i++)
        //        {
        //            var s = m_ImageManager.trackedImagePrefab.transform.GetChild(i).gameObject;
        //            s.SetActive(true);
        //        }
        //    }
        //}

        //public void changeAllOBjWithMaterial(GameObject obj, Material generalMat, string[] nameInteractiveArray, Material interactionMaterial)
        //{
        //    if (obj.GetComponent<Renderer>() != null)
        //    {
        //        var selectionRenderer = obj.transform.GetComponent<Renderer>();
        //        if (nameInteractiveArray.Contains<string>(obj.name))
        //        {
        //            selectionRenderer.material = interactionMaterial;
        //        }
        //        else
        //        {
        //            selectionRenderer.material = generalMat;
        //        }
        //    }
        //    else if (obj.transform.childCount > 0)
        //    {
        //        for (int i = 0; i < obj.transform.childCount; i++)
        //        {
        //            var s = obj.transform.GetChild(i).gameObject;
        //            changeAllOBjWithMaterial(s, generalMat, nameInteractiveArray, interactionMaterial);
        //        }
        //    }
        //    obj.tag = "Untagged";
        //}

        [SerializeField] private Material noInteractionMaterial;
        [SerializeField] private Material interactionMaterial;
        [SerializeField] string[] nameInteractiveArray;
        private GameObject[] objs;
        private GameObject clone;
        public void showIteract()
        {            
            objs = GameObject.FindGameObjectsWithTag("ARContainer");
            Debug.Log("number of objects" + objs.Length);
            if (objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    clone = Instantiate(objs[i], objs[i].transform.position, objs[i].transform.rotation);
                    changeAllOBjWithMaterial(clone, noInteractionMaterial, nameInteractiveArray, interactionMaterial);
                    objs[i].SetActive(false);
                }
            }            
        }

        public void returnInteract()
        {
            if (objs.Length > 0)
            {
                Destroy(clone);
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].SetActive(true);
                }
            }
        }

        public void changeAllOBjWithMaterial(GameObject obj, Material generalMat, string[] nameInteractiveArray, Material interactionMaterial)
        {
            if (obj.GetComponent<Renderer>() != null)
            {
                var selectionRenderer = obj.transform.GetComponent<Renderer>();
                if (nameInteractiveArray.Contains<string>(obj.name))
                {
                    selectionRenderer.material = interactionMaterial;
                }
                else
                {
                    selectionRenderer.material = generalMat;
                }
            }
            else if (obj.transform.childCount > 0)
            {
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    var s = obj.transform.GetChild(i).gameObject;
                    changeAllOBjWithMaterial(s, generalMat, nameInteractiveArray, interactionMaterial);
                }
            }
            obj.tag = "Untagged";
        }
    }
}