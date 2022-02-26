using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace HTLibrary.Utility
{
    /// <summary>
    /// UI褪色工具
    /// </summary>
    public class FadeInOutUtility : MonoBehaviour
    {
        [Header("Fader Speed")]
        public float fadeSpeed = 1.5f;

        [Header("Fade Image")]
        public Image tex;

        /// <summary>
        /// 是否颜色变淡
        /// </summary>
        public bool isClear = false;

        /// <summary>
        /// 是否颜色变深
        /// </summary>
        public bool isBlack = false;

        /// <summary>
        /// 颜色变浅
        /// </summary>
        private void FadeToClear()
        {
            tex.color = Color.Lerp(tex.color, Color.clear, fadeSpeed * Time.deltaTime);

        }

        /// <summary>
        /// 颜色变黑
        /// </summary>
        private void FadeToBlack()
        {
            tex.color = Color.Lerp(tex.color, Color.black, fadeSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        private void StartScene()
        {
            FadeToClear();
            tex.enabled = true;

            if (tex.color.a <= 0.05f)
            {
                tex.enabled = false;
                isClear = false;
            }
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        public void EndScen()
        {          
            FadeToBlack();
            tex.enabled = true;

            if (tex.color.a >= 0.95f)
            {
               // tex.enabled = false;
                isBlack = false;
            }
        }

        /// <summary>
        /// 差值运算
        /// </summary>
        private void Update()
        {
            if(isClear)
            {
                StartScene();
            }

            if(isBlack)
            {
                EndScen();
            }
        }

    }
}

