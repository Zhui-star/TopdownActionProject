using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Framework
{
    /// <summary>
    /// 游戏入口管理器
    /// 不同阶段执行不同逻辑
    /// production 产品阶段
    /// Test Q&A阶段
    /// Developing 开发阶段
    /// </summary>
    public enum EnviormentMode
    {
        Developing,
        Test,
        Production
    }
    public  class MainManager : MonoSingleton<MainManager>
    {
        [SerializeField] private EnviormentMode mode;
        public EnviormentMode Mode
        {
            get
            {
                return mode;
            }
        }

    }

}
