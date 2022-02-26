using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    [System.Serializable]
    public class SkillSetting
    {
        public string skillName;

        public int skillID;

        public string skillDescription;

        public SkillSpawn skillSpawn;

        public WhileAttack whileAttack;

        public string skillPrefab;

        public AudioClip castSoundEffect;

        public AudioClip soundEffect;

        public string animationName;

        public bool IsLoop;

        public float skillDelay;

        public string castEffect;

        public float castTime;

        public float cooldown;

        public Sprite skillIcon;

        public float skillTime;

        public ChangePosition changePosition;

        public float changeValue;

        public float changeTimer;

        public bool preventThrough;

        public LayerMask layerMask;

        public bool permitDash;

        public Vector3 skillPositionOffset;


        public bool CanJump;

        public Vector3 JumpDirection;

        public float JumpForce;

        
        public bool CanReduceCoolDown;

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
        public float DownDecelerationMultiple;
        public float FollowTargetSpeed;
        public float UpDeclerationMultiple;

        [Header("等待技能释放")]
        public bool WaitSkillTrigger = false;

        [Header("初始化执行前特效的位置偏移")]
        public Vector3 castEffectOffset;

        public float BeforeReleseSkillTime = 0;

        //Timeline playable asset key
        public string _playableAssetKey;

        [HideInInspector]
        public bool _cantCoolDown=false;
    }

}
