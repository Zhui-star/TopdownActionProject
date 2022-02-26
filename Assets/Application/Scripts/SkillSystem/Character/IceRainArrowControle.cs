using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class IceRainArrowControle : MonoBehaviour
    {
        //public ParticleSystem subCollision;
        public string ExplosionName;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDisable()
        {
            if (ExplosionName != "")
            {
                GameObject castEff = PoolManagerV2.Instance.GetInst(ExplosionName);

                castEff.transform.position = this.transform.position;
                //castEff.transform.rotation = this.transform.rotation;
            }
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (this.gameObject.activeSelf == false)
        //    {
        //        if (ExplosionName != "")
        //        {
        //            GameObject castEff = PoolManagerV2.Instance.GetInst(ExplosionName);

        //            castEff.transform.position = this.transform.position;
        //            //castEff.transform.rotation = this.transform.rotation;
        //        }
        //    }

           
        //}

            
                
            

    }

}
