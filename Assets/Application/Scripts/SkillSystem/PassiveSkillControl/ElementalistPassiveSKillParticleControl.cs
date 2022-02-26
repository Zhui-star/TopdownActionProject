using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Application
{
    public class ElementalistPassiveSKillParticleControl : MonoBehaviour
    {
        public ParticleSystem ElementalistParticleSystem;
        public static ElementalistPassiveSKillParticleControl _instance;
        
        // Update is called once per frame
        private void Start()
        {
            ElementalistParticleSystem = GetComponent<ParticleSystem>();
            _instance = this;

            //Debug.Log("particle");
        }
        private void Update()
        {
            if(ElementalistPassiveSkill._instance.isRelease==false)
            {
                ElementalistParticleSystem.Stop();
            }
        }
    }
}