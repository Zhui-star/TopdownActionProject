using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class AIClearController : MonoBehaviour
    {
        /// <summary>
        /// 清空所有敌人
        /// </summary>
        public void ClearAI()
        {
            GameObject[] AIs = GameObject.FindGameObjectsWithTag(Tags.Enemies);

            foreach(var enemy in AIs)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                if(enemyHealth!=null)
                {
                    enemyHealth.Damage(99999, this.gameObject, 0.25f, 0,null);
                }
            }
        }
    }

}
