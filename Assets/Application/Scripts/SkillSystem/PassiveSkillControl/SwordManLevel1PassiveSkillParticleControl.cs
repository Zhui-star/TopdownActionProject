using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManLevel1PassiveSkillParticleControl : MonoBehaviour
{
    public ParticleSystem SwordmanParticleSystem;
    public static SwordManLevel1PassiveSkillParticleControl _instance;

    // Update is called once per frame
    private void Start()
    {
        SwordmanParticleSystem = GetComponent<ParticleSystem>();
        _instance = this;
        //Debug.Log("particle");
    }
}
