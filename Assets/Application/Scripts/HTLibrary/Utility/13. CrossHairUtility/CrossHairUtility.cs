using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 十字准星使用工具
    /// 1. 初始化一个10子准星
    /// </summary>
    public class CrossHairUtility : MonoBehaviour
    {
        public float length;
        public float width;
        public float distance;
        [Header("background")]
        public Texture2D crosshairTexture;

        private GUIStyle lineStyle;

        [Header("cross hair image")]
        public Texture tex;

        private void Start()
        {
            lineStyle = new GUIStyle();
            lineStyle.normal.background = crosshairTexture;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect((Screen.width - distance) / 2 - length, (Screen.height - width) / 2, length, width), tex, lineStyle);

            GUI.Box(new Rect((Screen.width + distance) / 2, (Screen.height - width) / 2, length, width), tex, lineStyle);

            GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - distance) / 2 - length, width, length), tex, lineStyle);

            GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height + distance) / 2, width, length), tex, lineStyle);
        }
    }

}
