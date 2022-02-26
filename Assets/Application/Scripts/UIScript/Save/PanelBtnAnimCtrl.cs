using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace HTLibrary.Application
{
    /// <summary>
    /// 对存储面板的边幅按钮的动画控制
    /// </summary>
    public class PanelBtnAnimCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.1f, 0.3f).SetUpdate(true);
            //transform.DOPunchPosition (new Vector3(5,0,0),1f,3,0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one*1.1f, 0.2f).SetUpdate(true);
        }

    }
}


