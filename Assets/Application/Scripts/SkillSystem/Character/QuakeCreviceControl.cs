using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    public class QuakeCreviceControl : MonoBehaviour
    {
        public bool isNormal;
        public SpikeSkill spikeSkill;
        public MMFeedbacks feedBack;
        public AudioClip audioClip;
        private void OnEnable()
        {
            spikeSkill.enabled = true;
            isNormal = false;
            Invoke("DisableSpikeSkill", 0.2f);
            feedBack?.PlayFeedbacks();
            SoundManager.Instance.PlaySound(audioClip, this.transform.position, false);
        }

        // Start is called before the first frame update
        void Start()
        {
            feedBack?.Initialization(this.gameObject);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isNormal == false)
            {
                // this.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.8f, this.transform.position.z);
                isNormal = true;
            }
        }

        public void DisableSpikeSkill()
        {
            spikeSkill.enabled = false;
        }
    }
}

