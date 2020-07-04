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
            original.transform.position = clone.transform.position;
            Destroy(clone);
            //original.transform.position = positionToHold.position;
            original.GetComponent<Renderer>().enabled = true;
        }        
    }
}
