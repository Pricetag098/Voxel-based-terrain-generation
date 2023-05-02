// EDITOR ONLY!
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MapGenerator myScript = (MapGenerator)target;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Generate Map"))
		{
			myScript.GenMap();
		}
        if (GUILayout.Button("Generate Mesh"))
        {
            myScript.GenMesh();
        }
		GUILayout.EndHorizontal();
        if (GUILayout.Button("Clear Map"))
        {
			myScript.map = null;
        }
        DrawDefaultInspector();

		
	}
}
#endif