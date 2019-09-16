using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SongController))]
public class SongControllerE : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Toggle(SongController.instance.IsPlaying);

        if (GUILayout.Button("Toggle Tracking")) {
            if (!SongController.instance.IsPlaying) {
                SongController.instance.Continue();
            } else {
                SongController.instance.Pause();
            }
        }
    }
}