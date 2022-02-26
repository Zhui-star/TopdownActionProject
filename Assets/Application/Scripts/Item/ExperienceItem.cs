using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    /// <summary>
    /// 经验块
    /// </summary>
    public class ExperienceItem : MonoBehaviour
    {
        [Header("能够获得经验")]
        public float Experience;


        public Health health;
        private CharacterXP xp;

        public ItemSport itemSport;

        public void Start()
        {

            if(health!=null)
            {
                health.OnDeath += PickExperienceItem;
            }

            if(itemSport!=null)
            {
                itemSport.triggerEvent += PickExperienceItem;
            }

        }

        public void OnDestroy()
        {
            if(health!=null&&health.OnDeath!=null)
            {
                health.OnDeath -= PickExperienceItem;
            }

            if (itemSport != null)
            {
                itemSport.triggerEvent -= PickExperienceItem;
            }

        }

        /// <summary>
        /// 捡到经验啦
        /// </summary>
        void PickExperienceItem()
        {
            if(xp==null)
            {
                xp = CharacterManager.Instance.GetCharacter("Player1").gameObject.GetComponent<CharacterXP>();
            }

            xp.AddExperience(Experience);

        }
    }

}
