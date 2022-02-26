using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
using System;
namespace HTLibrary.Application
{
    /// <summary>
    /// 展厅与继续游戏控制中心
    /// </summary>
    public class HTPauseGame :MonoSingleton<HTPauseGame>
    {

        MoreMountains.TopDownEngine. InputManager inputManager;
        private void Awake()
        {
            inputManager = MoreMountains.TopDownEngine.InputManager.Instance;
        }

        public void PauseGame()
        {
            if (GameManager.Instance == null) return;
             GameManager.Instance.Pause();       
        }

        public void UnPauseGame()
        {
            if (GameManager.Instance == null) return;
           GameManager.Instance.UnPause();
        }
    }

}
