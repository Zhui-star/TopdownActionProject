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
    public class StartUpController : HTLibrary.Framework.Controller
    {
        public override void Execute(object data)
        {
            RegisterController(Consts.EnterScenesController, typeof(EnterSceneController));
        }
    }

}
