using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using NaughtyAttributes;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    /// <summary>
    /// 初始化设置目标
    /// </summary>
    public class AIActionSetTarget :AIAction
    {
        [ReadOnly]
        public Transform target;
        CharacterOrientation3D oritentation3D;

        public override void PerformAction()
        {
            if(target==null)
            {
                target = GameObject.FindGameObjectWithTag(Tags.Player).transform;
            }

            if (_brain==null|| target == null) return;
            _brain.Target = target;
        }

        // Start is called before the first frame update
   
    }
}

