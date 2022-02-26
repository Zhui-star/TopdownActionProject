using UnityEngine;
using System.Collections;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System.Collections.Generic;
namespace HTLibrary.Application
{
    /// <summary>
    /// 循环开启关闭触发器
    /// </summary>
    public class LoopHideOpenCollider : MonoBehaviour
    {
        public Collider _collider;

        public float _openDuration = 0.1f;
        [MMInformation("多少秒开启一次",MMInformationAttribute.InformationType.Info,false)]
        public float _coolDownDuration;

        private void OnEnable()
        {
            StartCoroutine(LoopHideOpen());
        }

        IEnumerator LoopHideOpen()
        {
            while (true)
            {
                _collider.enabled = true;
                yield return new WaitForSeconds(_openDuration);
                _collider.enabled = false;
                yield return new WaitForSeconds(_coolDownDuration);
            }
        }
    }

}
