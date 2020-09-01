using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static class containing the index of eyepieces and slides. It is used to transfer this information between the 
// interaction scene and the "lookthrough" scene
public static class StaticContainer {
    public static int lensIndex { get; set; }
    public static int slideIndex { get; set; }
}
