using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    public class HpSugarItem : MonoBehaviour
    {
        [Header("能够获得的血量")]
        public int HpVolume = 5;
        public Health health;
        public ItemSport itemSport;

        public void OnEnable()
        {
            if (itemSport != null)
            {
                itemSport.triggerEvent += PickHpSugarItem;
            }
        }

        public void OnDisable()
        {
            if(itemSport != null)
            {
                itemSport.triggerEvent -= PickHpSugarItem;
            }
        }

        private void PickHpSugarItem()
        {
            if(health==null)
            {
                GameObject playerGo = CharacterManager.Instance.GetCharacter("Player1").gameObject;
                health = playerGo.GetComponent<Health>();       
            }

            health.CurrentHealth += HpVolume;
        }
    }
}
