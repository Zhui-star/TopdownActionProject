using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTLibrary.Utility;
using System;
using NaughtyAttributes;
using MoreMountains.TopDownEngine;
using HTLibrary.Application;
namespace HTLibrary.Framework
{
    /// <summary>
    /// Level Manager 管理游戏关卡
    /// </summary>
    public class HTLevelManager : MonoSingleton<HTLevelManager>
    {
        private LevelManager levelManager;
        private LevelUnitManager _levelUnitManager;

        [HideInInspector]
        public bool isInitial = false;

        private PatnerDataManager _patnerDataManager;

        public event Action ResetDataEvent;

        [SerializeField]
        private List<int> _healthBarOrders = new List<int>();
        private int _healthBarOrderCurrentIndex=0;

        private MainManager _mainManager;

        public uint StartBattleMoney{get;set;}
        private void Awake()
        {
            _levelUnitManager = LevelUnitManager.Instance;
            _mainManager = MainManager.Instance;
        }

        /// <summary>
        /// 初始化人角色
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += LoadScenesCallBack;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= LoadScenesCallBack;

           
            ResetBeforeLevelData(true);
        }

        private void Start()
        {
          
            DontDestroyOnLoad(this);

            _patnerDataManager = PatnerDataManager.Instance;
        }

        public int CurrentCharacterID=1;

        /// <summary>
        /// 当前角色学习到的技能
        /// </summary>
        [ReadOnly]
        public List<int> learenedSkills = new List<int>();

        public void AddLearenSkill(int id)
        {
            if(learenedSkills.Contains(id))
            {
                return;
            }


            learenedSkills.Add(id);
        }

        /// <summary>
        /// 清空当前学习的技能
        /// </summary>
        public void ClearLearnSkill()
        {
            learenedSkills.Clear();
        }

        /// <summary>
        /// 返回已学会的技能
        /// </summary>
        /// <returns></returns>
        public List<int> ReturnLearenedSkills()
        {
            return learenedSkills;
        }

        /// <summary>
        /// 场景加载回调
        /// </summary>
        /// <param name="secne"></param>
        /// <param name="sceneType"></param>
        void LoadScenesCallBack(Scene secne,UnityEngine.SceneManagement.LoadSceneMode sceneType)
        {
            bool testMode = false;
            switch(_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    testMode = true;
                    break;
                case EnviormentMode.Developing:
                    testMode = false;
                    break;
                case EnviormentMode.Production:
                    testMode = false;
                    break;
            }

            if (!testMode&&(secne.buildIndex == 0 || secne.buildIndex == 1)) return; // 测试模式下打包要更改这个
            levelManager = LevelManager.Instance;

            if (levelManager != null && levelManager.PlayerPrefabs.Length>0)
            {
             
                levelManager.PlayerPrefabs[0] = CharacterSelection.
                    Instance.GetCharacterUnit(CurrentCharacterID).characterPrefab.GetComponent<Character>();

            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void ResetData(bool shutdown = false)
        {
            CurrentCharacterID = 1;
            ClearLearnSkill();
            ResetBeforeLevelData(shutdown);

            HTDBManager.Instance.ResetWeapon();
            HTDBManager.Instance.ClearPickUpList();

            _patnerDataManager.ClearFollowPatnerList();
        }

        /// <summary>
        /// 每次进入主题关卡重置数据
        /// </summary>
        void ResetBeforeLevelData(bool shutdown=false)
        {
            PlayerPrefs.DeleteKey(Consts.GameLevel+SaveManager.Instance.LoadGameID);
            PlayerPrefs.DeleteKey(Consts.GameExp+SaveManager.Instance.LoadGameID);
            //PlayerPrefs.DeleteKey(Consts.GameCurrentHP);

            PlayerPrefs.DeleteKey(Consts.LeagionKnightPassiveSkill + SaveManager.Instance.LoadGameID);

            if(!shutdown)
            {
                PlayerPrefs.SetInt(Consts.GameCurrentHP+SaveManager.Instance.LoadGameID, 666666);
            }
            else
            {
                PlayerPrefs.DeleteKey(Consts.GameCurrentHP+SaveManager.Instance.LoadGameID);
            }

            PlayerPrefs.DeleteKey(Consts.GameStateUnits + SaveManager.Instance.LoadGameID);

            PlayerPrefs.DeleteKey(SaveManager.Instance.LoadGameID + Consts.PatnerData);

            ResetDataEvent?.Invoke();


        }

        /// <summary>
        /// 返回血条优先级
        /// </summary>
        /// <returns></returns>
        public int GetHealthBarOder()
        {
            int order;
            _healthBarOrderCurrentIndex =_healthBarOrderCurrentIndex>=_healthBarOrders.Count-1? 0: _healthBarOrderCurrentIndex;
            order = _healthBarOrders[_healthBarOrderCurrentIndex];
            _healthBarOrderCurrentIndex++;
            return order;
        }

        /// <summary>
        /// Acuqired money in this battle
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public uint GetAcquiredMoney(uint money)
        {
            return money-StartBattleMoney;
        }


    }
}

