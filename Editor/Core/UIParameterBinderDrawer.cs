using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static GameFramework.UIKit.UIParameterBinder;

namespace GameFramework.UIKit
{
    [CustomEditor(typeof(UIParameterBinder))]
    public class UIParameterBinderDrawer : UnityEditor.Editor
    {
      
        private SerializedProperty entries;
        private ReorderableList entriesList;

        private void OnEnable()
        {
            entries = serializedObject.FindProperty("entries");
            entriesList = new ReorderableList(serializedObject,entries);
            entriesList.drawElementCallback -= DrawEntry;
            entriesList.drawElementCallback += DrawEntry;
            entriesList.onAddCallback -= AddEntry;
            entriesList.onAddCallback += AddEntry;
        }

        private void OnDisable()
        {
            entriesList.drawElementCallback -= DrawEntry;
            entriesList.onAddCallback -= AddEntry;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            entriesList.DoLayoutList();

            if (GUILayout.Button("Generate Panel Binder Code"))
            {
                var binder = (UIParameterBinder)target;
                BinderCodeGenerator.GenerateBinderCode_Panel(target.name, binder.entries);
            }
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

        }

        private GUIContent entryNameContent = new GUIContent();

        #region Draw Entries
        private void DrawEntry(Rect rect, int index, bool isActive, bool isFocused)
        {

            var property = entriesList.serializedProperty.GetArrayElementAtIndex(index);

            var nameProp = property.FindPropertyRelative("Name");
            var typeProp = property.FindPropertyRelative("ParameterType");
            ParameterType parameterType = (ParameterType)typeProp.enumValueIndex;

            int propertyCount = 4;
            if (parameterType == ParameterType.Component)
                propertyCount = 5;

            float space = 10f;
            var totalWidth = rect.width - space * propertyCount;
            var propertyWidth = totalWidth / propertyCount;
            //Label
            var labelPos = rect;
            labelPos.width = propertyWidth;
            if (!string.IsNullOrEmpty(nameProp.stringValue))
            {
                entryNameContent.text = nameProp.stringValue;
                EditorGUI.LabelField(labelPos, entryNameContent);
            }
            else
            {
                EditorGUI.LabelField(labelPos, property.displayName);
            }
            //Name
            var namePos = rect;
            namePos.x = labelPos.x + labelPos.width + space;
            namePos.width = propertyWidth;
            EditorGUI.PropertyField(namePos, nameProp, GUIContent.none);
            //PropertyType
            var typePos = rect;
            typePos.x = namePos.x + namePos.width + space;
            typePos.width = Mathf.Min(propertyWidth, 120f);
            EditorGUI.PropertyField(typePos, typeProp, GUIContent.none);
            //PropertyValue
            var parameterValuePos = rect;
            parameterValuePos.x = typePos.x + typePos.width + space;
            parameterValuePos.width = Mathf.Min(propertyWidth, 150f);
            if (parameterType == ParameterType.GameObject)
            {
                var goProp = property.FindPropertyRelative("Go");
                EditorGUI.PropertyField(parameterValuePos, goProp, GUIContent.none);
            }
            else if (parameterType == ParameterType.Component)
            {
                var componentProp = property.FindPropertyRelative("Component");
                EditorGUI.PropertyField(parameterValuePos, componentProp, GUIContent.none);

                var componentTypePos = rect;
                componentTypePos.x = parameterValuePos.x + parameterValuePos.width + space;
                componentTypePos.width = Mathf.Min(propertyWidth, 150f);

                string componentTypeStr= componentProp.objectReferenceValue == null ? "null" : 
                        componentProp.objectReferenceValue.GetType().Name;
                if (GUI.Button(componentTypePos, componentTypeStr)
                    && componentProp.objectReferenceValue != null)
                {
                    ShowSelectComponentMenu(componentTypePos, (Component)componentProp.objectReferenceValue, componentProp);
                }
            }


        }

        private void AddEntry(ReorderableList reorderableList)
        {
            var property = reorderableList.serializedProperty;
            int selectedIndex = -1;
            if (reorderableList.selectedIndices != null
                && reorderableList.selectedIndices.Count > 0)
            {
                selectedIndex = reorderableList.selectedIndices[0];
            }
            if (selectedIndex < 0)
            {
                property.InsertArrayElementAtIndex(reorderableList.count <= 0 ? 0 : reorderableList.count - 1);
            }
            else
            {
                property.InsertArrayElementAtIndex(selectedIndex);
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        private void ShowSelectComponentMenu(Rect position,Component currentComponent,SerializedProperty property)
        {
            var components = currentComponent.gameObject.GetComponents<Component>();
            GenericMenu menu = new GenericMenu();
            foreach (var component in components)
            {
                menu.AddItem(new GUIContent(component.GetType().Name), false, () =>
                {
                    property.objectReferenceValue = component;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.DropDown(position);
        }
        #endregion
    }
}
