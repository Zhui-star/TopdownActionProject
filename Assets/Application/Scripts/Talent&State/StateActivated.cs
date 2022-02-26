using System.Collections;
using UnityEngine;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 符文之力交互
    /// </summary>
    public class StateActivated : ButtonActivated
    {
        public SpriteRenderer spriteRender;
        Character character;
        TalentItem talentItem;

        public delegate void OnClickTalentDelegate();
        public OnClickTalentDelegate OnClickTalent;

        private TaltAwdChestCtrl chestCtrl;

        [Header("Runne Animation")]
        public Animator animator;
        public bool isGotten = false; //天赋自身是否被选中

        [Header("符文特效")]
        public ParticleSystem talentNormal;
        public MMFeedbacks activatedFeedbakcs;

        [Header("Test Mode")]
        [SerializeField] private bool _testMode;
        [SerializeField] private int _testSpecificID;
     
        private void Awake()
        {
            if (!_testMode)
            {
                character = CharacterManager.Instance.GetCharacter("Player1");
            }
        }

        protected override void OnEnable()
        {
            isGotten = false;

            base.OnEnable();

            if(!_testMode)
            {
                Initial();
            }
        }

        /// <summary>
        /// 初始化天赋
        /// </summary>
        void Initial()
        {
            DashDirectionEnum directionEnum = character.GetComponent<CharacterDash3D>().directionEnum;

            int talentID = _testMode?_testSpecificID:TalentSystemManager.Instance.GetRandomTalentStateID(directionEnum);
            talentItem = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(talentID);
            spriteRender.sprite = talentItem.TalentIcon;

            ButtonPromptText = talentItem.Descritions[0]+"(F)";
        }


        private IEnumerator Start()
        {
            if (_testMode)
            {
                yield return new WaitUntil(() => CharacterManager.Instance.GetCharacter("Player1"));
                character = CharacterManager.Instance.GetCharacter("Player1");
                Initial();
            }

            activatedFeedbakcs?.Initialization();
        }

        /// <summary>
        /// 获取天赋/符文之力
        /// </summary>
        public override void TriggerButtonAction()
        {
            if (isGotten == false)
            {
                talentNormal.Stop();
                GameObject castEff = PoolManagerV2.Instance.GetInst("TalentChoose");
                castEff.transform.position = this.transform.position;
                isGotten = true;
                chestCtrl = this.gameObject.GetComponentInParent<TaltAwdChestCtrl>();


                base.TriggerButtonAction();
                CharacterStateManager stateManager = character.GetComponent<CharacterStateManager>();
                stateManager.GetState(talentItem.talentType, 1);

                StateUtility stateUnit = new StateUtility();
                stateUnit.talentType = talentItem.talentType;
                stateUnit.level = 1;
                stateUnit.ID = talentItem.ID;

                Health health=character.GetComponent<Health>(); // When get hp talent should restore current value;
                switch(stateUnit.talentType)
                {
                    case TalentType.AddHP:
                        Debugs.LogInformation("Before pick up hp runne current Hp:" + health.CurrentHealth, Color.green);
                        health.CurrentHealth+=20;
                        Debugs.LogInformation("After pick hp runne current Hp:" + health.CurrentHealth, Color.blue);
                    break;
                }

                stateManager.AddGameStateUnits(stateUnit);
                
                animator.Play("Gotten");
                activatedFeedbakcs?.PlayFeedbacks();
                Invoke("TalentChosen",0.5f);

                if (chestCtrl != null)
                {
                    chestCtrl.isGotten = true;
                }
                OnClickTalent?.Invoke();
            }
            
            
        }

        private void TalentChosen()
        {
            this.gameObject.SetActive(false);
           
        }
    }

}
