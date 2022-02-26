using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;
using MoreMountains.TopDownEngine;
using static MoreMountains.TopDownEngine.CharacterStates;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 物体身上Icon
    /// </summary>
    public class ObjectIcon : MonoBehaviour
    {
        private Transform objectIcon;
        private Transform playerTransform;
        public float Offset = 200;
        private Health _health;

        private IEnumerator Start()
        {
            objectIcon = UIIndicatorPanel.Instance.GetObjectIconTrs();
            _health = GetComponent<Health>();
            _health.OnDeath += ImplementInView;

            ImplementInView();
            yield return new WaitUntil(() =>CharacterManager.Instance.GetCharacter("Player1")!=null);
            playerTransform = CharacterManager.Instance.GetCharacter("Player1").transform;
        }

        private void LateUpdate()
        {
            if (playerTransform == null)
            {              
                return;
            }

            if(_health._character.ConditionState.CurrentState!= CharacterConditions.Dead&&!IsInView())
            {
                IconUpdate();
            }
            else
            {
                ImplementInView();               
            }
        }

        private void OnDestroy()
        {
            _health.OnDeath -= ImplementInView;
        }

        /// <summary>
        /// 指示器UI 隐藏
        /// </summary>
        void ImplementInView()
        {
            //Debug.Log("<color=red> ObjectIcon.SetActive(false)");
            objectIcon.gameObject.SetActive(false);
        }


        /// <summary>
        /// 检测是否在屏幕中
        /// </summary>
        /// <returns></returns>
        bool IsInView()
        {
            Transform camTransform = Camera.main.transform;
            Vector2 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            Vector3 dir = (transform.position - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);
            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// UI更新偏移
        /// </summary>
        void IconUpdate()
        {
            objectIcon.gameObject.SetActive(true);

            Vector3 indicatorPoint = (transform.position - playerTransform.position).normalized + playerTransform.position;
            Vector3 indicatorScnPoint = Camera.main.WorldToScreenPoint(indicatorPoint);
            Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTransform.position);

            Vector2 indicatorUIPoint;
            Vector2 playerUIPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((UIIndicatorPanel.Instance.transform as RectTransform), 
                indicatorScnPoint, null, out indicatorUIPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle((UIIndicatorPanel.Instance.transform as RectTransform)
                , playerPos, null, out playerUIPos);


            Vector2 indicatorPos = playerUIPos + (indicatorUIPoint - playerUIPos).normalized*Offset;

            objectIcon.localPosition= new Vector3(indicatorPos.x, indicatorPos.y, 0);

            UILookAt(objectIcon, indicatorUIPoint - playerUIPos, Vector3.up);


        }

        /// <summary>
        /// 指示器望向玩家
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="dir"></param>
        /// <param name="lookAxis"></param>
        void UILookAt(Transform transform, Vector3 dir, Vector3 lookAxis)
        {
            Quaternion q = Quaternion.identity;
            q.SetFromToRotation(lookAxis, dir);
            transform.rotation = q;
        }

    }

}
