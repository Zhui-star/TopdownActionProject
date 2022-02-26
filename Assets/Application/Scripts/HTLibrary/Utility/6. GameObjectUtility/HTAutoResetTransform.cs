using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    public enum ResetType
    {
        None,
        Position,
        Rotation,
        Scale
    }

    /// <summary>
    /// 激活和隐藏重置Transform信息
    /// </summary>
    public class HTAutoResetTransform: MonoBehaviour
    {
        public ResetType _resetType;

        private Vector3 originalPosition;
        private Vector3 _originScale;
        private Quaternion _originRotation;

        private void OnEnable()
        {
            switch(_resetType)
            {
                case ResetType.None:
                    break;
                case ResetType.Position:
                    originalPosition = transform.localPosition;
                    break;
                case ResetType.Rotation:
                    _originRotation = transform.localRotation;
                    break;
                case ResetType.Scale:
                    _originScale = transform.localScale;
                    break;
            }
            
        }

        private void OnDisable()
        {
            switch (_resetType)
            {
                case ResetType.None:
                    break;
                case ResetType.Position:
                    transform.localPosition = originalPosition;
                    break;
                case ResetType.Rotation:
                    transform.localRotation = _originRotation;
                    break;
                case ResetType.Scale:
                    transform.localScale = _originScale;
                    break;
            }
           
        }
    }

}
