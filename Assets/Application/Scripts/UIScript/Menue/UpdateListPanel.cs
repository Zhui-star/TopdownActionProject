using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using DG.Tweening;
using UnityEngine.UI;
namespace HTLibrary.Application
{
    /// <summary>
    /// 更新日志面板
    /// </summary>
    public class UpdateListPanel : MonoBehaviour
    {
        public float animTime = 0.5f;
        public TextAsset contentTxt;
        public Text content;

        private void Start()
        {
            UpdateContentUI();    
        }

        public void ChangePanel(bool open)
        {
            if(open)
            {
                transform.DOScale(1, animTime).OnComplete(OpenCallBack);
               
            }
            else
            {
                transform.DOScale(0, animTime);
            }
        }

        void UpdateContentUI()
        {
            content.text = contentTxt.text;
        }

        void OpenCallBack()
        {
            content.transform.localPosition = Vector3.zero;
        }
    }

}
