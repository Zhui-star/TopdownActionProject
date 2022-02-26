using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using Cinemachine;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{

    /// <summary>
    /// 相机控制
    /// </summary>
    public class HTCameraController : MonoBehaviour
    {
        //private float distance;
        public float maxDistance = 30; //摄像机最大距离
        public float minDistance = 15; //摄像机最小距离
        public float scrollSpeed = 2; //滚轮移动速度

        MoreMountains.TopDownEngine.InputManager inputManager;
        CinemachineVirtualCamera virCamera;
        CinemachineFramingTransposer framingTransposer;

        private float _targetDistance = 0;
        private float _targetTransitonDuration = 0;
        private float _targetDuration = 0;
        private float _transitionTimer = 0;
        private float _durationTimer = 0;
        private float _originalDisntace;
        bool _recover = false;
        // Start is called before the first frame update
        void Start()
        {

            inputManager = MoreMountains.TopDownEngine.InputManager.Instance;
            virCamera = GetComponent<CinemachineVirtualCamera>();
            framingTransposer = virCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_MaximumDistance = 30;
            framingTransposer.m_MinimumDistance = 15;
            //distance = framingTransposer.m_CameraDistance;

            if (PlayerPrefs.HasKey(Consts.CameraDistance + SaveManager.Instance.LoadGameID) && PlayerPrefs.GetFloat(Consts.CameraDistance +
                SaveManager.Instance.LoadGameID
                ) >= minDistance)
            {
                framingTransposer.m_CameraDistance = PlayerPrefs.GetFloat(Consts.CameraDistance + SaveManager.Instance.LoadGameID);
            }
        }

        private void LateUpdate()
        {
            ProcessCameraDistanceProcess();
            //Debug.Log("鼠标滚轮输入反馈:" + inputManager.PrimaryCameraZoom);
            //Debug.Log("")
            ScrollView();

            //Debug.Log("鼠标滚轮输入反馈:" + inputManager.PrimaryCameraZoom);
        }

        void ScrollView()
        {
            if (_targetDistance != 0)
                return;
            if (inputManager.PrimaryCameraZoom != 0)
            {
                if (framingTransposer.m_CameraDistance >= minDistance && framingTransposer.m_CameraDistance <= maxDistance)
                {
                    framingTransposer.m_CameraDistance -= inputManager.PrimaryCameraZoom * scrollSpeed;
                    //framingTransposer.m_CameraDistance = Mathf.Clamp(framingTransposer.m_CameraDistance, 4, 25);
                }
                else if (framingTransposer.m_CameraDistance < minDistance)
                {
                    framingTransposer.m_CameraDistance = minDistance;
                }
                else if (framingTransposer.m_CameraDistance > maxDistance)
                {
                    framingTransposer.m_CameraDistance = maxDistance;
                }
            }
            //framingTransposer.m_CameraDistance = distance;
        }

        /// <summary>
        /// Set camera distance
        /// </summary>
        /// <param name="distance">target distance</param>
        /// <param name="transitionDuration">transition of animation time</param>
        /// <param name="duration">keep target distance time</param>
        public void SetCameraDistance(float distance, float transitionDuration, float duration)
        {
            _targetDistance = distance;
            _targetTransitonDuration = transitionDuration;
            _targetDuration = duration;
            _originalDisntace = framingTransposer.m_CameraDistance;
        }

        /// <summary>
        /// Process transition, keep target distance state, recover
        /// </summary>
        void ProcessCameraDistanceProcess()
        {
            if (_targetDistance <= 0) return;
            if (!_recover && _transitionTimer < _targetTransitonDuration)
            {
                framingTransposer.m_CameraDistance = Mathf.Lerp(_originalDisntace, 
                _targetDistance, _transitionTimer / _targetTransitonDuration);
                _transitionTimer += Time.deltaTime;
                return;
            }

            if (_durationTimer < _targetDuration)
            {
                framingTransposer.m_CameraDistance=_targetDistance;
                _durationTimer += Time.deltaTime;
                return;
            }

            if (_transitionTimer > 0)
            {
                _recover = true;
                framingTransposer.m_CameraDistance = Mathf.Lerp(_originalDisntace, _targetDistance, 
                _transitionTimer / _targetTransitonDuration);
                _transitionTimer -= Time.deltaTime;
                return;
            }
            _recover=false;
            _targetDistance = 0;
            framingTransposer.m_CameraDistance=_originalDisntace;
        }


        private void OnDestroy()
        {
            if(framingTransposer.m_CameraDistance>maxDistance)
            {
                framingTransposer.m_CameraDistance=_originalDisntace>=minDistance?_originalDisntace:maxDistance;
            }

            PlayerPrefs.SetFloat(Consts.CameraDistance + SaveManager.Instance.LoadGameID,
             framingTransposer.m_CameraDistance);
            PlayerPrefs.Save();
        }
    }
}

