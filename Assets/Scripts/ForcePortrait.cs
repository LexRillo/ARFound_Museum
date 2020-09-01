using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that forces the the engine to show in the content Portrait mode
public class ForcePortrait : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
