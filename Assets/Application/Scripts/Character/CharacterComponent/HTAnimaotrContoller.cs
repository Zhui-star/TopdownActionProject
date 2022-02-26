using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.Animations;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 动画攻速控制器
    /// </summary>
    public class HTAnimaotrContoller : MonoBehaviour
    {
        private Animator animatorController;

        public int animIndex;

        public List<string> animationClipName;

        private HTAttackSpeed attackSpeed;
        
        private void Start()
        {
            attackSpeed = GetComponent<HTAttackSpeed>();
            animatorController = GetComponent<Weapon>().Owner.CharacterModel.GetComponent<Animator>();
        }

        bool IsAttackAnim = false;
        private void FixedUpdate()
        {
            if (animatorController == null) return;
            IsAttackAnim = false;
            AnimatorStateInfo animInfo = animatorController.GetCurrentAnimatorStateInfo(animIndex);
            foreach(var temp in animationClipName)
            {
                if(animInfo.IsName(temp))
                {
                    IsAttackAnim = true;
                    break;
                }
            }

            if(IsAttackAnim)
            {
                animatorController.speed = 1 /attackSpeed.AnimSpeedPercent();
            }
            else
            {
                animatorController.speed = 1;
            }

        }
    }
}

