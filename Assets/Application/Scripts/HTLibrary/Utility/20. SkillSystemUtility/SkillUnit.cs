using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 技能属性
    /// </summary>
    [Serializable]
    public class SkillUnit
    {
        public string skillName;

        public int skillID;

        public string skillDescription;
        [HideInInspector]
        public SkillType skillType;
        [HideInInspector]
        public SkillProperty skillProperty;

        public SkillSpawn skillSpawn;
        public WhileAttack whileAttack;

        public Sprite skillIcon;

        public string animationName;
        public bool IsLoop;

        public bool permitDash;


        [Header("冷却")]
        public float cooldown;
        public bool CanReduceSkillCoolDown;

        [Header("Audio")]
        public AudioClip castSoundEffect;
        public AudioClip soundEffect;

        [Header("读条")]
        public string castEffect;
        public float castTime;
        public Vector3 CastEffctOffset;

        [Header("技能")]

        public string skillPrefab;
        public Vector3 skillPositionOffset;
        public float BeforeReleseSkillTime = 0;
        public float skillDelay;
        public float skillTime;

        [Header("冲刺")]
        public ChangePosition changePosition;
        public float changeValue;
        public float changeTimer;
        public bool prventThrough;
        public LayerMask layerMask;

        [Header("跳跃")]
        public bool CanJump;
        public float JumpForce;
        public float JumpZSpeed;
        public float JumpTime;
        public float StartFrezzeAnim = 0;
        public float FrezzeAnimDuration = 0;

        [Header("起飞")]
        public bool Fly;
        public float TargetHeight;
        public float UpSpeed;
        public float DownSpeed;
        public float FlyDuration;
        public float DownDeclerationMultiple;
        public float FollowTargetSpeed;
        public float UpDeclerationMultiple;

        [Header("技能挂起")]
        public bool WaitSkillTrigger = false;

        [Header("Timeline")]
        public string _playableAssetKey;

        //"If it is true, when skill can't enter cd"
        [HideInInspector]
        public bool _cantCoolDown=false;

    }

}
