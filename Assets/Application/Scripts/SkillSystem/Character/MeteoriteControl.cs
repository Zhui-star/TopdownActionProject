using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class MeteoriteControl : MonoBehaviour
    {
        public int height;
        public bool isEmit;
        public MeshRenderer meshRenderer;
        public GameObject effectGameObject;
        public Rigidbody rigidbody;
        public AudioSource audioSource;
        // Start is called before the first frame update
        private void OnEnable()
        {
            isEmit = true;
            meshRenderer.enabled = false;
            effectGameObject.SetActive(false);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isEmit == true)
            {
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + height, this.transform.localPosition.z);
                meshRenderer.enabled = true;
                effectGameObject.SetActive(true);
                isEmit = false;
                rigidbody.velocity = Vector3.zero;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Ground")
            {
                audioSource.Stop();
            }

        }
    }

}
