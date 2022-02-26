using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;

namespace HTLibrary.Utility
{
    public enum LoadSceneMode
    {
        Next, //下个关卡
        Before //上个关卡
    }
    
    /// <summary>
    /// 加载场景触发器
    /// </summary>
    public class LoadScenesTrigger : MonoBehaviour
    {
        public LoadSceneMode loadScenesMode = LoadSceneMode.Next;
        bool onlyOnce;

        [Header("Start battle")]
        public bool _startBattleScens;
        private void Start()
        {
            onlyOnce = true;
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == Tags.Player && onlyOnce)
            {
                switch (loadScenesMode)
                {
                    case LoadSceneMode.Next:
                        if (_startBattleScens)
                        {
                            HTLevelManager.Instance.StartBattleMoney = (uint)HTDBManager.Instance.GetCoins();
                            Debugs.LogInformation("StartBattleMoney:"+HTLevelManager.Instance.StartBattleMoney,Color.yellow);
                        }

                        LevelUnitManager.Instance.LoadNextScenes();
                        break;
                    case LoadSceneMode.Before:
                        LevelUnitManager.Instance.LoadBeforeScenes();
                        break;
                }
                onlyOnce = false;
                UIManager.Instance.ClearStackPanel();

            }
        }
    }

}
