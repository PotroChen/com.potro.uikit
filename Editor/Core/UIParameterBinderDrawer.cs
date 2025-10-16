using UnityEngine;
using UnityEditor;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(UIParameterBinder))]
    public class UIParameterBinderDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Panel Binder Code"))
            {
                BinderCodeGenerator.GenerateBinderCode_Panel(target.name);
            }
        }


    }
}
