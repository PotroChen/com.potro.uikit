using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace GameFramework.UIKit
{
    [CanEditMultipleObjects, CustomEditor(typeof(NoDrawingGraphic), false)]
    public class NoDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);

            base.RaycastControlsGUI();
            base.serializedObject.ApplyModifiedProperties();
        }

    }
}
