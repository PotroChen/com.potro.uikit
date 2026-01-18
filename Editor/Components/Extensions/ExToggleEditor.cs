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
        private SerializedProperty onClick;

        protected override void OnEnable()
        {
            base.OnEnable();
            onClick = serializedObject.FindProperty(nameof(ExToggle.onClick));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); 
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(onClick, new GUIContent(nameof(ExToggle.onClick)));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
