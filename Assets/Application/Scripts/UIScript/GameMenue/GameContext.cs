using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Framework;
using HTLibrary.Utility;
using HTLibrary.Application;
namespace HTLibrary.Application
{
    /// <summary>
    /// UI 框架启动
    /// </summary>
    public class GameContext : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.PushPanel(UIPanelType.GameMenuePanel);
        }
    }

}
