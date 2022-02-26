using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    
    /// <summary>
    /// 主题关卡单位
    /// </summary>
    [System.Serializable]
    public struct ThemLevelUnit
    {
        public int ThemLevel;
        public int LevelCount;
    }
    /// <summary>
    /// 关卡管理器
    /// </summary>
    public class LevelUnitManager : MonoSingleton<LevelUnitManager>
    {
        private int currentLevelIndex;
        public int CurrentLevelIndex
        {
            get
            {
                return currentLevelIndex;
            }
            set
            {
                currentLevelIndex = value;
            }
        }
        public bool TestModel = true;
        public int TestLevelIndex = 1;
        public LevelUnitConfigure levelManager;

        [ReadOnly]
        public List<LevelUnit> levelUnitList = new List<LevelUnit>();

        Dictionary<int, LevelUnit> levelUnitDicts = new Dictionary<int, LevelUnit>();

        [Header("游戏结束")]
        public ThemLevelUnit[] _themLevelUnits;
        public int[] _lobbyScenesIndexs;

        private MainManager _mainManager;
        private void Awake()
        {
            if (TestModel)
            {
                CurrentLevelIndex = TestLevelIndex;
            }

            foreach (var temp in levelManager.LevelUnitList)
            {
                levelUnitList.Add(temp);

                levelUnitDicts.Add(temp.index, temp);
            }

            _mainManager = MainManager.Instance;
        }

        /// <summary>
        /// 加载下个关卡
        /// </summary>
        public void LoadNextScenes()
        {
         //  int scenesIndex = levelUnitList[CurrentLevelIndex].nextIndex;


            switch(_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    LoadTestModeScenes();
                    break;
                default:
                    List<int> nextSceneIndexs = levelUnitList[CurrentLevelIndex].nextIndexs;
                    int randomIndex = Random.Range(0, nextSceneIndexs.Count);
                    int scenesIndex = nextSceneIndexs[randomIndex];

                    LoadScenes(scenesIndex);
                    break;
            }
        }

        /// <summary>
        /// 加载上个关卡
        /// </summary>
        public void LoadBeforeScenes()
        {
            int scenesIndex = levelUnitList[CurrentLevelIndex].beforeIndex;
            CurrentLevelIndex--;

            LoadScenes(scenesIndex);

        }

        void LoadScenes(int index)
        {
            LevelUnit getNextLevelUnit = levelUnitDicts.TryGet<int, LevelUnit>(index);
            CurrentLevelIndex = levelUnitList.IndexOf(getNextLevelUnit);
            LoadScenesUtility.Instance.LoadScenes(index);

            SceneArgs args = new SceneArgs
            {
                scenesIndex = index
            };

            MVC.SendEvent(Consts.EnterScenesController, args as object);

        }

        /// <summary>
        /// 返回当前关卡的一个强度
        /// </summary>
        /// <returns></returns>
        public float ReturnCurrentLevelStrength()
        {
            if (levelUnitList.Count - 1 < CurrentLevelIndex) return 1;
            return levelUnitList[CurrentLevelIndex].levelStrenth;
        }

        ThemLevelUnit GetCurrentThemUnit()
        {
            string scensName = levelUnitList[CurrentLevelIndex].scenesName;
            string[] strArray = scensName.Split('-');
            int themLevelIndex = int.Parse(strArray[0]);
            
            if(themLevelIndex>_themLevelUnits.Length) //保险处理
            {
                themLevelIndex=0;
            }

            return _themLevelUnits[themLevelIndex];
        }
        /// <summary>
        /// 得到当前主题光卡
        /// </summary>
        /// <returns></returns>
        public int GetCurrentThemLevel()
        {
        
            return GetCurrentThemUnit().ThemLevel;
        }

        /// <summary>
        /// 返回当前主题关卡进度
        /// </summary>
        /// <returns></returns>
        public string GetCurrentLevelProgress()
        {
            string scensName = levelUnitList[CurrentLevelIndex].scenesName;
            string[] strArray = scensName.Split('-');

            string progressStr = strArray[1]+ " / " + GetCurrentThemUnit().LevelCount;
            return progressStr;
        }

        /// <summary>
        /// 加载到最近的大厅场景
        /// </summary>
        public void LoadNestLobby()
        {
            int lobbyIndex = GetCurrentThemUnit().ThemLevel-1;
            int targetScenesIndex = _lobbyScenesIndexs[lobbyIndex];

            LoadScenes(targetScenesIndex);
        }

        /// <summary>
        /// 加载到Menue场景
        /// </summary>
        public void LoadMenueScenes()
        {
            int targetScenesIndex = 0;

            LoadScenes(targetScenesIndex);
        }

        /// <summary>
        /// 测试模式下加载场景
        /// </summary>
        public void LoadTestModeScenes()
        {
            int targetScensIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            LoadScenesUtility.Instance.LoadScenes(targetScensIndex);


            SceneArgs args = new SceneArgs
            {
                scenesIndex = targetScensIndex
            };

            MVC.SendEvent(Consts.EnterScenesController, args as object);
        }

    }


}
