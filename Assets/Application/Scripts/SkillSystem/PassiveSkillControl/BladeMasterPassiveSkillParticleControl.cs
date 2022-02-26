using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class BladeMasterPassiveSkillParticleControl : MonoBehaviour
    {
        public ParticleSystem BladeMasterParticleSystem;
        public static BladeMasterPassiveSkillParticleControl _instance;

        // Update is called once per frame
        private void Start()
        {
            BladeMasterParticleSystem = GetComponent<ParticleSystem>();
            _instance = this;
            //Debug.Log("particle");
        }
    }
}

