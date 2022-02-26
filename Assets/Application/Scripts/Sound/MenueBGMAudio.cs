using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class MenueBGMAudio : MonoBehaviour
    {
        [Header("游戏开场音效")]
        public AudioClip startGameClip;
        [Header("菜单背景音乐")]
        public AudioClip meneuBGMClip;

        protected AudioSource bgmSource;

        /// <summary>
        /// 先来一段 小小的音效
        /// </summary>
        private void Start()
        {
            SoundManager.Instance.PlaySound(startGameClip, Vector3.zero, false);
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.clip = meneuBGMClip;
            StartCoroutine(IPlayMenueBGM());
        }

        /// <summary>
        /// 延迟播放BGM
        /// </summary>
        /// <returns></returns>
        private IEnumerator IPlayMenueBGM()
        {
            yield return new WaitForSeconds(startGameClip.length);
            SoundManager.Instance.PlayBackgroundMusic(bgmSource, true);
        }
    }

}
