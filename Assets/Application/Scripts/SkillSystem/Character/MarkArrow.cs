using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Feedbacks;
using DG.Tweening;
namespace HTLibrary.Application
{
    /// <summary>
    /// 标记箭 用于瞬间移动
    /// </summary>
    public class MarkArrow : MonoBehaviourSimplify
    {
        [MMFReadOnly]
        public Transform _Owner;
        public MMFeedbacks _releseTriggerFeedBacks;
        public string _feedBackName;
        int _index;

       public bool Process { get; private set; }

        private void Awake()
        {
            _releseTriggerFeedBacks?.Initialization();    
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener(HTEventType.MarkArrow, ReleseTrigger);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener(HTEventType.MarkArrow, ReleseTrigger);

            EndWaitSkill();
        }

        /// <summary>
        /// 触发标记效果
        /// </summary>
        void ReleseTrigger()
        {
            StartCoroutine(IReleseTrigger());
        }

        IEnumerator IReleseTrigger()
        {
            if (_Owner != null)
            {
                Process = true;
                Vector3 targerPosition = Vector3.zero;
                targerPosition.y = _Owner.position.y;
                targerPosition = new Vector3(transform.position.x, targerPosition.y, transform.position.z);

                _Owner.transform.DOScale(0, 0.2f);

                if (!string.IsNullOrEmpty(_feedBackName))
                {
                    GameObject feedBack = PoolManagerV2.Instance.GetInst(_feedBackName);
                    feedBack.transform.position = _Owner.transform.position+Vector3.up*1.5f;
                }

                yield return new WaitForSeconds(0.1f);


                _Owner.transform.position = targerPosition;

             
                yield return new WaitForSeconds(0.1f);

                _Owner.transform.DOScale(1, 0.2f);
                _releseTriggerFeedBacks?.PlayFeedbacks();
              
                this.gameObject.SetActive(false);
                Process = false;
            }
        }

        /// <summary>
        /// 结算技能释放
        /// </summary>
        void EndWaitSkill()
        {
            SkillReleaseTrigger skillReleseTrigger = _Owner.GetComponent<SkillReleaseTrigger>();
            if(skillReleseTrigger!=null)
            {
                skillReleseTrigger.ReleseWaitSkill(_index);
            }

            
        }

        public void SetOwner(Transform owner, int skillIndex)
        {
            _Owner = owner;
            _index = skillIndex;
        }

        protected override void OnBeforeDestroy()
        {
            
        }
    }

}
