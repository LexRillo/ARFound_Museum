using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applyClippingPlaneToPrefab : MonoBehaviour
{
    
    public Material mat;
    public Material newmat;
    private bool alreadyCliped = false;
    private void setMaterial(GameObject Go, Material m)
    {
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            GameObject child = Go.transform.GetChild(i).gameObject;
            if (child.GetComponent<Renderer>() != null) {
                child.GetComponent<Renderer>().material = m;
            }
            setMaterial(child, m);
        }
    }

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
