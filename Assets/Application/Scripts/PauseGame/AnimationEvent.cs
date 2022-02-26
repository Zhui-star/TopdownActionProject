using UnityEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 动画管理事件
    /// </summary>
    public class AnimationEvent : MonoBehaviour
    {
        private Animator anim;
        private CharacterFunctionSwitch _functionSwitch;
        private void Awake()
        {
            anim = GetComponent<Animator>();

            if(transform.parent!=null)
            {
                _functionSwitch=transform.parent.gameObject.GetComponent<CharacterFunctionSwitch>();
            }
        }

        private void OnEnable()
        {
            if(_functionSwitch!=null)
            {
              _functionSwitch.EventHandlerFrozen+=PausedAnimationEvent;
            }
        }
        private void OnDisable()
        {
            if(_functionSwitch!=null)
            {
                _functionSwitch.EventHandlerFrozen-=PausedAnimationEvent;
            }
        }

        /// <summary>
        /// 暂停动画播放
        /// </summary>
        /// <param name="paused"></param>
        void PausedAnimationEvent(bool paused)
        {
            if(paused)
            {
                anim.speed = 0;
            }
            else
            {
                anim.speed = 1;
            }
        }
    }

}
