using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BridgeBuilder))]
public class BridgeBuilderEditor : Editor
{
    /// <summary>
    /// 脚本扩展，点击按钮生成桥
    /// </summary>
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		BridgeBuilder script = (BridgeBuilder)target;
		if(GUILayout.Button("Build Bridge"))
		{
			script.BuildBridge();
		}
	}
}