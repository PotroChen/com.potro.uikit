using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(ExToggle))]
    public class ExToggleEditor : ToggleEditor
    {
        private SerializedProperty click;
        private SerializedProperty pointerEnter;

        protected override void OnEnable()
        {
            base.OnEnable();
            click = serializedObject.FindProperty(nameof(ExToggle.m_Click));
            pointerEnter = serializedObject.FindProperty(nameof(ExToggle.m_PointerEnter));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); 
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(click, new GUIContent(nameof(ExToggle.m_Click).Replace("m_","")));
            EditorGUILayout.PropertyField(pointerEnter, new GUIContent(nameof(ExToggle.m_PointerEnter).Replace("m_", "")));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
