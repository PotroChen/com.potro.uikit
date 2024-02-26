using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using static GameFramework.UIKit.RecycleScrollList;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(RecycleScrollList))]
    public class RecycleScrollListEditor : ScrollRectEditor
    {
        SerializedProperty m_Horizontal;
        SerializedProperty m_Vertical;

        SerializedProperty m_RecycleItemTemplate;
        SerializedProperty m_Padding;
        SerializedProperty m_CellSize;
        SerializedProperty m_Spacing;
        SerializedProperty m_ScrollType;
        SerializedProperty m_DirectionV;
        SerializedProperty m_DirectionH;
        SerializedProperty m_AutoCalculateConstraintCount;
        SerializedProperty m_Count;

        protected override void OnEnable()
        {
            m_Horizontal = serializedObject.FindProperty("m_Horizontal");
            m_Vertical = serializedObject.FindProperty("m_Vertical");
            base.OnEnable();
            m_RecycleItemTemplate = serializedObject.FindProperty("m_RecycleItemTemplate");

            m_Padding = serializedObject.FindProperty("m_Padding");
            m_CellSize = serializedObject.FindProperty("m_CellSize");
            m_Spacing = serializedObject.FindProperty("m_Spacing");

            m_DirectionV = serializedObject.FindProperty("m_DirectionV");
            m_DirectionH = serializedObject.FindProperty("m_DirectionH");
            m_AutoCalculateConstraintCount = serializedObject.FindProperty("m_AutoCalculateConstraintCount");
            m_Count = serializedObject.FindProperty("m_Count");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("默认配置", EditorStyles.boldLabel);
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("自定义配置", EditorStyles.boldLabel);
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_RecycleItemTemplate);

            EditorGUILayout.PropertyField(m_Padding);
            EditorGUILayout.PropertyField(m_CellSize);
            EditorGUILayout.PropertyField(m_Spacing);

            if (m_Horizontal.boolValue == m_Vertical.boolValue)
            {
                EditorGUILayout.HelpBox("不支持Horizontal和Vertical为相同值,将默认生效为Vertical", MessageType.Error);
            }

            if (((RecycleScrollList)target).ScrollDirection == ScrollType.Vertical)
            {
                EditorGUILayout.PropertyField(m_DirectionV);
            }
            else
            {
                EditorGUILayout.PropertyField(m_DirectionH);
            }

            EditorGUILayout.PropertyField(m_AutoCalculateConstraintCount);
            EditorGUILayout.PropertyField(m_Count);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

    }
}
