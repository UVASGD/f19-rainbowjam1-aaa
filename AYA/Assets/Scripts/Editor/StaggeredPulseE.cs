using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaggeredPulse))]
public class StaggeredPulseE : Editor {


    public override void OnInspectorGUI() {
        var serProp = serializedObject.GetIterator();
        serProp.NextVisible(true);

        do {
            switch (serProp.name) {
                case "intensityMul":
                case "frequency":
                case "timing":
                case "easing":
                case "type":
                case "majorScaleAmplitude":
                case "minorScaleAmplitude":
                    // Do Nothing
                    break;
                default:
                    EditorGUILayout.PropertyField(serProp);
                    break;
            }
        } while (serProp.NextVisible(true));


        serializedObject.ApplyModifiedProperties();
    }
}
