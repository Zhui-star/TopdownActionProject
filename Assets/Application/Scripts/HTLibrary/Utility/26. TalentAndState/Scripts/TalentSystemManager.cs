using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using System;
using MoreMountains.TopDownEngine;
using HTLibrary.Application;

namespace HTLibrary.Utility
{
    public class LearnedTalent
    {
        public int ID;
        public int Level;
    }

    /// <summary>
    /// 天赋管理器
    /// </summary>
    public class TalentSystemManager : MonoSingleton<TalentSystemManager>
    {

        // 当前选择的TalentItem
        private TalentItem pickedItem;
        public event Action<TalentItem> PickedTalentEvent;
        public event Action<TalentType, int> StudyTalentEvent;
        public event Action UpdateStudyUIEvent;
        public TalentItem Pickeditem
        {
            get
            {
                return pickedItem;
            }
            set
            {
                if (PickedTalentEvent != null)
                {
                    PickedTalentEvent(value);
                }

                pickedItem = value;
            }
        }

        //当前获得的天赋ID
        public List<LearnedTalent> learnTalents = new List<LearnedTalent>();

        public TalentItemConfigure talentConfigure;

        public Dictionary<int, TalentItem> talentItemDicts = new Dictionary<int, TalentItem>();

        List<int> archerTalents = new List<int>();
        List<int> swordTalents = new List<int>();

        //Test 用来充当灵源 （灵源用来点天赋)
        public int spiritualSource = 0;

        [HideInInspector]
        public GameObject enterImg;

        [HideInInspector]
        public ParticleSystem studyfeedback;

        [Header("测试模式")]
        public bool TestModel = false;

        public bool SelectTalent { get; set; }
        private void Awake()
        {
            if (TestModel)
            {
                Initial();
            }
        }

        public void Initial()
        {

            InitialTalentDicts();
        }

        /// <summary>
        /// 初始化天赋字典
        /// </summary>
        void InitialTalentDicts()
        {

            if (talentConfigure == null) return;

            List<TalentItem> talentList = talentConfigure.talentItems;

            foreach (var temp in talentList)
            {
                if (!talentItemDicts.ContainsKey(temp.ID))
                {
                    talentItemDicts.Add(temp.ID, temp);
                }

                if (temp.canPicked)
                {
                    switch (temp.directionEnum)
                    {
                        case DashDirectionEnum.Archer:
                            archerTalents.Add(temp.ID);
                            break;
                        case DashDirectionEnum.SowrdMan:
                            swordTalents.Add(temp.ID);
                            break;
                        default:
                            archerTalents.Add(temp.ID);
                            swordTalents.Add(temp.ID);
                            break;
                    }
                }
            }

            LoadLearnedTalents();
        }

