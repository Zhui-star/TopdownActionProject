using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MnBtnAnimCtrl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    private Animator btnAnim;

    private void OnEnable()
    {
        btnAnim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnAnim != null)
        {
            btnAnim.SetBool("msIsOver", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnAnim != null)
        {
            btnAnim.SetBool("msIsOver", false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (btnAnim != null)
        {
            btnAnim.SetBool("isClickBtn", true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (btnAnim != null)
        {
            btnAnim.SetBool("isClickBtn", false);
        }
    }
}
