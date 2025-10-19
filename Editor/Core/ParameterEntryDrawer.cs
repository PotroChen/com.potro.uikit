//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//namespace GameFramework.UIKit
//{
//    using ParameterType = UIParameterBinder.ParameterType;

//    [CustomPropertyDrawer(typeof(UIParameterBinder.ParameterEntry))]
//    public class ParameterEntryDrawer:PropertyDrawer
//    {
//        private GUIContent nameContent = new GUIContent();
//        private ReorderableList componentTypeList = null;
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            var nameProp = property.FindPropertyRelative("Name");
//            var typeProp = property.FindPropertyRelative("ParameterType");
//            ParameterType parameterType = (ParameterType)typeProp.enumValueIndex;

//            int propertyCount = 4;

//            float space = 10f;
//            var totalWidth = position.width - space * propertyCount;
//            var propertyWidth = totalWidth / propertyCount;
//            //Label
//            var labelPos = position;
//            labelPos.width = propertyWidth;
//            if (!string.IsNullOrEmpty(nameProp.stringValue))
//            {
//                nameContent.text = nameProp.stringValue;
//                EditorGUI.LabelField(labelPos, nameContent);
//            }
//            else
//            {
//                EditorGUI.LabelField(labelPos, label);
//            }
//            //Name
//            var namePos = position;
//            namePos.x = labelPos.x + labelPos.width + space;
//            namePos.width = propertyWidth;
//            EditorGUI.PropertyField(namePos, nameProp, GUIContent.none);
//            //PropertyType
//            var typePos = position;
//            typePos.x = namePos.x + namePos.width + space;
//            typePos.width = Mathf.Min(propertyWidth, 120f);
//            EditorGUI.PropertyField(typePos, typeProp,GUIContent.none);
//            //PropertyValue
//            var parameterValuePos = position;
//            parameterValuePos.x =  typePos.x + typePos.width + space;
//            parameterValuePos.width = Mathf.Min(propertyWidth, 150f);
//            if (parameterType == ParameterType.GameObject)
//            {
//                var goProp = property.FindPropertyRelative("Go");
//                EditorGUI.PropertyField(parameterValuePos, goProp, GUIContent.none);
//            }
//            else if (parameterType == ParameterType.Component)
//            {
//                var componentProp = property.FindPropertyRelative("Component");
//                EditorGUI.PropertyField(parameterValuePos, componentProp, GUIContent.none);
//            }
//        }

//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return base.GetPropertyHeight(property, label);
//        }
//    }
//}
