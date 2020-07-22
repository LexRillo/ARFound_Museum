using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

// Original script https://www.youtube.com/watch?v=nX682vFtT6I

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Light))]
public class FocusLight : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        light = GetComponent<Light>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MirrorHandle.position == ReferenceMirrorHandle.position && Stand.position == ReferenceStand.position && 
            MirrorHandle.rotation.eulerAngles.z > 0 && MirrorHandle.rotation.eulerAngles.z < 60)
        {
            // Acceptable angles for mirror -> 
            //Debug.Log(MirrorHandle.rotation.eulerAngles);
            //float dampening = Vector3.Distance(iTakeCandle.position, ReferenceiTakeCandle.position) + 
            //    (MirrorHandle.rotation.eulerAngles.z - ReferenceMirrorHandle.rotation.eulerAngles.z)/6;
            float dampening = Vector3.Distance(iTakeCandle.position, ReferenceiTakeCandle.position)/5 + 
                (MirrorHandle.rotation.eulerAngles.z - ReferenceMirrorHandle.rotation.eulerAngles.z) / 600;
            //Debug.Log(dampening);
            if (dampening >= maximumOpacity)
            {
                filter.mesh = null;
            }
            mesh = BuildMesh(dampening);
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

        float farPosition = Mathf.Tan(light.spotAngle * 0.5f * Mathf.Deg2Rad) * light.range;

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
