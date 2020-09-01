using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to allow for changing the material of an object and reverting to their original material
// This script is usually called by the ray casting 

public class ChangeToScriptedMat : MonoBehaviour
{
    // object this script is attached to
    public GameObject corresponding_object;

    public void changeMat(Material newMaterial)
    {
        // gather the renderer and change the material if it is not null. If the object has no renderer (it is null) 
        // and it has children then apply this function recursively.
        // This is needed for object that have multiple components inside such as the stand
        Renderer selectionRenderer = this.transform.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            selectionRenderer.material = newMaterial;
        }
        if (selectionRenderer == null && this.transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var s = this.transform.GetChild(i);                
                selectionRenderer = s.GetComponent<Renderer>();
                if (selectionRenderer != null && s.tag != "NoHighlight")
                {
                    selectionRenderer.material = newMaterial;
                }
            }
        }
    }

    // Apply same operation as changeMat but using the original material of the object
    public void revertToOriginalMat()
    {
        Renderer selectionRenderer = this.transform.GetComponent<Renderer>();
        Renderer correspondingRenderer = corresponding_object.transform.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            selectionRenderer.material = correspondingRenderer.material;
        }
        if (selectionRenderer == null && this.transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var s = this.transform.GetChild(i);                
                var c_s = corresponding_object.transform.GetChild(i);
                selectionRenderer = s.GetComponent<Renderer>();
                correspondingRenderer = c_s.transform.GetComponent<Renderer>();
                if (selectionRenderer != null && s.tag != "NoHighlight")
                {
                    selectionRenderer.material = correspondingRenderer.material;
                }                              
            }
        }
    }
}
