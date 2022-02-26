using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class TrapExplosionCtr : MonoBehaviour
    {
        public BoxCollider boxCollider;
        public AudioClip audioClip;
        // Start is called before the first frame update
        private void OnEnable()
        {
            boxCollider.enabled = true;
            SoundManager.Instance.PlaySound(audioClip, this.transform.position,false);
        }
        void Start()
        {
            Invoke("StopAttack", 0.1f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StopAttack()
        {
            boxCollider.enabled = false;
        }
    }
}

