using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HTLibrary.Application
{
    public class HTXPBar : MonoBehaviour
    {
        private Image expFillImg;
        private CharacterXP characterExp;

        private void Start()
        {
            characterExp = CharacterManager.Instance.GetCharacter("Player1").GetComponent<CharacterXP>();
            expFillImg = transform.Find("Exp_Img").GetComponent<Image>();
            expFillImg.fillAmount = characterExp.GetExperiencePercent();
            characterExp.ExperienceUpdateEvent += GetExpForCharacter;
        }

        private void OnDestroy()
        {
            characterExp.ExperienceUpdateEvent -= GetExpForCharacter;
        }

        private void GetExpForCharacter(float getExp)
        {
            expFillImg.fillAmount = characterExp.GetExperiencePercent();
        }

        //test for exp 
        public void OnClickGetExp()
        {
            characterExp.AddExperience(3.0f);
        }
    }
}
