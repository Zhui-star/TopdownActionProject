using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using HTLibrary.Application;
using HTLibrary.Utility;
namespace HTLibrary.Framework
{
    public class AutoHide : MonoBehaviour
    {
        public bool canDestroy = false;
        public float time = 1.5f;

        private float lifeTime;
        private float startTime;

        public static AutoHide _instance;

        public MMFeedbacks initialFeedbacks;
        public MMFeedbacks hideFeedbacks;

        public bool activeChilds;

        public bool HideGreatSwordMan = false;

        public string DisableCallBackObjcet;

        private MarkArrow _markArrow;

        [Header("是否产生鬼影(拖尾)")]
        public bool _creatGhostShadow;

        [Header("血条是否被遮住,需要向上移动")]
        public bool _hpVerticalMove;

        public GameObject Owner{get;set;}

        private void Awake()
        {
            initialFeedbacks?.Initialization();
            hideFeedbacks?.Initialization();

            _markArrow = GetComponent<MarkArrow>();
        }

        private void OnEnable()
        {
            ActiveChilds();

            startTime = Time.time;
            GameManager.Instance.PausedGameEvent += PausedGameEvent;
            Invoke("Hide", time);

            initialFeedbacks?.PlayFeedbacks();

            ///发送血条向上移动
            if (_hpVerticalMove)
            {
                if (EventTypeManager.ContainHTEventType(HTEventType.HPVerticalMove))
                {
                    EventTypeManager.Broadcast(HTEventType.HPVerticalMove, true);
                }
            }

            if (!HideGreatSwordMan) return;
            if(EventTypeManager.ContainHTEventType(HTEventType.HideGreatPlayer))
            {
                EventTypeManager.Broadcast(HTEventType.HideGreatPlayer,false);
            }

            //产生鬼隐
            if(_creatGhostShadow)
            {
                InvokeRepeating("CreatGhostShadow", 0, 0.02f);
            }

         
        }

        private void Start()
        {
            _instance = this;
        }

        public void Hide()
        {
            if(_markArrow!=null&&_markArrow.Process)
            {
                return;
            }

            hideFeedbacks?.PlayFeedbacks();

            if (canDestroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }

            if(!string.IsNullOrEmpty(DisableCallBackObjcet))
            {
                GameObject callBackObject=  PoolManagerV2.Instance.GetInst(DisableCallBackObjcet);
                callBackObject.transform.position = this.transform.position;
                callBackObject.transform.rotation = this.transform.rotation;
            }

            //血条还原
            if(_hpVerticalMove)
            {
                if(EventTypeManager.ContainHTEventType(HTEventType.HPVerticalMove))
                {
                    EventTypeManager.Broadcast(HTEventType.HPVerticalMove, false);
                }
            }

            if (!HideGreatSwordMan) return;
            if (EventTypeManager.ContainHTEventType(HTEventType.HideGreatPlayer))
            {
                EventTypeManager.Broadcast(HTEventType.HideGreatPlayer, true);
            }
        }

        private void OnDisable()
        {
            if(GameManager.Instance!=null)
            {
                GameManager.Instance.PausedGameEvent -= PausedGameEvent;
            }

            CancelInvoke("Hide");
            CancelInvoke("CreatGhostShadow");
        }

        private void PausedGameEvent(bool pause)
        {

            if (pause)
            {
                lifeTime= Time.time-startTime;
                CancelInvoke("Hide");
            }
            else
            {
                Invoke("Hide", lifeTime);
            }
        }

        /// <summary>
        /// 激活所有子物体
        /// </summary>
        private void ActiveChilds()
        {
            if (!activeChilds) return;
            for(int i=0;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 得到剩余时间
        /// </summary>
        /// <returns></returns>
        public float GetLifeTimePercent()
        {
            lifeTime = Time.time - startTime;
            return 1-(lifeTime / time);
        }

        /// <summary>
        /// 创建鬼隐
        /// </summary>
        void CreatGhostShadow()
        {
            if(EventTypeManager.ContainHTEventType(HTEventType.CreatGhostShadow))
            {
                EventTypeManager.Broadcast(HTEventType.CreatGhostShadow);
            }
        }

    }

}
