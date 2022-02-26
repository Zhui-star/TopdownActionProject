using UnityEngine;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// Promt poup adoptive when it exceed camera view rect 
    /// </summary>
    public class PromtAdoptive : MonoBehaviour
    {
        private ButtonActivated _buttonAcitvated;
        private ButtonPrompt _buttonPromt;
        public float _ySpeed = 2.0f;
        private Vector3 _visualPosition;

        // Start is called before the first frame update
        void Start()
        {
            _buttonAcitvated = GetComponent<ButtonActivated>();
            _buttonPromt = _buttonAcitvated._buttonPrompt;
        }

        // Update is called once per frame
        void Update()
        {
            PromtUIAdoptive();
        }

        /// <summary>
        /// Implement UI Y AXIS position
        /// </summary>
        void PromtUIAdoptive()
        {   
            if(_buttonPromt==null)
                _buttonPromt=_buttonAcitvated._buttonPrompt;
            
            if(_buttonPromt==null)
                return;
            if (_buttonPromt.gameObject.activeInHierarchy)
            {
                _visualPosition = Camera.main.WorldToViewportPoint(_buttonPromt.transform.position);
                if (_visualPosition.y > 0.9f)
                {
                    _buttonPromt.transform.Translate(-Vector3.up * _ySpeed, Space.World);
                }
            }
        }
    }

}
