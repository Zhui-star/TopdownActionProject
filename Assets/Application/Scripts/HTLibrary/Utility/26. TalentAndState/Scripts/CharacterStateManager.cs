using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HTLibrary.Application;
using HTLibrary.Framework;
using System.Text;
namespace HTLibrary.Utility
{
    [Serializable]
    public class StateUtility
    {
        public int ID;
        public TalentType talentType;
        public int level;
    }
    /// <summary>
    /// 角色状态管理 增益属性 BUFF DEBUFF
    /// </summary>
    public class CharacterStateManager : MonoBehaviour
    {
        private List<CharacterState> stateLists = new List<CharacterState>();

        [Header("初始化状态列表")]
        public List<StateUtility> stateUtilityList = new List<StateUtility>();

        //游戏中获得的符文之力
        private List<StateUtility> gameStateUnits = new List<StateUtility>();

        private HTLevelManager _htLevelManager;

        private void OnEnable()
        {
            if(this.gameObject.tag==Tags.Player)
            {
                TalentSystemManager.Instance.StudyTalentEvent += GetState;
                _htLevelManager.ResetDataEvent += ClearStateUnits;
            }
           
        }
        private void OnDisable()
        {
            if (this.gameObject.tag == Tags.Player)
            {
                TalentSystemManager.Instance.StudyTalentEvent -= GetState;
                _htLevelManager.ResetDataEvent -= ClearStateUnits;
            }

            foreach (var temp in stateLists)
            {
                temp.OnExit();
            }

            SaveGameStateUnits();

        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        private void Awake()
        {
            _htLevelManager = HTLevelManager.Instance;

            foreach (var temp in stateUtilityList)
            {
                GetState(temp.talentType, temp.level);
            }

            if (this.gameObject.tag == Tags.Player)
            {
                InitialTalentState();
            }
        }

        /// <summary>
        /// 状态应用
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
       public void GetState(TalentType type,int level)
        {
            CharacterState state = null;
            switch (type)
            {
                case TalentType.None:
                    break;
                case TalentType.AddAttack:
                    AddState<CharacterAddAttackState>(state, type, level);                     

                    break;
                case TalentType.AddAttackSpeed:
                    AddState<ChAddAttackSpeedState>(state, type, level);

                    break;
                case TalentType.AddCritRank:
                    AddState<CharacterAddCritState>(state, type, level);

                    break;
                case TalentType.AddHP:
                    AddState<CharacterAddHpState>(state, type, level);
                    break;
                case TalentType.AddDefence:
                    AddState<CharacterAddDefenceState>(state, type, level);
                    break;
                case TalentType.AddDodgeRank:
                    AddState<CharacterAddDodgeState>(state, type, level);
                    break;
                case TalentType.Furious:
                    AddState<CharacterFuriousState>(state, type, level);
                    break;
                case TalentType.AddExperienceGet:
                    AddState<CharacterAddExpState>(state, type, level);
                    break;
                case TalentType.AddMoveSpeed:
                    AddState<CharacterAddMoveSpeedState>(state, type, level);
                    break;
                case TalentType.Parry:
                    AddState<CharacterParryState>(state, type, level);
                    break;
                case TalentType.SuckBlood:
                    AddState<CharacterSuckBloodState>(state, type, level);
                    break;
                default:
                    break;
            }

        }

        private void Update()
        {
            foreach(var temp in stateLists)
            {
                temp.Process();
            }
        }

        void InitialTalentState()
        {
            foreach(var temp in TalentSystemManager.Instance.learnTalents)
            {
                TalentItem item = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(temp.ID);

                GetState(item.talentType, temp.Level);
            }

            LoadGameStateUnits();

            Debugs.LogInformation("Talent initial finished" + TalentSystemManager.Instance.learnTalents.Count + 
                " runne initial finished" + gameStateUnits.Count,Color.black);

            
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="level"></param>

        void AddState<T>(CharacterState state,TalentType type, int level) where T:CharacterState, new()
        {
             state = null;

            var checkState = stateLists.Find(x => x.GetType() == typeof(T));

            if (checkState != null)
            {
                if(checkState.level>=level)
                {
                    level += checkState.level;
                }
   
                stateLists.Remove(checkState);
                checkState.OnExit();

            }

            state = new T ();
            state.level = level;

            stateLists.Add(state);


            if (state != null)
            {
                state.OnEnter();
            }
        }

        /// <summary>
        /// 加载以获取的符文之力
        /// </summary>
        void LoadGameStateUnits()
        {
            if(PlayerPrefs.HasKey(Consts.GameStateUnits+SaveManager.Instance.LoadGameID))
            {
               string gameStates= PlayerPrefs.GetString(Consts.GameStateUnits+SaveManager.Instance.LoadGameID);
               string[] stateUnits=  gameStates.Split(':');
                foreach(var tempStateUnit in stateUnits)
                {
           
                    string[]  elements=  tempStateUnit.Split(',');
                    if(elements.Length<=1)
                    {
                        continue;
                    }

                    TalentType talentType;
                    Enum.TryParse<TalentType>(elements[0],out talentType);

                    int level = int.Parse(elements[1]);

                    int ID = int.Parse(elements[2]);
                    StateUtility stateUnit = new StateUtility();
                    stateUnit.talentType = talentType;
                    stateUnit.level = level;
                    stateUnit.ID = ID;

                    gameStateUnits.Add(stateUnit);
                }


                foreach (var tempGameStateUnits in gameStateUnits)
                {
                    GetState(tempGameStateUnits.talentType, tempGameStateUnits.level);
                }
            }
        }

        /// <summary>
        /// 保存符文之力
        /// </summary>
        void SaveGameStateUnits()
        {
            StringBuilder saveInfo = new StringBuilder();

            foreach(var temp in gameStateUnits)
            {
                saveInfo.Append(temp.talentType.ToString());
                saveInfo.Append(",");
                saveInfo.Append(temp.level.ToString());
                saveInfo.Append(",");
                saveInfo.Append(temp.ID.ToString());
                saveInfo.Append(":");
            }

         

            PlayerPrefs.SetString(Consts.GameStateUnits + SaveManager.Instance.LoadGameID,saveInfo.ToString());
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 添加符文之力保存
        /// </summary>
        /// <param name="stateUnit"></param>
        public void AddGameStateUnits(StateUtility stateUnit)
        {
            gameStateUnits.Add(stateUnit);
        }

        /// <summary>
        /// 返回所有捡到的符文天赋
        /// </summary>
        /// <returns></returns>
        public List<StateUtility> GetGameStates()
        {
            return gameStateUnits;
        }

        /// <summary>
        /// 清空符文之力
        /// </summary>
        void ClearStateUnits()
        {
            gameStateUnits.Clear();
        }




    }

}
