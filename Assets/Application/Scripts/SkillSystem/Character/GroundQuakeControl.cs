using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class GroundQuakeControl : MonoBehaviour
    {
        public string ExplosionName;
        public ParticleSystem particleSystem;

        private void OnEnable()
        {
            //gameObject.SetActive(false);
            Invoke("StopParticle", 1.8f);
            Invoke("StartCrevice", 2f);
            
        }

        public void StartCrevice()
        {
            if (ExplosionName != "")
            {
                GameObject castEff = PoolManagerV2.Instance.GetInst(ExplosionName);

                castEff.transform.position = this.transform.position;
                //castEff.transform.rotation = this.transform.rotation;
            }

        }

        public void StopParticle()
        {
            particleSystem.Stop();
        }
    }

}
