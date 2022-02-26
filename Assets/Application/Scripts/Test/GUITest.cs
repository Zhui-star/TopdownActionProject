using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Test
{
    /// <summary>
    /// GUI测试
    /// </summary>
    public class GUITest : MonoBehaviour
    {
        public Vector2 point;

        public GUIStyle style;
        private void OnGUI()
        {
            GUI.Label(new Rect(point.x, point.y, 100, 100), "Test",style);
        }
    }

}
