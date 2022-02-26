using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 检测目标是否死亡
    /// </summary>
    public class AIDecisionTarget : AIDecision
    {
        Health _health;
        protected override void Start()
        {
            base.Start();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
           if(_brain.Target!=null)
            {
                _health = _brain.Target.gameObject.GetComponent<Health>();
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }


       
        public override bool Decide()
        {
            if(_brain.Target==null)
            {
                return true;
            }

            if(_health==null)
            {
                return true;
            }

            if(!(_health.gameObject.activeInHierarchy)||_health.CurrentHealth<=0)
            {
                _brain.Target = null;
                return true;
            }

            return false;
           
        }
    }

}
