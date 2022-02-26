using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Application
{
    public class ArcherPassiveSkillParticleControl : MonoBehaviour
    {
        public ParticleSystem ArcherParticleSystem;
        public static ArcherPassiveSkillParticleControl _instance;

        // Update is called once per frame
        private void Start()
        {
            ArcherParticleSystem = GetComponent<ParticleSystem>();
            _instance = this;
            //Debug.Log("particle");
        }
    }
}

