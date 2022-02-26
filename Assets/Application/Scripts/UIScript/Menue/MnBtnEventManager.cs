using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HTLibrary.Utility;
using HTLibrary.Application;

namespace HTLibrary.Framework
{ 
    public class MnBtnEventManager : MonoBehaviour
    {
        public SavePanel savenPanel;
        public void OnStartGameBtnClick()
        {
            savenPanel.TransformState(true);
        }
        public void OnSettingBtnClick()
        {
            UIManager.Instance.PushPanel(UIPanelType.SettingPanel);
        }

        public void OndeveloperListClick()
        {
            //TODO
        }

        public void OnExitBtnClick()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            UnityEngine.Application.Quit();
            #endif
        }
    }
}