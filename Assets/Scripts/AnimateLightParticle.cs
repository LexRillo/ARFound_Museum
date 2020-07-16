using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLightParticle : MonoBehaviour
{
    public GameObject light_particle_prefab;

    public void startTheAnimation()
    {
        if (light_particle_prefab.activeSelf){
            Animator anim = light_particle_prefab.GetComponent<Animator>();
            anim.Play("lighttrajectory", -1, 0f);
        }
        else
        {
            light_particle_prefab.SetActive(true);
            Animator anim = light_particle_prefab.GetComponent<Animator>();
            anim.Play("lighttrajectory");
        }
    }
}
