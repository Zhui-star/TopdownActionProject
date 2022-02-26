using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using MoreMountains.TopDownEngine;
using System;
using System.Collections.Generic;
using HTLibrary.Utility;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    [Serializable]
    public struct TimelineUnit
    {
        public string _playbleKey;
        public PlayableAsset _playAbleClip;
        public bool _invicible;
    }
    /// <summary>
    /// 过场控制器
    /// </summary>
    public class TimelineController : MonoBehaviour
    {
        [Header("Director")]
        public PlayableDirector playAbleDirector;

        private bool timelinePlaying = true;

        private float timelineDuration;

        GameManager gameManager;

        public event Action<float> StartTimelineEvent;
        public event Action StopTimelineEvent;

        [Space]
        [Header("Play timeline but don't pause anything")]
        public TimelineUnit[] _recorders;
        private Dictionary<string, TimelineUnit> _recorderDict = new Dictionary<string, TimelineUnit>();

        public MMFeedbacks _PlayNoPauseFeedBacks;
        private Collider _collider;
        public CharacterController _controller;
        [Header("Play on awake")]
        public bool _playOnAwake = false;

        private void Awake()
        {
            gameManager = GameManager.Instance;
            _collider=GetComponent<Collider>();
            _controller=GetComponent<CharacterController>();
        }

        /// <summary>
        /// 开始播放过场
        /// </summary>
        public void StartPlayableTimeline()
        {
            gameManager.PlayingTimeline = true;
            timelinePlaying = true;

            StartCoroutine(WaitForTimelineToFinish());
        }

        /// <summary>
        /// 等待过场结束
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForTimelineToFinish()
        {
            timelineDuration = (float)playAbleDirector.duration;

            StartTimelineEvent?.Invoke(timelineDuration);

            yield return new WaitForSeconds(timelineDuration + 0.5f);
            timelinePlaying = false;
            gameManager.PlayingTimeline = false;

            StopTimelineEvent?.Invoke();
        }

        private void Start()
        {  
            
            if (_playOnAwake)//Play on awake
            {
                playAbleDirector.Play();
                StartPlayableTimeline();
            }

            ParseRecorders();
            _PlayNoPauseFeedBacks?.Initialization();
        }

        /// <summary>
        /// Parse playable table into dictionary
        /// </summary>
        void ParseRecorders()
        {
            foreach (var unit in _recorders)
            {
                _recorderDict.Add(unit._playbleKey, unit);
            }
        }

        /// <summary>
        /// Play Time with specific playble asset
        /// </summary>
        /// <param name="_recorderKey"></param>
        public void TimelinePlayNoPause(string _recorderKey)
        {
            TimelineUnit unit = _recorderDict.TryGet<string, TimelineUnit>(_recorderKey);
            playAbleDirector.playableAsset = unit._playAbleClip;
            playAbleDirector.Play();
            if(unit._invicible)
            {
                _collider.enabled=false;
                _controller.enabled=false;
            }

            _PlayNoPauseFeedBacks?.PlayFeedbacks();

            double timeDuration=playAbleDirector.duration;

            StartCoroutine(WaitTimelinePlayNoPauseOver((float)timeDuration));
        }

        /// <summary>
        /// Do sth when time line over 
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator WaitTimelinePlayNoPauseOver(float duration)
        {
            yield return new WaitForSeconds(duration);
            _collider.enabled=true;
            _controller.enabled=true;
        }
    }

}
