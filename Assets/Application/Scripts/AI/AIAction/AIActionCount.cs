using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 使AIDecisionCount ++
    /// </summary>
    public class AIActionCount : AIAction
    {
        public AIDecisionCount _count;

        protected override void Start()
        {
            base.Start();
            if(_count==null)
            {
                _count = GetComponent<AIDecisionCount>();
            }        
        }
        public override void OnEnterState()
        {
            base.OnEnterState();

            if(_count!=null)
            {
                _count.AddCount();
            }
           
        }

        public override void PerformAction()
        {
          
        }
    }

}
