using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HTLibrary.Application
{ 
    //public class CameraDaptationEditor
    //{
    //    private static Camera _camera;
    //    [MenuItem("HTLibrary/cameraDaptation", false, -20)]
    //    public static void cameraDaptation()
    //    {
    //        _camera = Selection.activeGameObject.GetComponent<Camera>();
    //        _camera.orthographicSize = _camera.orthographicSize * 768 / 1024 * Screen.width / Screen.height;
    //    }
    //}
	public class CameraDaptation : MonoBehaviour
	{
        public float initWidth, initHeight, initSize;
        private Camera _camera;
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            //_camera.orthographicSize =  (initSize * (initWidth / initHeight)) / (Screen.width/Screen.height);
           switch(Screen.width)
            {
                case 1024:
                    _camera.orthographicSize = 135.58f;
                    break;
                case 1600:
                    _camera.orthographicSize = 202.58f;
                    break;
                case 1920:
                    _camera.orthographicSize = 250.33f;
                    break;
            }
        }
    }
}
