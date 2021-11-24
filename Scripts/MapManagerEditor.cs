using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapManager))]

public class MapManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager builder = (MapManager)target;
        if (GUILayout.Button("Reload All"))
        {
            builder.ReloadAll();
        }
    }
}
