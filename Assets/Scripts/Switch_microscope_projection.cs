using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class Switch_microscope_projection : MonoBehaviour
{
    private ARFaceManager arFaceManager;

    [SerializeField]
    public VitrineMaterial[] vitrines;
    public GameObject NofaceMessage;
    private float distance;
    public RectTransform microscope_vision;
    Vector3 initialScale = new Vector3(1.3f, 1.3f, 1.3f);
    Material blurEffectMat;
    void Awake() 
    {
        arFaceManager = GetComponent<ARFaceManager>();
        int value = StaticContainer.lensIndex;
        arFaceManager.facePrefab.transform.GetChild(0).GetComponent<MeshRenderer>().material = vitrines[value].Material;
        blurEffectMat = arFaceManager.facePrefab.transform.GetChild(6).GetComponent<MeshRenderer>().material;
        Debug.Log(blurEffectMat.name);
    }

    private void Update()
    {
        if (checkIfFaceIsClose())
        {
            NofaceMessage.SetActive(false);
        }
        else
        {
            NofaceMessage.SetActive(true);
            //float perceptual_distance = 1 - ((distance - 0.15f) / (0.35f - 0.15f));
            //Vector3 vec_of_distance = new Vector3(perceptual_distance, perceptual_distance, perceptual_distance);
            ////Debug.Log("The distance" + perceptual_distance);
            ////var additive = microscope_vision.localScale.x + perceptual_distance;
            ////Debug.Log("Scale" + additive);
            ////microscope_vision.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, );
            //microscope_vision.localScale = initialScale + vec_of_distance;
            //microscope_vision.ForceUpdateRectTransforms();
        }
    }

    private bool checkIfFaceIsClose()
    {
        if (arFaceManager.trackables.count > 0)
        {
            foreach (var face in arFaceManager.trackables)
            {
                distance = face.transform.position.z;
                if (face.transform.position.z < 0.75)
                {
                    distance = face.transform.position.z;
                    return true;
                }
            }

        }
        return false;
    }

    public void changeBlurriness(float bluriness_index)
    {
        blurEffectMat.SetFloat("_Size", bluriness_index);
    }
}

[System.Serializable]
public class VitrineMaterial
{
    public Material Material;

    public string Name;
}