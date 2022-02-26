using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using HTLibrary.Application;
using HTLibrary.Utility;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this class to a GameObject to have it play a background music when instanciated.
    /// Careful : only one background music will be played at a time.
    /// </summary>
    public class BackgroundMusic : MonoBehaviour
    {
        /// the background music
        public AudioClip NormalSoundClip;
        public BackGroundMusicConfigure backGroundMusicConfigure;
        public Dictionary<int, AudioClip> backGroundMusicUnit = new Dictionary<int, AudioClip>();

        protected AudioSource _source;

        LevelEvent levelEvent;

        private void OnEnable()
        {
            levelEvent = FindObjectOfType<LevelEvent>();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += DecideMusic;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= DecideMusic;
        }

        /// <summary>
        /// Gets the AudioSource associated to that GameObject, and asks the GameManager to play it.
        /// </summary>
        protected virtual void Awake()
        {
            _source = gameObject.AddComponent<AudioSource>() as AudioSource;
            _source.playOnAwake = false;
            _source.spatialBlend = 0;
            _source.rolloffMode = AudioRolloffMode.Logarithmic;
            _source.loop = true;

            _source.clip = NormalSoundClip;
          //  SoundManager.Instance.PlayBackgroundMusic(_source);
            InitialDict();
            
        }



        //初始化字典 将场景索引与背景音乐存入字典中
        private void InitialDict()
        {
            foreach (BackGroundMusicUnit temp in backGroundMusicConfigure.backGroundMusic)
            {
                if (backGroundMusicUnit.ContainsKey(temp.sceneIndex)) return;
                backGroundMusicUnit.Add(temp.sceneIndex, temp.audioClip);
            }
        }
        //判断场景切换背景音乐
        public void DecideMusic(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
           
            if(GetBackGroundMusicClip(scene.buildIndex)!=NormalSoundClip)
            {
                _source.clip = GetBackGroundMusicClip(scene.buildIndex);
               
                if(levelEvent!=null)
                {
                    SoundManager.Instance.PlayBackgroundMusic(_source,true, levelEvent.PauseBGM);
                }
                else
                {
                    SoundManager.Instance.PlayBackgroundMusic(_source);
                }


                NormalSoundClip = _source.clip;
            }
        }

        //根据场景索引获取背景音乐
        public AudioClip GetBackGroundMusicClip(int sceneIndex)
        {
            return backGroundMusicUnit.TryGet<int, AudioClip>(sceneIndex);
        }

        /// <summary>
        /// 播放特定BGM
        /// </summary>
        /// <param name="gameOverClip"></param>
        public void PlaySpecialBGM(AudioClip gameOverClip)
        {
            _source.clip = gameOverClip;
            _source.Play();
        }

    }
}
