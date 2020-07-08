using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToScriptedMat : MonoBehaviour
{
    public Material regularMat;

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
        Renderer selectionRenderer = this.transform.GetComponent<Renderer>();
        if (selectionRenderer != null)
        {
            selectionRenderer.material = regularMat;
        }
        if (selectionRenderer == null && this.transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var s = this.transform.GetChild(i);
                selectionRenderer = s.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = regularMat;
                }
            }
        }
    }
}
