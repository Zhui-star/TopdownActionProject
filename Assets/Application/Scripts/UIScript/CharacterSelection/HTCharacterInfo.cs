using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.UI;

namespace HTLibrary.Application
{
    public class HTCharacterInfo : MonoBehaviour
    {
        public Text hpTxt;
        public Text attackTxt;
        public Text defenceTxt;
        public Text dodgeTxt;
        public Text critTxt;

        public CharacterConfig characterConfigure;

        private void Start()
        {
            UpdateInfo(characterConfigure.characterHP+characterConfigure.additiveHP, 
                characterConfigure.characterAttack+characterConfigure.additiveAttack
                , characterConfigure.characterDefence+characterConfigure.additiveDefence, 
                characterConfigure.characterCritRank+characterConfigure.additiveCritRank, 
                characterConfigure.characterDodge+characterConfigure.additiveDodge
                );    
        }

        private void OnEnable()
        {
            characterConfigure.UpdatePropertyEvent += UpdateInfo;
        }

        private void OnDisable()
        {
            characterConfigure.UpdatePropertyEvent -= UpdateInfo;
        }

        /// <summary>
        /// 更新人物面板属性信息
        /// </summary>
        /// <param name="hp">人物血量</param>
        /// <param name="attack">人物攻击</param>
        /// <param name="denfecne">人物防御</param>
        /// <param name="crit">人物暴击率</param>
        /// <param name="dodge">人物闪避率</param>
        void UpdateInfo(float hp,float attack,float denfecne, float crit, float dodge)
        {
            hpTxt.text = hp.ToString();
            attackTxt.text = attack.ToString();
            defenceTxt.text = denfecne.ToString();

            string dodgeStr=dodge>0? (dodge * 100).ToString("00") + "%":"0%";
            dodgeTxt.text = dodgeStr;

            string critStr = crit > 0 ? (crit * 100).ToString("00") + "%" : "0%";
            critTxt.text = critStr;
        }


    }

}
