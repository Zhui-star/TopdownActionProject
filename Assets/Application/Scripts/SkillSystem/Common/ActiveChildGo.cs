using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 初始化激活所有孩子
    /// </summary>
    public class ActiveChildGo : MonoBehaviour
    {
        private void OnEnable()
        {
            ActiveChildsGo();
        }

        void ActiveChildsGo()
        {
            for(int i=0;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

}
