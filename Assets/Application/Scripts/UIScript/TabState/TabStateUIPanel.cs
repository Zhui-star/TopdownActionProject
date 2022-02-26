using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HTLibrary.Application
{
    public enum TabItemType
    {
        None,
        TabEquipItem,
        TabTalentItem,
        TabPatnerItem,
        TabSkillItem

    }
    [System.Serializable]
    public struct TabItem
    {
        public TabItemType _tabItemType;
        public TabShowItemUI _tabShowItemUI;
    }

    /// <summary>
    /// Tab角色信息面板
    /// </summary>
    public class TabStateUIPanel : BasePanel
    {
        private HTPauseGame _htPauseGame;
        private CanvasGroup _canvasGroup;
        private HTCharacterSkill _htCharacterSkill;
        private CharacterSelection _characterSelection;
        private HTDBManager _htDBManager;
        private CharacterIdentity _characterIdentity;
        private TalentSystemManager _talentSystemManager;
        private PatnerDataManager _patnerDataManager;
        private SaveManager _saveManager;
        private SkillSystemManager _skillSystemManager;
        private InventoryManager _inventoryManager;
        private CharacterStateManager _stateManager;

        private PatnerController[] _patnerControllers;

        public TabItem[] _tabItems;

        private Dictionary<TabItemType, TabShowItemUI> _tabItemDicts = new Dictionary<TabItemType, TabShowItemUI>();


        [Header("UI 组件")]
        public Text _playerNameTxt;
        public Text _characterName;
        public Text _characterPropertieTxt;
        public Transform _skillContent;
        public Transform _equipContent;
        public Transform _talentContent;
        public Transform _patnerContent;

        public ScrollRect _propertiesScroll;
        private void Awake()
        {
            _htPauseGame = HTPauseGame.Instance;
            _canvasGroup = GetComponent<CanvasGroup>();
            _characterSelection = CharacterSelection.Instance;
            _talentSystemManager = TalentSystemManager.Instance;
            _patnerDataManager = PatnerDataManager.Instance;
            _saveManager = SaveManager.Instance;
            _skillSystemManager = SkillSystemManager.Instance;
            _inventoryManager = InventoryManager.Instance;
            _htDBManager = HTDBManager.Instance;

            InitialData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitialData()
        {
            foreach (var tabItem in _tabItems)
            {
                if (_tabItemDicts.ContainsKey(tabItem._tabItemType))
                {
                    continue;
                }

                _tabItemDicts.Add(tabItem._tabItemType, tabItem._tabShowItemUI);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();


            Character character = CharacterManager.Instance.GetCharacter("Player1");
            _htCharacterSkill = character.GetComponent<HTCharacterSkill>();
            _characterIdentity = character.GetComponent<CharacterIdentity>();
            _stateManager = character.GetComponent<CharacterStateManager>();
            _patnerControllers = FindObjectsOfType<PatnerController>();

            _canvasGroup.blocksRaycasts = true;
            transform.DOScale(1, 0.25f).OnComplete(()=>{_htPauseGame.PauseGame();}).SetUpdate(true);
            
            ClearParentChilds();
            UpdateCharacterPropertiesUI();
            UpdateStatePanel();
        }

        public override void OnExit()
        {
            base.OnExit();

            _canvasGroup.blocksRaycasts = false;
            transform.DOScale(0, 0.25f).OnComplete(()=>{_htPauseGame.UnPauseGame();}).SetUpdate(true);
        }

        /// <summary>
        /// 更新角色基础属性
        /// </summary>
        void UpdateCharacterPropertiesUI()
        {
            CharacterConfig config = _characterIdentity.characterConfigure;
            string str = "";
            str += "血量: " + (int)(config.characterHP + config.additiveHP )+ "\n";
            str += "攻击: " + (int)(config.characterAttack + config.additiveAttack) + "\n";
            str += "防御: " + (int)(config.characterDefence + config.additiveDefence)+ "\n";
            str += "攻击速度: " + (config.characterAttachSpeed + config.characterAddtiveAttackSpeed).ToString("f2")+ "\n";
            str += "闪避率: " + Mathf.Round((config.characterDodge + config.additiveDodge)*100)+"/%" + "\n";
            str += "暴击率: " + Mathf.Round((config.characterCritRank + config.additiveCritRank)*100)+"/% "+ "\n";
            str += "暴击倍率: " + (config.characterCritMultiple + config.additiveCritMultiple).ToString("f2") + "\n";
            str += "移动速度: " + (int)(config.characterMoveSpeed + config.additiveMoveSpeed);

            _characterPropertieTxt.text = str;

            //open panel scroll position in top
            //_propertiesScroll.verticalNormalizedPosition=0;
            _propertiesScroll.content.DOLocalMoveY(0,0.2f);
        }


        /// <summary>
        /// 更新状态面板
        /// </summary>
        void UpdateStatePanel()
        {
            _playerNameTxt.text = PlayerPrefs.GetString(Consts.Name + _saveManager.LoadGameID);
            CharacterUnit characterUnit = _characterSelection.GetCharacterUnit(_characterSelection.currentSelectIndex);
            _characterName.text = characterUnit.characterName;

            foreach (var skill in _htCharacterSkill.learnSkills)
            {
                SkillUnit skillUnit = _skillSystemManager.GetSkillById(skill);

                TabShowItemUI skillItemUI = GameObject.Instantiate(_tabItemDicts.TryGet<TabItemType, TabShowItemUI>
                    (TabItemType.TabSkillItem), _skillContent);

                (skillItemUI as TabSkillItemUI).SetSkillUnit(skillUnit);
                skillItemUI.UpdateUI();
            }

            foreach (var equip in _htDBManager.GetEquips())
            {


                Item _item = _inventoryManager.GetItemById(equip);

                TabShowItemUI equipItemUI = GameObject.Instantiate(_tabItemDicts.TryGet<TabItemType, TabShowItemUI>(TabItemType.TabEquipItem),
                    _equipContent);

                (equipItemUI as TabEquipItemUI).SetItem(_item);

                equipItemUI.UpdateUI();
            }

            foreach (var state in _stateManager.GetGameStates())
            {
                TalentItem _talentItem = _talentSystemManager.talentItemDicts.TryGet<int, TalentItem>(state.ID);

                TabShowItemUI talentItemUI = GameObject.Instantiate(_tabItemDicts.TryGet<TabItemType, TabShowItemUI>(TabItemType.TabTalentItem),
                  _talentContent);

                (talentItemUI as TabTalentItemUI).SetItem(_talentItem);
                talentItemUI.UpdateUI();
            }

            foreach (var patner in _patnerDataManager.GetFollowPatnerIds())
            {
                PatnerUnit patnerUnit = _patnerDataManager.GetPatnerById(patner);

                TabShowItemUI patnerItemUI = GameObject.Instantiate(_tabItemDicts.TryGet<TabItemType, TabShowItemUI>(TabItemType.TabPatnerItem),
                   _patnerContent);

                Health health = null;
                foreach (var patnerController in _patnerControllers)
                {
                    if (patnerController.ID == patner)
                    {
                        health = patnerController.GetComponent<Health>();
                        break;
                    }
                }

                (patnerItemUI as TabPatnerItemUI).SetPatner(patnerUnit, health);

                patnerItemUI.UpdateUI();
            }


        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanelClick()
        {
            UIManager.Instance.PopPanel();
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        void ClearParentChilds()
        {
            for (int i = 0; i < _skillContent.childCount; i++)
            {
                Destroy(_skillContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < _equipContent.childCount; i++)
            {
                Destroy(_equipContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < _talentContent.childCount; i++)
            {
                Destroy(_talentContent.GetChild(i).gameObject);
            }

            for (int i = 0; i < _patnerContent.childCount; i++)
            {
                Destroy(_patnerContent.GetChild(i).gameObject);
            }
        }

    }

}
