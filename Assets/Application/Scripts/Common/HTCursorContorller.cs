using MoreMountains.Tools;
using UnityEngine;
using System.Collections;
namespace HTLibrary.Application
{
    public enum CursorState
    {
        Idle,
        EnterPress,
        CursorProcessing,
        WaitCompleteCurrentState,
        EndPress
    }

    /// <summary>
    /// 指针控制器
    /// </summary>
    public class HTCursorContorller : MonoBehaviour
    {
        private MMStateMachine<CursorState> _cursorState;
        public Texture2D[] _cursorTextures;
        IEnumerator _IProcessing;
        private float _treshold = 0.5f;
        private float _lastPressTimer;
        private int _currentTexIndex = 0;
        private void Awake()
        {
            _cursorState = new MMStateMachine<CursorState>(gameObject, false);
            _cursorState.ChangeState(CursorState.Idle);
            Cursor.SetCursor(_cursorTextures[0],Vector2.zero,CursorMode.ForceSoftware);
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _cursorState.ChangeState(CursorState.EndPress);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                _cursorState.ChangeState(CursorState.EnterPress);
                _lastPressTimer = Time.time;
            }
            else if (Input.GetMouseButton(0))
            {
                _lastPressTimer = Time.time;
            }
            else   //再次检测
            {
                if (_currentTexIndex != 0)
                {
                    if(Time.time-_lastPressTimer>_treshold)
                    {
                        _cursorState.ChangeState(CursorState.EndPress);
                        _lastPressTimer=Time.time;
                    }
                }

            }


            switch (_cursorState.CurrentState)
            {
                case CursorState.Idle:
                    break;
                case CursorState.EnterPress:
                    EnterPress();
                    break;
                case CursorState.CursorProcessing:
                    Processing(_cursorState.PreviousState == CursorState.EnterPress);
                    break;
                case CursorState.WaitCompleteCurrentState:
                    break;
                case CursorState.EndPress:
                    EndPress();
                    break;

            }
        }

        /// <summary>
        /// 进入点击状态
        /// </summary>
        void EnterPress()
        {
            _cursorState.ChangeState(CursorState.CursorProcessing);
        }

        /// <summary>
        /// 松开点击状态
        /// </summary>
        void EndPress()
        {
            _cursorState.ChangeState(CursorState.CursorProcessing);
        }

        /// <summary>
        /// 处理动画过程
        /// </summary>
        /// <param name="asc">是否是按下状态</param>
        void Processing(bool asc)
        {
            _cursorState.ChangeState(CursorState.WaitCompleteCurrentState);

            if (_IProcessing != null)
            {
                StopCoroutine(_IProcessing);
            }
            _IProcessing = IProcessing(asc);
            StartCoroutine(_IProcessing);
        }

        IEnumerator IProcessing(bool asc)
        {

            if (asc)
            {
                for (int i = 0; i < _cursorTextures.Length; i++)
                {
                    yield return 0;
                    Cursor.SetCursor(_cursorTextures[i], Vector2.zero, CursorMode.ForceSoftware);
                    _currentTexIndex = i;
                }
            }
            else
            {
                for (int i = _cursorTextures.Length - 1; i >= 0; i--)
                {
                    yield return 0;
                    Cursor.SetCursor(_cursorTextures[i], Vector2.zero, CursorMode.ForceSoftware);
                    _currentTexIndex = i;
                }
            }

            _cursorState.ChangeState(CursorState.Idle);
        }
    }

}
