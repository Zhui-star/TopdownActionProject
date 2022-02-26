using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class TrailAchieve : MonoBehaviour
    {
        public static TrailAchieve _instance;
        public ParticleSystem beginnerSlashCombo1;
        public ParticleSystem beginnerSlashCombo2;
        public ParticleSystem beginnerPunchCombo3;

        private void Awake()
        {
            _instance = this;
        }
    }
}

