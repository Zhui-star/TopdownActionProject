using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 播放环境音效
    /// </summary>
    public class EnviromentSound : MonoBehaviour
    {
        public AudioClip envClip;
        [Header("当前环境音效是否循环")]
        public bool loop = false;
        public AudioSource envAudioSource;
        public float pitch;
        public float pan;
        public float sptialBlend;
        public float volumeMultiple = 1;
        // Start is called before the first frame update
        void Start()
        {
            PlaySound();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySound()
        {
            SoundManager.Instance.PlaySound(envClip, transform.position, pitch, pan, sptialBlend, volumeMultiple, loop, envAudioSource);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        public void StopLoopSound()
        {
            SoundManager.Instance.StopLoopingSound(envAudioSource);
        }

        /// <summary>
        /// 暂停循环的环境音效
        /// </summary>
        public void PauseLoopSound()
        {
            if (!loop) return;
            SoundManager.Instance.PauseLoopingSound(envAudioSource);
        }

    }

}
