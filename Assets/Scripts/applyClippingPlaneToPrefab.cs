using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to apply the clipping plane shader material to a prefab and allow the light particle animation to be played
public class applyClippingPlaneToPrefab : MonoBehaviour
{
    
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
            if (child.GetComponent<Renderer>() != null && child.tag != "NoHighlight") {
                child.GetComponent<Renderer>().material = m;
            }
            setMaterial(child, m);
        }
    }


    // Only apply the new material to the object and its children when they are not clipped
    public void applyNewMat()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ARContainer");
        if (objs.Length > 0)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (alreadyCliped == false)
                {
                    setMaterial(objs[i], newmat);
                    alreadyCliped = true;
                }
                else
                {
                    setMaterial(objs[i], mat);
                    alreadyCliped = false;
                }
            }
        }      
    }

    // get light partivle and animate it
    public void playAnim()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ARContainer");
        if (objs.Length > 0)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].GetComponent<AnimateLightParticle>().startTheAnimation();
            }
        }
    }
}