        /// <summary>
        /// 天赋 id 是否已经学会
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsLearnTalent(int id)
        {
            foreach (var temp in learnTalents)
            {
                if (temp.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回指定ID 的天赋 等级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public int ReturnTalentLevel(int id)
        {
            foreach (var temp in learnTalents)
            {
                if (temp.ID == id)
                {
                    return temp.Level;
                }
            }
            return 0;
        }

        /// <summary>
        /// 是否解锁这个天赋
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUnlockTalent(int id)
        {
            if (talentItemDicts.TryGet<int, TalentItem>(id).IsInitial)
                return true;

            List<int> learnedIDs = new List<int>();

            foreach (var temp in learnTalents)
            {
                learnedIDs.Add(temp.ID);
            }

            foreach (var temp in learnedIDs)
            {
                TalentItem talentItem = talentItemDicts.TryGet<int, TalentItem>(temp);
                foreach (var temp2 in talentItem.BindIDs)
                {
                    if (temp2 == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 当前天赋ID 是否已经学满
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ISMaxLevel(int id)
        {
            int level = ReturnTalentLevel(id);
            TalentItem talentItem = talentItemDicts.TryGet<int, TalentItem>(id);
            if (talentItem.talenCosts.Count > level)
            {
                return false;
            }

            return true;
        }

        void AddTalentLevel(int id)
        {
            foreach (var temp in learnTalents)
            {
                if (temp.ID == id)
                {
                    temp.Level++;
                    break;
                }
            }
        }

        /// <summary>
        /// 返回下一级学习天赋的消耗
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int ReturnNextLearnCost(TalentItem item, int level)
        {
            List<TalentCost> talentCostList = item.talenCosts;
            int costs = 0;
            foreach (var temp in talentCostList)
            {
                if (temp.level == level+1)
                {
                    costs = temp.cost;
                    return costs;
                }
                
            }
            return costs;
        }

        /// <summary>
        /// 返回当前学习天赋的消耗
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int ReturnLearnCost(TalentItem item, int level)
        {
            List<TalentCost> talentCostList = item.talenCosts;
            int costs = 0;
            foreach (var temp in talentCostList)
            {
                if (temp.level == level)
                {
                    costs = temp.cost;
                    return costs;
                }
                if(level == 0)
                {
                    return talentCostList[0].cost;
                }
            }
            return costs;
        }

        /// <summary>
        /// 是否能够学习天赋
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanStudyTalent(int id)
        {

            TalentItem item = talentItemDicts.TryGet<int, TalentItem>(id);
            if (ReturnNextLearnCost(item, ReturnTalentLevel(id)) > HTDBManager.Instance.GetCoins())
                return false;

            if (ISMaxLevel(id))
                return false;

            if (!IsLearnTalent(id) && IsUnlockTalent(id))
                return true;

            if (IsLearnTalent(id))
                return true;

            return false;

        }
        /// <summary>
        /// 学习天赋
        /// </summary>
        /// <param name="id"></param>
        public bool StudyTalent(int id)
        {
            TalentItem item = talentItemDicts.TryGet<int, TalentItem>(id);

            if (IsLearnTalent(id))
            {

                AddTalentLevel(id);
                HTDBManager.Instance.SaveCoinConsume(ReturnLearnCost(item, ReturnTalentLevel(id)));
                StudyTalentEvent?.Invoke(item.talentType, ReturnTalentLevel(id));
                UpdateStudyUIEvent?.Invoke();
                
                if (studyfeedback != null)
                {
                    studyfeedback.Play();
                }

                return true;
            }

            if (!IsLearnTalent(id) && IsUnlockTalent(id))
            {
                LearnedTalent talent = new LearnedTalent();
                talent.ID = id;
                HTDBManager.Instance.SaveCoinConsume(ReturnLearnCost(item, ReturnTalentLevel(id)));

                learnTalents.Add(talent);

                AddTalentLevel(id);

                StudyTalentEvent?.Invoke(item.talentType, ReturnTalentLevel(id));
                UpdateStudyUIEvent?.Invoke();

                if (studyfeedback != null)
                {
                    studyfeedback.Play();
                }
                return true;
            }


            return false;
        }

        private void OnApplicationQuit()
        {
            SaveLearnedTalents();
        }

        /// <summary>
        /// 保存已经学会的天赋
        /// </summary>
        public void SaveLearnedTalents()
        {
            string str = "";
            foreach (var temp in learnTalents)
            {
                str += temp.ID + "," + temp.Level;
                str += ":";
            }
            if (string.IsNullOrEmpty(str)) return;

            PlayerPrefs.SetString(Consts.LearnedTalent + SaveManager.Instance.LoadGameID, str);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 加载已经学会的天赋
        /// </summary>
        public void LoadLearnedTalents()
        {
            learnTalents.Clear();
            if (!PlayerPrefs.HasKey(Consts.LearnedTalent + SaveManager.Instance.LoadGameID))
                return;

            string str = PlayerPrefs.GetString(Consts.LearnedTalent + SaveManager.Instance.LoadGameID);
            string[] itemStrs = str.Split(':');

            foreach (var temp in itemStrs)
            {
                if (string.IsNullOrEmpty(temp)) return;

                string[] item = temp.Split(',');
                int id = Int32.Parse(item[0]);
                int level = Int32.Parse(item[1]);

                LearnedTalent talent = new LearnedTalent();
                talent.ID = id;
                talent.Level = level;

                learnTalents.Add(talent);
            }

        }


        /// <summary>
        /// 返回随机天赋状态ID
        /// </summary>
        /// <param name="directionEnum"></param>
        /// <returns></returns>
        public int GetRandomTalentStateID(DashDirectionEnum directionEnum)
        {
            int index;
            int id = 0;
            switch (directionEnum)
            {
                case DashDirectionEnum.Archer:
                    index = UnityEngine.Random.Range(0, archerTalents.Count);
                    id = archerTalents[index];
                    break;
                case DashDirectionEnum.SowrdMan:
                    index = UnityEngine.Random.Range(0, swordTalents.Count);
                    id = swordTalents[index];
                    break;
            }

            return id;

        }

    }
}
