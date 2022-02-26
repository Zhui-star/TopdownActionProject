using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    public class InteractiveZone :ButtonActivated
    {
        [Header("NPC交互触发配置v1.0")]
        public Animator anim;

        [MoreMountains.Tools.MMInformation("如果使用了UIManager该属性请参考UIPanelInfo配置文件填写合适的Panel Name",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        public UIPanelType uiPanelType;
        public InventoryType inventoryType;

        public MMFeedback triggerFeedBack;
        public AudioClip triggerClip;
        public AudioClip exitClip;

        private EnviromentSound envSound;

        private void Awake()
        {
            envSound = GetComponent<EnviromentSound>();
        }

        /// <summary>
        /// 遇到玩家开始指引交互
        /// </summary>
        /// <param name="collidingObject"></param>
        protected override void OnTriggerEnter(Collider collidingObject)
        {
            base.OnTriggerEnter(collidingObject);

            if(collidingObject.tag==Tags.Player)
            {
                if(envSound!=null)
                {
                    envSound.PauseLoopSound();
                }

                if(triggerClip!=null)
                {
                    SoundManager.Instance.PlaySound(triggerClip, transform.position, false);
                }

                triggerFeedBack?.Play(this.transform.position);

                if(anim!=null)
                {
                    anim.SetBool("Interactive", true);
                }
            }
        }

        protected override void OnTriggerExit(Collider collidingObject)
        {
            base.OnTriggerExit(collidingObject);

            if (collidingObject.tag == Tags.Player)
            {
                if(exitClip!=null)
                {
                    SoundManager.Instance.PlaySound(exitClip, transform.position, false);
                }

                triggerFeedBack?.Stop(this.transform.position);

                if (anim != null)
                {
                    anim.SetBool("Interactive", false);
                }

                if(envSound!=null)
                {
                    envSound.PlaySound();
                }
            }
        }

        /// <summary>
        /// Trigger Interactive
        /// </summary>
       public override void TriggerButtonAction()
        {
            base.TriggerButtonAction();

            if (uiPanelType != UIPanelType.None)
            {
                switch(uiPanelType)
                {
                    case UIPanelType.InventoryMenuePanel:
                        InventoryManager.Instance.invetoryType = inventoryType;
                        break;
                }

                UIManager.Instance.PushPanel(uiPanelType);
            }
        }
    }
}

