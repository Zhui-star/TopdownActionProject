using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using NaughtyAttributes;
using System.Collections;
namespace HTLibrary.Application
{
    /// <summary>
    /// 技能选择单位UI
    /// </summary>
    public class HTSelectUnitUI : MonoBehaviour
    {
        [ReadOnly]
        public SkillUnit skillUnit;

        [Header("UI配置")]
        public Text skillNameTxt;

        public Image skillIcon;

        public Text skillDescription;

        [SerializeField]
        private ScrollRect descriptionScroll;

        [Space]
        [SerializeField]
        private float autoScrollAnimTime = 3.0f;
        private float autoScrollAnimTimer = 0;
        bool topward = false;
        [Space]
        public bool TestModel = true;

        //public Text SkillDescrption;
        public HTSkillSelectPanel hTSkillSelectPanel;



        private void Start()
        {
            hTSkillSelectPanel = GetComponentInParent<HTSkillSelectPanel>();


            AutoDescriptionScroll();
        }

        #region Auto scroll of decription text
        void AutoDescriptionScroll()
        {
            descriptionScroll.content.anchoredPosition = new Vector2(0, 0);
            StartCoroutine(ProcessAutoLoopScroll(descriptionScroll));
        }

        IEnumerator ProcessAutoLoopScroll(ScrollRect scrollRect)
        {
            while (true)
            {
                if (autoScrollAnimTimer>=autoScrollAnimTime)
                {
                    topward = !topward;
                    autoScrollAnimTimer = 0;
                }

                float currentVerticaPositionPercent = topward ? Mathf.SmoothStep(0, 1, autoScrollAnimTimer / autoScrollAnimTime) :
                        Mathf.SmoothStep(1, 0, autoScrollAnimTimer / autoScrollAnimTime);
         
                autoScrollAnimTimer += Time.unscaledDeltaTime;
                scrollRect.verticalNormalizedPosition = currentVerticaPositionPercent;


                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        public void UpdateUI(SkillUnit skillUnit)
        {

            this.skillUnit = skillUnit;
            skillNameTxt.text = skillUnit.skillName;
            skillDescription.text = skillUnit.skillDescription;

            if (TestModel)
            {

            }
            else
            {
                //TODO 对技能图标进行赋值
                skillIcon.sprite = skillUnit.skillIcon;
            }
        }


        /// <summary>
        /// 学习技能
        /// </summary>

        public void SelectSkillClick()
        {

            Character ch = CharacterManager.Instance.GetCharacter("Player1");
            HTCharacterSkill characterSkill = ch.GetComponent<HTCharacterSkill>();

            foreach (var temp in HTLevelManager.Instance.ReturnLearenedSkills())
            {
                characterSkill.LearnSkill(temp);
            }
            characterSkill.LearnSkill(skillUnit.skillID);
            characterSkill.AssignSkill();

            HTLevelManager.Instance.AddLearenSkill(skillUnit.skillID);
            SkillBoxManager.Instance.ImplementSkillBoxs(true);
            UIManager.Instance.PopPanel();
        }
    }

}
