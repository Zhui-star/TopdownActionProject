using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
 namespace HTLibrary.Application
{
    /// <summary>
    /// 闪避率逻辑
    /// </summary>
    public class Dodge:MonoBehaviourSimplify
    {
        public CharacterConfig characterConfigure;
        public float additiveDodge;
        public bool SuccessDodge()
        {
          return  MathUtility.Percent((int)((characterConfigure.additiveDodge+characterConfigure.characterDodge+additiveDodge) * 100));
        }

        protected override void OnBeforeDestroy()
        {
            
        }



      
    }

}
