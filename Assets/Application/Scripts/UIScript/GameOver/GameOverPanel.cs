using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using DG.Tweening;
using UnityEngine.UI;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 游戏结束面板
    /// </summary>
    public class GameOverPanel : BasePanel
    {
        private CanvasGroup _canvasGroup;
        private HTPauseGame _pauseGame;
        private LevelUnitManager _levelUnitManager;
        private UIManager _uiManager;

        public Text _themLevelTxt;
        public Text _currentLevelProcessTxt;

        private MainManager _mainManager;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _levelUnitManager = LevelUnitManager.Instance;
            _pauseGame = HTPauseGame.Instance;
            _uiManager = UIManager.Instance;
            _mainManager = MainManager.Instance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            _canvasGroup.blocksRaycasts = true;
            transform.DOScale(1, 0.25f).OnComplete(()=>{
                _pauseGame.PauseGame();
            }).SetUpdate(true);

            UpdateUI();            
        }

        public override void OnExit()
        {
            base.OnExit();

            _canvasGroup.blocksRaycasts = false;
            transform.DOScale(0, 0.25f).OnComplete(()=>{_pauseGame.UnPauseGame();}).SetUpdate(true);
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        public void UpdateUI()
        {
            _themLevelTxt.text = "章节: " + _levelUnitManager.GetCurrentThemLevel();
            _currentLevelProcessTxt.text = "进度: " + _levelUnitManager.GetCurrentLevelProgress();
        }

        /// <summary>
        /// 回到最近大厅按钮事件
        /// </summary>
        public void RestartLevelClick()
        {
            switch(_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    TestModeClick();
                    break;
                default:
                    _uiManager.ClearStackPanel();
                   // _levelUnitManager.LoadNestLobby();
                   //TODO 测试用
                   _levelUnitManager.LoadMenueScenes();
                    break;
            }

            _canvasGroup.interactable = false;
        }

        /// <summary>
        /// 回到Menue 场景按钮事件
        /// </summary>
        public void ReturnMenueClick()
        {
            switch (_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    TestModeClick();
                    break;
                default:
                    _uiManager.ClearStackPanel();
                    _levelUnitManager.LoadMenueScenes();
                    break;
            }

            _canvasGroup.interactable = false;
        }



        /// <summary>
        /// 测试模式下加载场景
        /// </summary>
        void TestModeClick()
        {
            _uiManager.ClearStackPanel();
            _levelUnitManager.LoadTestModeScenes();
           
        }
    }

}
