using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// AI行为计数
    /// </summary>
    public class AIDecisionCount : AIDecision
    {
        private int count;
        public int targetCount = 5;


        public override bool Decide()
        {
          return  EvaluateCount();
        }

        bool EvaluateCount()
        {
            if(count>=targetCount)
            {
                count = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 计数器++
        /// </summary>
        public void AddCount()
        {
            
            count++;
        }
    }

}
