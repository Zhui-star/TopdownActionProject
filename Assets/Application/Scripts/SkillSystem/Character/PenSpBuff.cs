using System.Collections;
using UnityEngine;
using HTLibrary.Framework;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 风无行移动速度增益效果
    /// </summary>
    public enum AdditiveMoveSpeedMode
    {
        Number,
        Double
    }

    public class PenSpBuff : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;

        public float additiveMoveSpeed;
        public float additiveAttackSpeed;
        public AdditiveMoveSpeedMode additiveMoveSpeedMode;
        public MMFeedbacks initialFeedbacks;
        [Header("增加百分比，Number类型才有效")]
        public float _addPercent = 0;
        CharacterConfig characterConfigure;

        AutoHide autoHide;
        float skillCDTime = 0f;
        ParticleSystem trangleParticle;//三角片拖尾效果
        GameObject additiveEffectGo;
        GameObject slowDownEffectGo;//减速的粒子效果
        GameObject tornadoEffectGo;//龙卷风粒子效果

        private void Awake()
        {
            initialFeedbacks?.Initialization();
            autoHide = GetComponent<AutoHide>();
            skillCDTime = autoHide.time;
            trangleParticle = transform.Find("Trangle").GetComponent<ParticleSystem>();
            slowDownEffectGo = transform.Find("RegenerateRed").gameObject;
            tornadoEffectGo = transform.Find("Tornado loop").gameObject;
            additiveEffectGo = transform.Find("RegenerateBlue").gameObject;
        }

        private void OnEnable()
        {
            initialFeedbacks?.PlayFeedbacks();
            StartCoroutine(ReadyToFadeParticle());
        }

        IEnumerator ReadyToFadeParticle()
        {
            yield return new WaitForSeconds(skillCDTime - 1f);
            trangleParticle.startLifetime = 0.1f;
            trangleParticle.maxParticles = 5;
            slowDownEffectGo.SetActive(true);
            tornadoEffectGo.SetActive(false);
            additiveEffectGo.SetActive(false);
        }

        private void ReSetParticleConfiguration()
        {
            trangleParticle.startLifetime = 1f;
            trangleParticle.maxParticles = 200;
            slowDownEffectGo.SetActive(false);
            tornadoEffectGo.SetActive(true);
            additiveEffectGo.SetActive(true);
        }

        ///<summary>
        ///解除Buff
        /// </summary>
        private void OnDisable()
        {
            RemoveBuff();
            ReSetParticleConfiguration();
        }

        /// <summary>
        /// 启动buff
        /// </summary>
        private void ActivateBuff()
        {
            CharacterMovement move = owner.GetComponent<CharacterMovement>();
            if(move!=null)
            {
                characterConfigure = move.characterConfigure;

                switch(additiveMoveSpeedMode)
                {
                    case AdditiveMoveSpeedMode.Number:
                        additiveMoveSpeed = (characterConfigure.additiveMoveSpeed+characterConfigure.characterMoveSpeed)*
                        _addPercent;

                        additiveAttackSpeed = (characterConfigure.characterAttachSpeed + 
                        characterConfigure.characterAddtiveAttackSpeed) * _addPercent;
                        break;
                    case AdditiveMoveSpeedMode.Double:
                        additiveMoveSpeed = characterConfigure.additiveMoveSpeed + characterConfigure.characterMoveSpeed;

                        additiveAttackSpeed = characterConfigure.characterAddtiveAttackSpeed +
                         characterConfigure.characterAttachSpeed;
                        break;
                }
                characterConfigure.additiveMoveSpeed += additiveMoveSpeed;
                characterConfigure.characterAddtiveAttackSpeed += additiveAttackSpeed;
                MoveAndPenertrate(true);
            }
        }

        /// <summary>
        /// 移动穿透敌人
        /// </summary>
        private void MoveAndPenertrate(bool isOn)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), isOn);
        }

        private void RemoveBuff()
        {
            if (characterConfigure != null)
            {
                MoveAndPenertrate(false);
                characterConfigure.additiveMoveSpeed -= additiveMoveSpeed;
                characterConfigure.characterAddtiveAttackSpeed -= additiveAttackSpeed;

                Debugs.LogInformation("Pen Sp buff skill additiveMoveSpeed:"+additiveMoveSpeed+":"+"additive attack speed:"
                +additiveAttackSpeed,Color.cyan);
            }
        }

        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
            ActivateBuff();
        }
    }
}
