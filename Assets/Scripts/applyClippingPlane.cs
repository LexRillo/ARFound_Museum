using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to apply the clipping plane shader material to a regular object
public class applyClippingPlane : MonoBehaviour
{
    public GameObject objectToClip;
    public Material mat;
    public Material newmat;
    private bool alreadyCliped = false;

    // Apply the given material to the object
    private void setMaterial(GameObject Go, Material m)
    {
        // iterate through the children and recursively change the material from the renderer.
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            GameObject child = Go.transform.GetChild(i).gameObject;
            // exclude objects with the tag "NoHighlight"
            if (child.GetComponent<Renderer>() != null) {
                child.GetComponent<Renderer>().material = m;
            }
            setMaterial(child, m);
        }
    }

    // Only apply the new material when it is not clipped
    public void applyNewMat()
    {
        if (alreadyCliped == false) {
            setMaterial(objectToClip, newmat);
            alreadyCliped = true;
        }
        else
        {
            setMaterial(objectToClip, mat);
            alreadyCliped = false;
        }
        
    }
}
