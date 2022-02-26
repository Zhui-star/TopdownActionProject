using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// z暂停播放BGMTrack
    /// </summary>
    public class StopBGMTrack : MonoBehaviour
    {
         private SoundManager soundManager;

         public TimelineController timeLineController;

        private void Awake()
        {
            soundManager = SoundManager.Instance; 
        }

        private void OnEnable()
        {
            timeLineController.StartTimelineEvent += PauseBGM;
            timeLineController.StopTimelineEvent += ReusemeBGM;
        }

        private void OnDisable()
        {
            timeLineController.StartTimelineEvent -= PauseBGM;
            timeLineController.StopTimelineEvent -= ReusemeBGM;
        }

        /// <summary>
        /// 暂停BGM
        /// </summary>
        /// <param name="duration"></param>
        public void PauseBGM(float duration)
        {
            soundManager.PauseBackgroundMusic();
        }

        /// <summary>
        /// 播放BGM
        /// </summary>

        public void ReusemeBGM()
        {
            soundManager.ResumeBackgroundMusic();
        }
    }

}
