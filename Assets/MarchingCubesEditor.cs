// EDITOR ONLY!
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MarchingCubesEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MapGenerator myScript = (MapGenerator)target;
		if (GUILayout.Button("Generate"))
		{
			myScript.GenMap();
		}
		DrawDefaultInspector();

		
	}
}
#endif