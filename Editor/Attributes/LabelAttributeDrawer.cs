using UnityEngine;
using UnityEditor;
namespace GameFramework.UIKit
{

    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer:PropertyDrawer
    {
        private GUIContent labelContent = new GUIContent();
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            labelContent.text = label.text;
            if (attribute != null && attribute is LabelAttribute attri && !string.IsNullOrEmpty(attri.Text.Trim()))
            {
                labelContent.text = attri.Text.Trim();
            }
            EditorGUI.PropertyField(position, property, labelContent);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
