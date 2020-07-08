using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArObject : MonoBehaviour
{
    public GameObject theSession;
    public GameObject ARcamera;
    
    private Transform _selection = null;
    private GameObject original = null;
    private GameObject clone;


    public void holdingObject()
    {
        _selection = theSession.GetComponent<UnityEngine.XR.ARFoundation.Samples.lookthrough>().getSelection();
        if (_selection != null)
        {
            original = _selection.gameObject;
                        
            clone = Instantiate(original, _selection.position, _selection.rotation);
            clone.transform.localScale = original.transform.localScale;
            clone.transform.parent = ARcamera.transform;

            var selectionRenderer = original.GetComponent<Renderer>();
            if (selectionRenderer == null)
            {
                for (int i = 0; i < original.transform.childCount; i++)
                {
                    var s = original.transform.GetChild(i);
                    selectionRenderer = s.GetComponent<Renderer>();
                    selectionRenderer.enabled = false;
                }
            }
            else
            {
                selectionRenderer.enabled = false;
            }
            
        }
    }

    public void droppingObject()
    {
        if (original != null) {

            // Debug.Log(getCloseness());
            // if closeness > 0.992 then original.transform.position = the position of the reference
            Vector3 resulting_position = getCloseness();
            if (resulting_position != Vector3.zero)
            {
                original.transform.position = resulting_position;
            }
            else
            {
                original.transform.position = clone.transform.position;
            }
            
            Destroy(clone);
            //original.transform.position = positionToHold.position;
            var selectionRenderer = original.GetComponent<Renderer>();
            if (selectionRenderer == null)
            {
                for (int i = 0; i < original.transform.childCount; i++)
                {
                    var s = original.transform.GetChild(i);
                    selectionRenderer = s.GetComponent<Renderer>();
                    selectionRenderer.enabled = true;
                }
            }
            else
            {
                selectionRenderer.enabled = true;
            }
        }        
    }

    public Vector3 getCloseness()
    {
        Ray this_ray = theSession.GetComponent<UnityEngine.XR.ARFoundation.Samples.lookthrough>().myRayCast();
        GameObject[] refobjs = GameObject.FindGameObjectsWithTag("ARrefpositions");
        if (refobjs.Length > 0)
        {
            for (int k = 0; k < refobjs.Length; k++)
            {
                if (original.name == "Upper_lens_holder_microscope" || original.name == "Upper_lens_holder_microscope1")
                {
                    Transform corresponding_child = refobjs[k].transform.Find("Upper_lens_holder_microscope");
                    if (corresponding_child != null)
                    {
                        //Debug.Log("correspondingchidl" + corresponding_child.position);
                        float closeness = Vector3.Dot(this_ray.direction.normalized, (corresponding_child.position - this_ray.origin).normalized);
                        Debug.Log("closeness " +closeness);
                        if (closeness > 0.992)
                        {
                            return corresponding_child.position;
                        }
                    }
                } else if (original.name == "microscope_slide_bacteria" || original.name == "microscope_slide_microbes" || original.name == "microscope_slide_insects")
                {
                    Transform corresponding_child = refobjs[k].transform.Find("positionForSlides");
                    if (corresponding_child != null)
                    {
                        //Debug.Log("correspondingchidl" + corresponding_child.position);
                        float closeness = Vector3.Dot(this_ray.direction.normalized, (corresponding_child.position - this_ray.origin).normalized);
                        if (closeness > 0.992)
                        {
                            return corresponding_child.position;
                        }
                    }
                }               
            }
        }
        return Vector3.zero;
    }
}
