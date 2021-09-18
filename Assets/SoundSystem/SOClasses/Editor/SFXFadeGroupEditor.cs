using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

// Values of bool and cycle # don't persist upon hitting play
// Same serialization problem from last semester
// Look into solutions, check code you did in test folder before official GBE folder
[CustomEditor(typeof(SoundSystem.SFXLoop))]
public class SFXFadeGroupEditor : Editor
{
    AnimBool animBool;

    private SoundSystem.SFXLoop sfxLoop;

    private int intFieldCycleCount = 0;

    private void OnEnable()
    {
        sfxLoop = base.target as SoundSystem.SFXLoop;

        animBool = new AnimBool(false);
        animBool.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);

        GUILayout.Label("Loop Settings", EditorStyles.boldLabel);
        animBool.target = EditorGUILayout.ToggleLeft("Enable Finite Looping", animBool.target);

        if (!sfxLoop.isLoopedInfinitely)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                EditorGUI.indentLevel++;

                intFieldCycleCount = EditorGUILayout.IntField("Loops for # of Cycles", intFieldCycleCount);
                sfxLoop.NumCycles = intFieldCycleCount;

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
