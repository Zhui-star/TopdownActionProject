using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using UnityEngine.UI;

namespace HTLibrary.Application
{
    /// <summary>
    /// 技能快捷栏
    /// </summary>
    public class SkillBox : MonoBehaviour
    {
        [Header("快捷栏类型")]
        public SkillBoxType skillBoxType;
        private CharacterDash3D dash3D;
        private SkillReleaseTrigger skillRelease;
        public Image cdUI;
        public Image skillImage;
        public Image _waitSkillCD;

        public GameObject _waitEnterCooldownGo;
        private void Start()
        {
            ImplementSkillBox(true);
        }

        private void OnDestroy()
        {
            if (dash3D == null)
            {
                return;
            }

            ImplementSkillBox(false);

        }


        public void ImplementSkillBox(bool add)
        {
            switch (skillBoxType)
            {
                case SkillBoxType.Dodge:
                    if (dash3D == null)
                    {
                        dash3D = CharacterManager.Instance.GetCharacter("Player1").GetComponent<CharacterDash3D>();
                    }

                    if (dash3D != null)
                    {
                        if (add)
                        {
                            dash3D.MonitorCDEvent += UpdateCDUI;
                        }
                        else
                        {
                            dash3D.MonitorCDEvent -= UpdateCDUI;
                            dash3D = null;
                        }
                    }
                    break;
                case SkillBoxType.Skill1:

                    if (skillRelease == null)
                    {
                        skillRelease = CharacterManager.Instance.GetCharacter("Player1").GetComponent<SkillReleaseTrigger>();
                    }

                    if (add)
                    {
                        if (skillRelease.skillSetting[0].skillIcon != null)
                        {
                            skillImage.sprite = skillRelease.skillSetting[0].skillIcon;
                            skillImage.color = Color.white;
                        }

                        skillRelease.Skill01MonitorEvent += Skill01CDUI;
                    }
                    else
                    {
                        skillRelease.Skill01MonitorEvent -= Skill01CDUI;

                    }

                    break;

                case SkillBoxType.Skill2:

                    if (skillRelease == null)
                    {
                        skillRelease = CharacterManager.Instance.GetCharacter("Player1").GetComponent<SkillReleaseTrigger>();
                    }

                    if (add)
                    {
                        if (skillRelease.skillSetting[1].skillIcon != null)
                        {
                            skillImage.sprite = skillRelease.skillSetting[1].skillIcon;
                            skillImage.color = Color.white;
                        }

                        skillRelease.Skill02MonitorEvent += Skill02CDUI;
                    }
                    else
                    {
                        skillRelease.Skill02MonitorEvent -= Skill02CDUI;
                        skillRelease = null;
                    }

                    break;
            }
        }

        /// <summary>
        /// 更新CD UI
        /// </summary>
        /// <param name="percent"></param>
        private void UpdateCDUI(float percent)
        {
            cdUI.fillAmount = percent;
        }

        /// <summary>
        /// 更新技能01 事件
        /// </summary>
        private void Skill01CDUI()
        {
            if (skillRelease.GetCantCoolDown(0))
            {
                if (_waitEnterCooldownGo)
                    _waitEnterCooldownGo.SetActive(true);
            }
            else
            {
                _waitEnterCooldownGo.SetActive(false);
            }

            cdUI.fillAmount = skillRelease.SkillCoolDownPercent(0);

            float percent = skillRelease.GetWaitSkillTimePercent(0);

            _waitSkillCD.gameObject.SetActive(percent > 0);

            _waitSkillCD.fillAmount = percent;


        }

        /// <summary>
        /// 更新技能 02 事件
        /// </summary>
        private void Skill02CDUI()
        {
            if (skillRelease.GetCantCoolDown(1))
            {
                if (_waitEnterCooldownGo)
                    _waitEnterCooldownGo.SetActive(true);
            }
            else
            {
                _waitEnterCooldownGo.SetActive(false);
            }

            cdUI.fillAmount = skillRelease.SkillCoolDownPercent(1);

            float percent = skillRelease.GetWaitSkillTimePercent(1);

            _waitSkillCD.gameObject.SetActive(percent > 0);

            _waitSkillCD.fillAmount = percent;
        }
    }

}
