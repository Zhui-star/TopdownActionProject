using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 元素法师被动技能 每隔NS 释放元素地刺
    /// </summary>
    public class ElementMagePassiveSkill : MonoBehaviourSimplify
    {
        public string _passiveSkillObj;
        public float _internalTime = 5.0f;
        public float _radius = 40;
        public float _skillCount = 3;
        private PoolManagerV2 _poolManager;

        private void Awake()
        {
            _poolManager = PoolManagerV2.Instance;
        }

        private void Start()
        {
            InvokeRepeating("TriggerPassiveSkill", 1, _internalTime);    
        }

        /// <summary>
        /// 触发被动技能
        /// </summary>
        void TriggerPassiveSkill()
        {
            for(int i=0;i<_skillCount;i++)
            {
                GameObject skillObj = _poolManager.GetInst(_passiveSkillObj);
                Vector3 targerPostion = Vector3.zero;

                float x = Random.Range(-_radius, _radius);
                float z = Random.Range(-_radius, _radius);

                targerPostion = new Vector3(x + transform.position.x,transform.position.y, z + transform.position.z);

                skillObj.transform.position = targerPostion;
            }
           
        }

        protected override void OnBeforeDestroy()
        {
          
        }
    }

}
