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
                        //Debug.Log("closeness " +closeness);
                        if (closeness > 0.992)
                        {
                            checkIfLensInPos(corresponding_child, original);
                            return corresponding_child.position;
                        }
                    }
                } 
                else if (original.name == "microscope_slide_bacteria" || original.name == "microscope_slide_microbes" || original.name == "microscope_slide_insects")
                {
                    Transform corresponding_child = refobjs[k].transform.Find("positionForSlides");
                    checkIfSlideInPos(corresponding_child, original);
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

    private void checkIfSlideInPos(Transform focus, GameObject selected)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ARContainer");
        GameObject[] refobjs = GameObject.FindGameObjectsWithTag("ARrefpositions");
        if (objs.Length > 0)
        {
            for (int k = 0; k < objs.Length; k++)
            {
                Transform bact_slide = objs[k].transform.Find("microscope_slide_bacteria");
                Transform microbes_slide = objs[k].transform.Find("microscope_slide_microbes");
                Transform insects_slide = objs[k].transform.Find("microscope_slide_insects");

                if (bact_slide.position == focus.position)
                {
                    
                    bact_slide.position = refobjs[k].transform.Find("microscope_slide_bacteria").transform.position;
                }
                else if (microbes_slide.position == focus.position)
                {
                    StaticContainer.slideIndex = 2;
                    microbes_slide.position = refobjs[k].transform.Find("microscope_slide_microbes").transform.position;
                }
                else if (insects_slide.position == focus.position)
                {
                    StaticContainer.slideIndex = 3;
                    insects_slide.position = refobjs[k].transform.Find("microscope_slide_insects").transform.position;
                }
                if (selected.name == "microscope_slide_bacteria")
                {
                    StaticContainer.slideIndex = 1;
                }
                if (selected.name == "microscope_slide_microbes")
                {
                    StaticContainer.slideIndex = 2;
                }
                if (selected.name == "microscope_slide_insects")
                {
                    StaticContainer.slideIndex = 3;
                }
            }
        }
    }

    private void checkIfLensInPos(Transform focus, GameObject selected)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ARContainer");
        GameObject[] refobjs = GameObject.FindGameObjectsWithTag("ARrefpositions");
        if (objs.Length > 0)
        {
            for (int k = 0; k < objs.Length; k++)
            {
                Transform lens1 = objs[k].transform.Find("Upper_lens_holder_microscope");
                Transform lens2 = objs[k].transform.Find("Upper_lens_holder_microscope1");

                if (lens1.position == focus.position && selected.name == "Upper_lens_holder_microscope")
                {
                    StaticContainer.lensIndex = 1;
                }
                else if (lens1.position == focus.position && selected.name == "Upper_lens_holder_microscope1")
                {
                    StaticContainer.lensIndex = 2;
                    lens1.position = refobjs[k].transform.Find("Upper_lens_holder_microscope1").transform.position;
                }
                else if (lens2.position == focus.position && selected.name == "Upper_lens_holder_microscope")
                {
                    StaticContainer.slideIndex = 1;
                    lens2.position = refobjs[k].transform.Find("Upper_lens_holder_microscope1").transform.position;
                }
                else if(lens1.position != focus.position && lens2.position != focus.position)
                {
                    StaticContainer.slideIndex = 0;
                }
            }
        }
    }
}
