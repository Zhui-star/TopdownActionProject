using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class ButtonPrompt : MonoBehaviour
    {
        [Header("Bindings")]
        public Image Border;
        public Image Background;
        public CanvasGroup ContainerCanvasGroup;
        public Text PromptText;
        public Text ContainText;//为了自适应对话框

        [Header("Durations")]
        public float FadeInDuration = 0.2f;
        public float FadeOutDuration = 0.2f;
        
        protected Color _alphaZero = new Color(1f, 1f, 1f, 0f);
        protected Color _alphaOne = new Color(1f, 1f, 1f, 1f);
        protected Coroutine _hideCoroutine;

        protected Color _tempColor;

        public virtual void Initialization()
        {
            ContainerCanvasGroup.alpha = 0f;
        }

        public virtual void SetText(string newText)
        {
            if(ContainText!=null)
            {
                ContainText.text = newText;

                ContainText.text = ContainText.text.Replace("\\n", "\n");

            }

            PromptText.text = newText;
            PromptText.text= PromptText.text.Replace("\\n", "\n");
        }

        public virtual void SetBackgroundColor(Color newColor)
        {
            Background.color = newColor;
        }

        public virtual void SetTextColor(Color newColor)
        {
            PromptText.color = newColor;
        }

        public virtual void Show()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }
            
            StartCoroutine(MMFade.FadeCanvasGroup(ContainerCanvasGroup, FadeInDuration, 1f, true));
        }

        public virtual void Hide(bool Instant=false)
        {
            if(Instant)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                _hideCoroutine = StartCoroutine(HideCo());
            } 
        }

        protected virtual IEnumerator HideCo()
        {
            StartCoroutine(MMFade.FadeCanvasGroup(ContainerCanvasGroup, FadeOutDuration, 0f, true));
            yield return new WaitForSeconds(0.3f);
            this.gameObject.SetActive(false);
        }
    }
}