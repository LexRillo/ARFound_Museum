using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// This script creates the mesh from the attributes of the light to create a cone of dim light. 
// This cone of light represents the light reflected from the mirror passing and passes through the sample hole
// Original script https://www.youtube.com/watch?v=nX682vFtT6I

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Light))]
public class FocusLight : MonoBehaviour
{
    // maximum opacity is kept low for a "volumetric light"-kind of effect
    public float maximumOpacity = 0.05f;

    private MeshFilter filter;
    private Light light;

    private Mesh mesh;

    public Transform MirrorHandle;
    public Transform ReferenceMirrorHandle;
    public Transform Stand;
    public Transform ReferenceStand;
    public Transform iTakeCandle;
    public Transform ReferenceiTakeCandle;

    void Start()
    {
        filter = GetComponent<MeshFilter>();
        light = GetComponent<Light>();        
    }

    // Update is called once per frame
    void Update()
    {
        // Conditions: The mirror is in the default position, the candle is close to the mirror, the mirror is at a sufficient angle (0-60 degrees) to reflect some light.
        // Do not draw the cone of light if the conditions are not met
        if (MirrorHandle.position == ReferenceMirrorHandle.position && Stand.position == ReferenceStand.position && 
            MirrorHandle.rotation.eulerAngles.z > 0 && MirrorHandle.rotation.eulerAngles.z < 60)
        {
            // slight rotations of the mirror dampen the cone of light making it look dimmer
            float dampening = Vector3.Distance(iTakeCandle.position, ReferenceiTakeCandle.position)/5 + 
                (MirrorHandle.rotation.eulerAngles.z - ReferenceMirrorHandle.rotation.eulerAngles.z) / 600;
            if (dampening >= maximumOpacity)
            {
                filter.mesh = null;
            }
            // Use the Build Mesh function with the calculated dampening
            mesh = BuildMesh(dampening);
            // Apply the resulting mesh
            filter.mesh = mesh;
        }
        else
        {
            filter.mesh = null;
        }
        
    }

    private Mesh BuildMesh(float damp)
    {
        mesh = new Mesh();

        // using the light attributes calculate the dimension of the cone.
        float farPosition = Mathf.Tan(light.spotAngle * 0.5f * Mathf.Deg2Rad) * light.range;

        // Building the neccessaty components for a mesh
        mesh.vertices = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(farPosition, farPosition, light.range),
            new Vector3(-farPosition, farPosition, light.range),
            new Vector3(-farPosition, -farPosition, light.range),
            new Vector3(farPosition, -farPosition, light.range)
        };
        mesh.colors = new Color[] {
            new Color(light.color.r, light.color.g, light.color.b, light.color.a * (maximumOpacity-damp)),
            new Color(light.color.r, light.color.g, light.color.b, 0),
            new Color(light.color.r, light.color.g, light.color.b, 0),
            new Color(light.color.r, light.color.g, light.color.b, 0),
            new Color(light.color.r, light.color.g, light.color.b, 0)
        };
        mesh.triangles = new int[] {
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,1
        };

        return mesh;
    }
}
