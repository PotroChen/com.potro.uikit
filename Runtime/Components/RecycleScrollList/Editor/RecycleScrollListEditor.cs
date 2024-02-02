using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameFramework.UIKit.RecycleScrollList;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(RecycleScrollList))]
    public class RecycleScrollListEditor : Editor
    {
        SerializedProperty m_SrollRect;
        SerializedProperty m_GridLayout;
        SerializedProperty m_RecycleItemTemplate;
        SerializedProperty m_ScrollType;
        SerializedProperty m_DirectionV;
        SerializedProperty m_DirectionH;
        SerializedProperty m_AutoCalculateConstraintCount;
        SerializedProperty m_Count;

        private void OnEnable()
        {
            m_SrollRect = serializedObject.FindProperty("m_SrollRect");
            m_GridLayout = serializedObject.FindProperty("m_GridLayout");
            m_RecycleItemTemplate = serializedObject.FindProperty("m_RecycleItemTemplate");
            m_ScrollType = serializedObject.FindProperty("m_ScrollType");
            m_DirectionV = serializedObject.FindProperty("m_DirectionV");
            m_DirectionH = serializedObject.FindProperty("m_DirectionH");
            m_AutoCalculateConstraintCount = serializedObject.FindProperty("m_AutoCalculateConstraintCount");
            m_Count = serializedObject.FindProperty("m_Count");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_SrollRect);
            EditorGUILayout.PropertyField(m_GridLayout);
            EditorGUILayout.PropertyField(m_RecycleItemTemplate);
            EditorGUILayout.PropertyField(m_ScrollType);

            if ((ScrollType)m_ScrollType.enumValueIndex == ScrollType.Vertical)
            {
                EditorGUILayout.PropertyField(m_DirectionV);
            }
            else
            {
                EditorGUILayout.PropertyField(m_DirectionH);
            }
            EditorGUILayout.PropertyField(m_AutoCalculateConstraintCount);
            EditorGUILayout.PropertyField(m_Count);
            if(EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

    }
}
