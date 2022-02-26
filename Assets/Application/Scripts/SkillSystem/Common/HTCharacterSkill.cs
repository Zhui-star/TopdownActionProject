using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class HTCharacterSkill : MonoBehaviour
    {
        public List<int> learnSkills;
        private SkillReleaseTrigger skillReleaseTrigger;
        public bool TestMode = false;
        private void Awake()
        {
            skillReleaseTrigger = GetComponent<SkillReleaseTrigger>();

            if(TestMode)
            {
                if (learnSkills.Count <= 0)
                {

                    foreach (var temp in learnSkills)
                    {
                        LearnSkill(temp);
                    }
                }

                AssignSkill();
            }
          
        }

        public void LearnSkill(int skillId)
        {
            if(learnSkills.Contains(skillId))
            {
                //Debug.LogError("技能:" + skillId + ":已经学会");
                return;
            }

            learnSkills.Add(skillId);

           
        }

        

        public void AssignSkill()
        {
            if(skillReleaseTrigger==null)
            {
                skillReleaseTrigger = GetComponent<SkillReleaseTrigger>();
            }

            for(int i=0;i<learnSkills.Count;i++)
            {
                AddToSkillSetting(learnSkills[i], i);               
            }
        }

        /// <summary>
        /// 得到技能空草
        /// </summary>
        /// <returns></returns>
        //被动技能学习
        int GetEmptySkillSlot()
        {

            
            foreach(var skill in skillReleaseTrigger.skillSetting)
            {
                if(skill.skillID==0)
                {
                    if(skillReleaseTrigger.skillSetting.IndexOf(skill)<=1)
                    {
                        continue;
                    }

                    return skillReleaseTrigger.skillSetting.IndexOf(skill);
                }
            }
            return -1;
        }

        /// <summary>
        /// 学习被动技能
        /// </summary>
        /// <param name="skillId"></param>
        public int LearnPassiveSkill(int skillId)
        {
            int i = GetEmptySkillSlot();
            if(i==-1)
            {
                return -1;
            }

            AddToSkillSetting(skillId,i);
            return i;
        }

        /// <summary>
        /// 添加技能进入技能曹位置
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="i"></param>
        void AddToSkillSetting(int skillId,int i)
        {

            skillReleaseTrigger.skillSetting[i].skillName = SkillSystemManager.Instance.GetSkillById(skillId).skillName;
            skillReleaseTrigger.skillSetting[i].skillID = SkillSystemManager.Instance.GetSkillById(skillId).skillID;
            skillReleaseTrigger.skillSetting[i].skillIcon = SkillSystemManager.Instance.GetSkillById(skillId).skillIcon;
            skillReleaseTrigger.skillSetting[i].skillPrefab = SkillSystemManager.Instance.GetSkillById(skillId).skillPrefab;
            skillReleaseTrigger.skillSetting[i].castEffect = SkillSystemManager.Instance.GetSkillById(skillId).castEffect;
            skillReleaseTrigger.skillSetting[i].castTime = SkillSystemManager.Instance.GetSkillById(skillId).castTime;
            skillReleaseTrigger.skillSetting[i].skillDelay = SkillSystemManager.Instance.GetSkillById(skillId).skillDelay;
            skillReleaseTrigger.skillSetting[i].skillDescription = SkillSystemManager.Instance.GetSkillById(skillId).skillDescription;
            skillReleaseTrigger.skillSetting[i].skillSpawn = SkillSystemManager.Instance.GetSkillById(skillId).skillSpawn;
            skillReleaseTrigger.skillSetting[i].whileAttack = SkillSystemManager.Instance.GetSkillById(skillId).whileAttack;
            skillReleaseTrigger.skillSetting[i].soundEffect = SkillSystemManager.Instance.GetSkillById(skillId).soundEffect;
            skillReleaseTrigger.skillSetting[i].animationName = SkillSystemManager.Instance.GetSkillById(skillId).animationName;
            skillReleaseTrigger.skillSetting[i].cooldown = SkillSystemManager.Instance.GetSkillById(skillId).cooldown;
            skillReleaseTrigger.skillSetting[i].changePosition = SkillSystemManager.Instance.GetSkillById(skillId).changePosition;
            skillReleaseTrigger.skillSetting[i].changeValue = SkillSystemManager.Instance.GetSkillById(skillId).changeValue;
            skillReleaseTrigger.skillSetting[i].changeTimer = SkillSystemManager.Instance.GetSkillById(skillId).changeTimer;
            skillReleaseTrigger.skillSetting[i].skillTime = SkillSystemManager.Instance.GetSkillById(skillId).skillTime;
            skillReleaseTrigger.skillSetting[i].preventThrough = SkillSystemManager.Instance.GetSkillById(skillId).prventThrough;
            skillReleaseTrigger.skillSetting[i].layerMask = SkillSystemManager.Instance.GetSkillById(skillId).layerMask;
            skillReleaseTrigger.skillSetting[i].permitDash = SkillSystemManager.Instance.GetSkillById(skillId).permitDash;
            skillReleaseTrigger.skillSetting[i].skillPositionOffset = SkillSystemManager.Instance.GetSkillById(skillId).skillPositionOffset;
            skillReleaseTrigger.skillSetting[i].IsLoop = SkillSystemManager.Instance.GetSkillById(skillId).IsLoop;
            skillReleaseTrigger.skillSetting[i].CanJump = SkillSystemManager.Instance.GetSkillById(skillId).CanJump;
            skillReleaseTrigger.skillSetting[i].JumpForce = SkillSystemManager.Instance.GetSkillById(skillId).JumpForce;
            skillReleaseTrigger.skillSetting[i].CanReduceCoolDown = SkillSystemManager.Instance.GetSkillById(skillId).CanReduceSkillCoolDown;
            skillReleaseTrigger.skillSetting[i].castSoundEffect = SkillSystemManager.Instance.GetSkillById(skillId).castSoundEffect;
            skillReleaseTrigger.skillSetting[i].castEffectOffset = SkillSystemManager.Instance.GetSkillById(skillId).CastEffctOffset;
            skillReleaseTrigger.skillSetting[i].BeforeReleseSkillTime = SkillSystemManager.Instance.GetSkillById(skillId).BeforeReleseSkillTime;
            skillReleaseTrigger.skillSetting[i].JumpTime = SkillSystemManager.Instance.GetSkillById(skillId).JumpTime;
            skillReleaseTrigger.skillSetting[i].JumpZSpeed = SkillSystemManager.Instance.GetSkillById(skillId).JumpZSpeed;

            skillReleaseTrigger.skillSetting[i].Fly = SkillSystemManager.Instance.GetSkillById(skillId).Fly;
            skillReleaseTrigger.skillSetting[i].TargetHeight = SkillSystemManager.Instance.GetSkillById(skillId).TargetHeight;
            skillReleaseTrigger.skillSetting[i].UpSpeed = SkillSystemManager.Instance.GetSkillById(skillId).UpSpeed;
            skillReleaseTrigger.skillSetting[i].DownSpeed = SkillSystemManager.Instance.GetSkillById(skillId).DownSpeed;
            skillReleaseTrigger.skillSetting[i].FlyDuration = SkillSystemManager.Instance.GetSkillById(skillId).FlyDuration;
            skillReleaseTrigger.skillSetting[i].DownDecelerationMultiple = SkillSystemManager.Instance.GetSkillById(skillId).DownDeclerationMultiple;
            skillReleaseTrigger.skillSetting[i].FollowTargetSpeed = SkillSystemManager.Instance.GetSkillById(skillId).FollowTargetSpeed;
            skillReleaseTrigger.skillSetting[i].UpDeclerationMultiple = SkillSystemManager.Instance.GetSkillById(skillId).UpDeclerationMultiple;

            skillReleaseTrigger.skillSetting[i].StartFrezzeAnim = SkillSystemManager.Instance.GetSkillById(skillId).StartFrezzeAnim;
            skillReleaseTrigger.skillSetting[i].FrezzeAnimDuration = SkillSystemManager.Instance.GetSkillById(skillId).FrezzeAnimDuration;

            skillReleaseTrigger.skillSetting[i].WaitSkillTrigger = SkillSystemManager.Instance.GetSkillById(skillId).WaitSkillTrigger;

            skillReleaseTrigger.skillSetting[i]._playableAssetKey=SkillSystemManager.Instance.GetSkillById(skillId)._playableAssetKey;

            skillReleaseTrigger.skillSetting[i]._cantCoolDown=SkillSystemManager.Instance.GetSkillById(skillId)._cantCoolDown;
        }
    }

}
