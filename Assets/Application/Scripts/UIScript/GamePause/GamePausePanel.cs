using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using DG.Tweening;

namespace HTLibrary.Application
{
    /// <summary>
    /// 游戏暂停面板
    /// </summary>
    public class GamePausePanel : BasePanel
    {
        CanvasGroup _canvasGroup;
        HTPauseGame _htPauseGame;
        LevelUnitManager _levelUnitManager;
        UIManager _uiManager;

        private MainManager _mainManager;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _htPauseGame = HTPauseGame.Instance;
            _levelUnitManager = LevelUnitManager.Instance;
            _uiManager = UIManager.Instance;
            _mainManager = MainManager.Instance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _canvasGroup.blocksRaycasts = true;
            transform.DOScale(1, 0.25f).OnComplete(() => { _htPauseGame.PauseGame(); }).SetUpdate(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _canvasGroup.blocksRaycasts = false;
            transform.DOScale(0, 0.25f).OnComplete(()=>{_htPauseGame.UnPauseGame();}).SetUpdate(true);
        }

        /// <summary>
        /// 返回开始界面
        /// </summary>
        public void ReturnMenueClick()
        {
            switch (_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    _uiManager.PopPanel();
                    break;

                default:
                    _levelUnitManager.LoadMenueScenes();
                    _uiManager.ClearStackPanel();
                    break;
            }

        }

        /// <summary>
        /// 打开设置面板
        /// </summary>
        public void OpenSettingClick()
        {
            UIManager.Instance.PushPanel(UIPanelType.SettingPanel);
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ContinueGameClick()
        {
            UIManager.Instance.PopPanel();
        }
    }

}
