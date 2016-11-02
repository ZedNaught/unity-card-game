using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpellCard))]
public class SpellCardEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        SpellCard card = (SpellCard)target;
        if (GUILayout.Button("Play Card")) {
            card.Play(null);
        }
    }
}
