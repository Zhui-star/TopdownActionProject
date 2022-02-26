using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class BerserkerPassiveSkillControl : MonoBehaviour
    {
        public ParticleSystem[] particleSystems;
        public static BerserkerPassiveSkillControl _instance;
        // Start is called before the first frame update
        void Start()
        {
            _instance = this;
        }  
    }
}

