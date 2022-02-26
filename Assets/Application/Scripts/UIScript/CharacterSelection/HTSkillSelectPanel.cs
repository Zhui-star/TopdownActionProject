using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace HTLibrary.Application
{
    /// <summary>
    /// 技能选择面板
    /// </summary>
    public class HTSkillSelectPanel : BasePanel
    {
        public float animSecond = 0.5f;
        public Transform grid;
        public GameObject skillUnitUI;

        public override void OnEnter()
        {

            base.OnEnter();
            InitialPanel();
            transform.DOScale(Vector3.one, animSecond).OnComplete(() =>
            {

            }).SetUpdate(true);
        }

        public override void OnExit()
        {
            base.OnExit();

            bool pushWeaponPanel = false;
            CharacterIdentity characterType = CharacterManager.Instance.GetCharacter("Player1")
            .GetComponent<CharacterIdentity>();

            if (HTDBManager.Instance.SelectHeroWeapon(characterType.heroType).Count <= 0)
            {

            }
            else if (HTDBManager.Instance.NeedWeaponEuip())
            {
                UIManager.Instance.PushPanel(UIPanelType.WeaponSwitchTipsPanel);
                pushWeaponPanel = true;
            }
            else
            {

                if (!HTDBManager.Instance.CheckCurrentIsBestWeapon(characterType.heroType))
                {
                    UIManager.Instance.PushPanel(UIPanelType.WeaponSwitchTipsPanel);
                    pushWeaponPanel = true;
                }
            }
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
               {
                   if (!pushWeaponPanel)
                   {
                       HTPauseGame.Instance.UnPauseGame();
   
                   }

               ;
               }).SetUpdate(true);
        }

        /// <summary>
        /// 初始化UI面板
        /// </summary>
        void InitialPanel()
        {
            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }

            if (CharacterSelection.Instance.ReturnCurrentSkill().Count > 3)
            {
                foreach (var temp in GetRandomId())
                {
                    GameObject go = GameObject.Instantiate(skillUnitUI, grid);
                    HTSelectUnitUI selectUnitUI = go.GetComponent<HTSelectUnitUI>();
                    selectUnitUI.UpdateUI(SkillSystemManager.Instance.GetSkillById(temp));
                }
            }
            else
            {
                foreach (var temp in CharacterSelection.Instance.ReturnCurrentSkill())
                {

                    GameObject go = GameObject.Instantiate(skillUnitUI, grid);
                    HTSelectUnitUI selectUnitUI = go.GetComponent<HTSelectUnitUI>();
                    selectUnitUI.UpdateUI(SkillSystemManager.Instance.GetSkillById(temp));
                }
            }

        }

        //获取随机的技能 不超过3个
        public List<int> GetRandomId()
        {
            System.Random randomIndex = new System.Random();
            HashSet<int> map = new HashSet<int>();
            List<int> finalSkillID = new List<int>();
            List<int> currentSkillID = CharacterSelection.Instance.ReturnCurrentSkill();
            int index = 0;

            while (index < 3)
            {
                int currenIndex = randomIndex.Next(0, CharacterSelection.Instance.ReturnCurrentSkill().Count);
                if (!map.Contains(currenIndex))
                {
                    map.Add(currenIndex);
                    finalSkillID.Add(currentSkillID[currenIndex]);
                    index++;
                }
                else
                    currenIndex = randomIndex.Next(0, CharacterSelection.Instance.ReturnCurrentSkill().Count);
            }
            return finalSkillID;
        }
    }

}
