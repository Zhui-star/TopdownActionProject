using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 游戏启动器
    /// </summary>
    public class GameStartUp :MonoSingleton<GameStartUp>
    {
        private void Awake()
        {
            MVC.RegisterController(Consts.StartUpGameController, typeof(StartUpController));
        }
        // Start is called before the first frame update
        void Start()
        {
            MVC.SendEvent(Consts.StartUpGameController);
        }

      
    }
}

