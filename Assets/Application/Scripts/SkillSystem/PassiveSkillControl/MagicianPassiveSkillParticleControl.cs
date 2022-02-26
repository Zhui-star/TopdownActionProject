using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianPassiveSkillParticleControl : MonoBehaviour
{
    public ParticleSystem MagicianParticleSystem;
    public static MagicianPassiveSkillParticleControl _instance;

    // Update is called once per frame
    private void Start()
    {
        MagicianParticleSystem = GetComponent<ParticleSystem>();
        _instance = this;
        //Debug.Log("particle");
    }
}
