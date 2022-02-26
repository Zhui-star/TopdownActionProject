using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// UI指示器面板
    /// </summary>
    public class UIIndicatorPanel : MonoSingleton<UIIndicatorPanel>
    {
        public GameObject objectIcon;

        /// <summary>
        /// 得到一个一个物体Icon
        /// </summary>
        /// <returns></returns>

        public Transform GetObjectIconTrs()
        {
          GameObject _object=  GameObject.Instantiate(objectIcon, transform);
            return _object.transform;
        }
    }

}
