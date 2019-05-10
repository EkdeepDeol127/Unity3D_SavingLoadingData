using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(XmlManagerScript))]
public class LoadOrSaveScene : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("Save and Load JSON", EditorStyles.boldLabel);

        XmlManagerScript XMLScript = (XmlManagerScript)target;

        if (GUILayout.Button("Save JSON"))
        {
            XMLScript.SaveScene();
        }

        if (GUILayout.Button("Load JSON"))
        {
            XMLScript.LoadScene();
        }
    }
}