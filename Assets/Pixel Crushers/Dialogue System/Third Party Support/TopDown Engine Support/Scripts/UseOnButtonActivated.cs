using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using UnityEngine;
using HTLibrary.Application;
using MoreMountains.Feedbacks;
using HTLibrary.Utility;
using HTLibrary.Framework;
namespace PixelCrushers.DialogueSystem.TopDownEngineSupport
{

    /// <summary>
    /// This is a TopDown ButtonActivated component that invokes OnUse on 
    /// any Dialogue System triggers.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/Third Party/TopDown Engine/Use On Button Activated")]
    public class UseOnButtonActivated : ButtonActivated
    {
        [Tooltip("If Hide Prompt After Use is ticked, allow prompt to appear when re-entering zone.")]
        public bool hidePromptOnlyDuringConversation = true;

        private Character _character;
        CharacterOrientation3D _orientation;
        private bool _canActivate;

        public float _delayEnd = 0.5f;
        bool _IsStartConversation;
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
        protected override void OnEnable()
        {
            base.OnEnable();

            DialogueManager.instance.conversationStarted += StartCoversationCallBack;
            DialogueManager.instance.conversationEnded += EndCoversationCallBack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (DialogueManager.instance == null) return;
            DialogueManager.instance.conversationStarted -= StartCoversationCallBack;
            DialogueManager.instance.conversationEnded -= EndCoversationCallBack;
        }

        /// <summary>
        /// Target character smooth forward talking character
        /// </summary>
        private void Update()
        {
            if(_IsStartConversation&&_characterButtonActivation.GetActivateds().Contains(this))
            {
                _orientation.HTForceRotation(this.transform, true, 3.0f);
            }
        }

        public override void Initialization()
        {
            _character = null;
            _canActivate = true;
            base.Initialization();
        }

        protected override void TriggerEnter(GameObject collider)
        {
            _character = collider.gameObject.MMGetComponentNoAlloc<Character>();

            if (_character != null)
            {
                _orientation = _character.gameObject.GetComponent<CharacterOrientation3D>();
            }

            base.TriggerEnter(collider);

            if (collider.tag == Tags.Player)
            {
                if (envSound != null)
                {
                    envSound.PauseLoopSound();
                }

                if (triggerClip != null)
                {
                    SoundManager.Instance.PlaySound(triggerClip, transform.position, false);
                }

                triggerFeedBack?.Play(this.transform.position);

                if (anim != null)
                {
                    anim.SetBool("Interactive", true);
                }
            }
        }

        protected override void TriggerExit(GameObject collider)
        {
            _character = null;
            base.TriggerExit(collider);

            if (collider.tag == Tags.Player)
            {
                if (exitClip != null)
                {
                    SoundManager.Instance.PlaySound(exitClip, transform.position, false);
                }

                triggerFeedBack?.Stop(this.transform.position);

                if (anim != null)
                {
                    anim.SetBool("Interactive", false);
                }

                if (envSound != null)
                {
                    envSound.PlaySound();
                }
            }
        }

        protected override void ActivateZone()
        {
            if (!_canActivate) return;
            if (DialogueDebug.logInfo) Debug.Log(name + ".ActivateZone: Invoking OnUse on triggers.", this);
            base.ActivateZone();
            var actor = (_character != null) ? _character.transform : GameObject.FindGameObjectWithTag("Player").transform;
            SendMessage("OnUse", actor);
            if (hidePromptOnlyDuringConversation) _promptHiddenForever = false;
        }

        protected virtual void OnConversationEnd(Transform actor)
        {
            // Prevent double-activation if the activation button is the same as the dialogue UI's continue button hotkey.
            StartCoroutine(TemporarilyBlockActivation());
            if (hidePromptOnlyDuringConversation) ShowPrompt();
        }

        protected IEnumerator TemporarilyBlockActivation()
        {
            _canActivate = false;
            yield return null;
            _canActivate = true;
        }

        public void StartCoversationCallBack(Transform t)
        {
            GameManager.Instance.PlayingTimeline = true;

            if (_orientation == null) return;
            _IsStartConversation = true;
        }

        public void EndCoversationCallBack(Transform t)
        {
            StartCoroutine(ProcessConversationEnd());
        }

        /// <summary>
        /// Delay return noraml state
        /// </summary>
        /// <returns></returns>
        IEnumerator ProcessConversationEnd()
        {
            yield return new WaitForSeconds(_delayEnd);
            GameManager.Instance.PlayingTimeline = false;
            _IsStartConversation = false;
        }

        public void TriggerConversationEnd()
        {
            if (uiPanelType != UIPanelType.None)
            {
                switch (uiPanelType)
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
