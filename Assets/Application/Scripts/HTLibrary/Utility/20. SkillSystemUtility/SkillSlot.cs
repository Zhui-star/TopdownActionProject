using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace HTLibrary.Utility
{
    public class SkillSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        public Color normalColor;

        public Color highlightColor;

        public GameObject skillUIPrefab;

        private Image backGroundImage;
       
        private void Start()
        {
            backGroundImage = GetComponent<Image>();
        }

        public void StoreSkillUnit(SkillUnit skillUnit)
        {
            if(transform.childCount==0)
            {
                GameObject skillGo = Instantiate(skillUIPrefab) as GameObject;
                skillGo.transform.SetParent(this.transform);
                skillGo.transform.localScale = Vector3.one;
                skillGo.transform.localPosition = Vector3.zero;
                skillGo.GetComponent<SkillUnitUI>().SetSkillUnit(skillUnit);
            }
        }

        public SkillUnitUI GetSkillUnitUI()
        {
            return GetComponentInChildren<SkillUnitUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backGroundImage.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            backGroundImage.color = normalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           //TODO 选择该技能
        }
    }
}

