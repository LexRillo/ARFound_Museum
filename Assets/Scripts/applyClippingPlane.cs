using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applyClippingPlane : MonoBehaviour
{
    public GameObject objectToClip;
    public Material mat;
    public Material newmat;
    private bool alreadyCliped = false;
    // Start is called before the first frame update
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
