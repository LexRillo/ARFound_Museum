using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToScriptedMat : MonoBehaviour
{
    public GameObject corresponding_object;

    public void changeMat(Material newMaterial)
    {
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
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = newMaterial;
                }
            }
        }
    }

    public void revertToOriginalMat()
    {
        //Renderer selectionRenderer = this.transform.GetComponent<Renderer>();
        //if (selectionRenderer != null)
        //{
        //    selectionRenderer.material = regularMat;
        //}
        //if (selectionRenderer == null && this.transform.childCount != 0)
        //{
        //    for (int i = 0; i < this.transform.childCount; i++)
        //    {
        //        var s = this.transform.GetChild(i);
        //        selectionRenderer = s.GetComponent<Renderer>();
        //        if (selectionRenderer != null)
        //        {
        //            selectionRenderer.material = regularMat;
        //        }
        //    }
        //}
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
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = correspondingRenderer.material;
                }                              
            }
        }
    }
}
