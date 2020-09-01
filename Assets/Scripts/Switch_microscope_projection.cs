using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFaceManager))]
public class Switch_microscope_projection : MonoBehaviour
{
    private ARFaceManager arFaceManager;

    [SerializeField]
    // slides from that appear in the interaction scene
    public VitrineMaterial[] vitrines;
    // troubleshooting message
    public GameObject NofaceMessage;
    private float distance;
    public RectTransform microscope_vision;
    Vector3 initialScale = new Vector3(1.3f, 1.3f, 1.3f);
    // Material for blurr effect
    Material blurEffectMat;

    // Prepare scene with the values given from the interaction scene
    void Awake() 
    {
        arFaceManager = GetComponent<ARFaceManager>();
        int lensValue = StaticContainer.lensIndex;
        int slidesValue = StaticContainer.slideIndex;
        arFaceManager.facePrefab.transform.GetChild(0).GetComponent<MeshRenderer>().material = vitrines[slidesValue].Material;
        Debug.Log(vitrines[slidesValue].Material.name);

        // Get blur material and apply if using an incorrect eyepiece for a slide
        blurEffectMat = arFaceManager.facePrefab.transform.GetChild(6).GetComponent<MeshRenderer>().material;
        if (lensValue ==1 && (slidesValue == 1 || slidesValue == 2))
        {
            changeBlurriness(0);
        }
        else if(lensValue == 2 && slidesValue == 3)
        {
            changeBlurriness(0);
        }
    }

    private void Update()
    {
        // Show troubleshooting message if the face is too far from the camera
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

    // Method find the distance and rerturning whether it is too far from the camera
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

    // apply blur effect from the material
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