using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using System.Linq;

// When corresponding button is pressed, this behaviour will run and highlight the interactible parts and make the other parts transparent.
namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARSession))]
    [RequireComponent(typeof(ARSessionOrigin))]
    public class ShowInteractable : MonoBehaviour
    {
        [SerializeField] private Material noInteractionMaterial;
        [SerializeField] private Material interactionMaterial;
        // List of the objects included as interactible
        [SerializeField] string[] nameInteractiveArray;
        private GameObject[] objs;
        private GameObject clone;
        public void showIteract()
        { 
            // Find all the root objects of the scene
            objs = GameObject.FindGameObjectsWithTag("ARContainer");
            
            if (objs.Length > 0)
            {
                // for each root object in the scene, create a clone and change its material according using the changeAllObjWithMaterial function
                // Then deactivate the original objects and only display the clone
                for (int i = 0; i < objs.Length; i++)
                {
                    clone = Instantiate(objs[i], objs[i].transform.position, objs[i].transform.rotation);
                    changeAllOBjWithMaterial(clone, noInteractionMaterial, nameInteractiveArray, interactionMaterial);
                    objs[i].SetActive(false);
                }
            }            
        }

        // When the behaviour is done, the scene is reset by destroying the clone and reactivating the original root objects
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

        // Function to cange the material of listed objects
        public void changeAllOBjWithMaterial(GameObject obj, Material generalMat, string[] nameInteractiveArray, Material interactionMaterial)
        {
            // If the object renderer exists then change the material
            if (obj.GetComponent<Renderer>() != null)
            {
                var selectionRenderer = obj.transform.GetComponent<Renderer>();
                // If the name of the object is in the list then apply the highlight material
                if (nameInteractiveArray.Contains<string>(obj.name))
                {
                    selectionRenderer.material = interactionMaterial;
                }
                else
                {
                    selectionRenderer.material = generalMat;
                }
            }
            // If the object renderer does not exist but the object has children, apply this function recursively
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