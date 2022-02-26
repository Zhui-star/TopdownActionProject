using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 天赋显示的UI
    /// </summary>
    public class TalentItemUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler,IPointerExitHandler
    {
        public int itemID;

        public Image talentImg;

        public GameObject unlockMask;

        public GameObject enterImg;

        public Text levelTxt;

        public ParticleSystem studyParticle;

        [Header("交互显示")]
        public Color _enterColor;
        public Color _selectColor;
        private Image _enterImg;

        void Awake()
        {
            _enterImg = enterImg.GetComponent<Image>();
        }

        void Start()
        {
            UpdateUI();

            TalentSystemManager.Instance.UpdateStudyUIEvent += UpdateUI;
        }

        void OnDestroy()
        {
            TalentSystemManager.Instance.UpdateStudyUIEvent -= UpdateUI;
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        public void UpdateUI()
        {
            TalentItem item = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(itemID);

            talentImg.sprite = item.TalentIcon;

            if (TalentSystemManager.Instance.IsLearnTalent(itemID))
            {
                unlockMask.SetActive(false);

                int level = TalentSystemManager.Instance.ReturnTalentLevel(itemID);

                talentImg.color = Color.white;

                ShowLevelTxt(level,item);
            }
            else if (TalentSystemManager.Instance.IsUnlockTalent(itemID))
            {
                talentImg.color = Color.grey;
                unlockMask.SetActive(false);

                ShowLevelTxt(0,item);
            }
            else
            {
                unlockMask.SetActive(true);
                talentImg.color = Color.red;

                ShowLevelTxt(0,item);
            }
        }

        /// <summary>
        /// 显示天赋文本
        /// </summary>
        /// <param name="level"></param>
        void ShowLevelTxt(int level,TalentItem item)
        {
            levelTxt.text = (level +"/" +item.talenCosts.Count).ToString();
        }

   
        public void OnPointerDown(PointerEventData eventData)
        {
            //TODO 反馈

            if(!TalentSystemManager.Instance.SelectTalent)
            {
                TalentSystemManager.Instance.SelectTalent = true;
                _enterImg.color = _selectColor;
            }
            else
            {
                TalentItem talentItem = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(itemID);
                if(TalentSystemManager.Instance.Pickeditem==talentItem)
                {
                    TalentSystemManager.Instance.SelectTalent = false;
                    //enterImg.SetActive(false);
                    _enterImg.color = _enterColor;
                }
                else
                {
                    _enterImg.color = _selectColor;
                    SelectTalentUI();                 
                }
            }

           
        }

        /// <summary>
        /// 显示该天赋信息
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TalentSystemManager.Instance.SelectTalent) return;
            _enterImg.color = _enterColor;
            SelectTalentUI();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        /// <summary>
        /// 选中天赋
        /// </summary>
        void SelectTalentUI()
        {

            if (TalentSystemManager.Instance.enterImg != null)
            {
                TalentSystemManager.Instance.enterImg.SetActive(false);
            }

            if (TalentSystemManager.Instance.studyfeedback != null)
            {
                TalentSystemManager.Instance.studyfeedback = null;
            }

            TalentItem talentItem = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(itemID);
            TalentSystemManager.Instance.Pickeditem = talentItem;
            enterImg.SetActive(true);
            TalentSystemManager.Instance.enterImg = enterImg;

            if (studyParticle != null)
            {
                TalentSystemManager.Instance.studyfeedback = studyParticle;
            }
        }
    }

}
