using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WaterBuilder))]
public class WaterBuilderEditor : Editor
{
    /// <summary>
    /// 点击按钮生成水
    /// </summary>
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		WaterBuilder script = (WaterBuilder)target;
		if(GUILayout.Button("Build Water"))
		{
			script.BuildWater();
		}
	}
}