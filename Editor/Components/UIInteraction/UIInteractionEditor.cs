using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(UIInteraction))]
    public class UIInteractionEditor : Editor
    {
        class Styles
        {
            public GUIContent onPointerEnter_Label = new GUIContent("OnPointerEnter");
            public GUIContent onPointerExit_Label = new GUIContent("OnPointerExit");

            public GUIContent isOn_Label = new GUIContent("IsOn");
            public GUIContent isOff_Label = new GUIContent("IsOff");
            
            public GUIContent nodeActivity = new GUIContent("节点控制");
        }

        Styles styles = new Styles();

        SerializedProperty onPointerEnter;
        SerializedProperty onPointerExit;
        //Toggle相关
        SerializedProperty isToggle;
        SerializedProperty nodeActivity_IsOn;
        SerializedProperty nodeActivity_IsOff;
        private void OnEnable()
        {
            onPointerEnter = serializedObject.FindProperty(nameof(UIInteraction.interactions_onPointerEnter));
            onPointerExit = serializedObject.FindProperty(nameof(UIInteraction.interactions_onPointerExit));
            isToggle = serializedObject.FindProperty(nameof(UIInteraction.isToggle));
            nodeActivity_IsOn = serializedObject.FindProperty(nameof(UIInteraction.interactions_IsOn));
            nodeActivity_IsOff = serializedObject.FindProperty(nameof(UIInteraction.interactions_IsOff));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(onPointerEnter, styles.onPointerEnter_Label);
            EditorGUILayout.PropertyField(onPointerExit, styles.onPointerExit_Label);
            #region Toggle相关
            EditorGUILayout.PropertyField(isToggle);
            if (isToggle.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(nodeActivity_IsOn, styles.isOn_Label);
                EditorGUILayout.PropertyField(nodeActivity_IsOff,styles.isOff_Label);

                EditorGUI.indentLevel--;
            }
            #endregion

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
