using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    /// <summary>
    /// 碰撞特效反馈
    /// </summary>
    public class HTColliderFeedBack : MonoBehaviour
    {
        public LayerMask layerMask;
        public string feedbackName;

        [Header("音效反馈管理")]
        public AudioClip clip;
        [Header("是否只有一次")]
        public bool JustOnce = true;
        [Header("是否只有一次，但是不销毁")]
        public bool JustOnceButDontDestroy = false;
        bool IsResponse = false;

        private GameObject Owner;

        public MMFeedbacks colliderFeedbacks;

        private ObjectTimeEvent objectTimeEvent;

        private void Awake()
        {
            objectTimeEvent = GetComponent<ObjectTimeEvent>();
        }

        private void Start()
        {
            colliderFeedbacks?.Initialization();
           
        }

        public void SetOwner(GameObject owner)
        {
            this.Owner = owner;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!IsResponse&&MMLayers.LayerInLayerMask(collider.gameObject.layer, layerMask))
            {
                if (feedbackName != "")
                {
                    if(objectTimeEvent!=null)
                    {
                        objectTimeEvent.StopInvoke();
                    }

                    GameObject castEff = PoolManagerV2.Instance.GetInst(feedbackName);

                    castEff.transform.position = this.transform.position;
                    castEff.transform.rotation = this.transform.rotation;

                   SpikeSkill spikeSkill=  castEff.GetComponent<SpikeSkill>();
                    if(spikeSkill!=null)
                    {
                        spikeSkill.Owner = Owner;
                    }


                }

                if (clip != null)
                {
                    SoundManager.Instance.PlaySound(clip, transform.position, false);
                }

                colliderFeedbacks?.PlayFeedbacks();

                if (JustOnce)
                {
                    this.gameObject.SetActive(false);
                }

                if(JustOnceButDontDestroy)
                {
                    IsResponse = true;
                }

            }

            
           


        }

        /// <summary>
        /// 重置
        /// </summary>

        private void OnDisable()
        {
            IsResponse = false;
        }
    }

}
