using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class EnterGamePanel : MonoBehaviour
    {
        public GameObject menuePanel;
        public AudioClip buttonClick;
        /// <summary>
        /// 进入游戏的按钮点击事件
        /// </summary>
        public void Update()
        {
            if(Input.anyKeyDown)
            {
                this.gameObject.SetActive(false);
                menuePanel.SetActive(true);
                SoundManager.Instance.PlaySound(buttonClick, Vector3.zero, false);
            }
        }
    }
}

