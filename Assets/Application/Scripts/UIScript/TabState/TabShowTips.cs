using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using UnityEngine.UI;
namespace HTLibrary.Application
{
    /// <summary>
    /// Tab信息面板 细节显示
    /// </summary>
    public class TabShowTips : MonoBehaviourSimplify
    {
        public Text _showInfoTxt;

        private void OnEnable()
        {
            EventTypeManager.AddListener<string>(HTEventType.ShowTabInfo, UpdateUI);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<string>(HTEventType.ShowTabInfo, UpdateUI);
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        /// <param name="infoTxt"></param>
        void UpdateUI(string infoTxt)
        {
            _showInfoTxt.text = infoTxt;
        }

        protected override void OnBeforeDestroy()
        {
          
        }
    }

}
