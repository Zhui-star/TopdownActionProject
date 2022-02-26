using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using HTLibrary.Utility;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class TaltAwdChestCtrl : MonoBehaviour
    {
        [Header("关卡宝箱配置")]
        public AudioClip triggerClip;

        public Animator anim;
        public Animator talent1Anim;
        public Animator talent2Anim;
        public Animator talent3Anim;

        public GameObject smokeClounds;
        public GameObject[] talent;
        private bool isOpen = false;
        public bool isGotten = false; //是否有天赋获取到
        private bool isOnGround = false;
        private Rigidbody rigidbody;
        public MMFeedbacks mMFeedbakcs; 

        public Collider _preventCollder;// Protect player collider with talent when chest start open 
        private Collider _trigger;
        private void OnEnable()
        {
            isGotten = false;
            isOnGround = false;
        }
        private void Start()
        {
            rigidbody = this.GetComponent<Rigidbody>();
            _trigger=GetComponent<Collider>();
            mMFeedbakcs?.Initialization(this.gameObject);
            _preventCollder.enabled=false;
        }
        // Update is called once per frame
        void Update()
        {
            if (isGotten == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject currentTalent = talent[i].transform.Find("StateActivated(Clone)").gameObject;
                    if (currentTalent.activeSelf&&currentTalent.GetComponent<StateActivated>().isGotten==false)
                    {
                        talent[i].SetActive(false);
                        GameObject castEff = PoolManagerV2.Instance.GetInst("TalentNotChoose");
                        castEff.transform.position = talent[i].transform.position;
                        _preventCollder.enabled=false;
                    }
                }
                isGotten = false;
            }
        }

        /// <summary>
        /// 遇到玩家开始指引交互
        /// </summary>
        /// <param name="collidingObject"></param>
        private void OnTriggerEnter(Collider collidingObject)
        {
            if (!isOnGround&&collidingObject.gameObject.layer == 9)
            {
                smokeClounds.SetActive(true);
                isOnGround = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                mMFeedbakcs?.PlayFeedbacks();
                _preventCollder.enabled=true;
                StartCoroutine(ReOpenTrigger());
                this.transform.SetLocalPosY(0);
            }
            
            if (isOnGround == true&& collidingObject.tag == Tags.Player)
            {

                if (triggerClip != null && isOpen == false)
                {
                    SoundManager.Instance.PlaySound(triggerClip, transform.position, false);
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject talentActivated = PoolManagerV2.Instance.GetInst("StateActivated");

                        talentActivated.transform.position = talent[i].transform.position;
                        talentActivated.transform.SetParent(talent[i].transform);
                    }
                    isOpen = true;
                    if (anim != null)
                    {
                        anim.SetBool("Interactive", true);
                    }
                    if (talent1Anim != null && talent2Anim != null && talent3Anim != null)
                    {
                        talent1Anim.SetBool("Interactive", true);
                        talent2Anim.SetBool("Interactive", true);
                        talent3Anim.SetBool("Interactive", true);
                    }
                }
                //triggerFeedBack?.Play(this.transform.position);

               
            }   
        }

        /// <summary>
        /// Re open trigger when chest trigger ground
        /// </summary>
        /// <returns></returns>
        IEnumerator ReOpenTrigger()
        {
            _trigger.enabled=false;
            yield return null;
            _trigger.enabled=true;
        }

    }
}

