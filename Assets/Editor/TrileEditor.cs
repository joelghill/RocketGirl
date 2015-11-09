using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Trile))]
class TrileEditor : Editor
{
    public override void OnInspectorGUI() {
        // Show default inspector property editor
        DrawDefaultInspector();

        Trile myTrile = (Trile)target;

        if (GUILayout.Button("Apply Shader"))
        {
            myTrile.ApplyShader();
        }

    }
}
