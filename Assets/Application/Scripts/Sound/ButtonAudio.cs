using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 处理Button音效
    /// </summary>
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        public AudioClip buttonHighlight;
        public AudioClip buttonClick;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (buttonClick== null) return;
            SoundManager.Instance.PlaySound(buttonClick, Vector3.zero, false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonHighlight == null) return;
            SoundManager.Instance.PlaySound(buttonHighlight, Vector3.zero, false);
        }
    }

}
