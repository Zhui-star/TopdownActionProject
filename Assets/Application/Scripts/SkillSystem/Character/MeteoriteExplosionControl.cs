using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class MeteoriteExplosionControl : MonoBehaviour
    {
        public SpikeSkill spikeSkill;

        private void OnEnable()
        {
            spikeSkill.enabled = true;
            Invoke("DisableSpikeSkill", 0.2f);
        }

        private void DisableSpikeSkill()
        {
            spikeSkill.enabled = false;
            Debug.Log("Spike sill disable");
        }
    }
}

