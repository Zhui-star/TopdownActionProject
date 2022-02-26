using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 用于检测是否遮住了角色
    /// </summary>
    public class OcclutionCheckUitlity : MonoBehaviour
    {
        Vector3 storePosition;
        bool occlusion = false;
        bool isTrigger = false;
        public GameObject hideGo;
        public bool isUseMouseEnter = false;
        /// <summary>
        /// 遮挡了玩家
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag==Tags.Player)
            {
                SetColor(0.3f);
            }
        }

        /// <summary>
        /// 玩家离开了
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                isTrigger = false;
                SetColor(1.0f);
            }
        }

        /// <summary>
        /// 遮挡了玩家
        /// </summary>
        /// <param name="other"></param>

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                SetColor(0.3f);
                isTrigger = true;
            }
        }

        private void OnMouseEnter()
        {
            if (isUseMouseEnter)
            {
                SetState(false, 0.3f);
                storePosition = Input.mousePosition;
                occlusion = true;
            }
        }

        private void Update()
        {
            if(occlusion)
            {
                float distance = Vector3.Distance(storePosition, Input.mousePosition);
                if (distance > 5.0f)
                {
                    SetState(true, 1.0f);
                    occlusion = false;
                }
            }
        }

        void SetColor(float alpha)
        {
            if(hideGo!=null)
            {
                hideGo.SetActive(alpha == 1);
                return;
            }

            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (var temp in renders)
            {
                if (isTrigger) return;
                temp.material.color = new Color(temp.material.color.r, temp.material.color.g, temp.material.color.b, alpha);
            }
        }

        void SetState(bool enabled,float alpha)
        {

            SetColor(alpha);

            Collider[] cols = GetComponentsInChildren<Collider>();
            foreach (var temp in cols)
            {
                temp.enabled = enabled;
            }

        }
    }

}
