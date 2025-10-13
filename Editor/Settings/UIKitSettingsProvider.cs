using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameFramework.UIKit
{
    public class UIKitSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateUIKitSettingsProvider()
        {
            var provider = new UIKitSettingsProvider("Project/UI框架设置",SettingsScope.Project);
            return provider;
        }

        SerializedObject m_SerializedObject;
        SerializedProperty m_UIPanelGeneratedCodeDirectory;
        public UIKitSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
                : base(path, scopes, keywords) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            UIKitEditorSettings.instance.Save();

            m_SerializedObject = UIKitEditorSettings.instance.GetSerializedObject();
            m_UIPanelGeneratedCodeDirectory = m_SerializedObject.FindProperty("m_UIPanelGeneratedCodeDirectory");
        }

        /* ScriptableSingleton故意设置成DontSaveAndHide
         * 故意不希望我们用PropertyField.而是通过调用Save()保存(不知道为什么)
         */
        public override void OnGUI(string searchContext)
        {
            try
            {
                EditorGUI.indentLevel++;
                m_SerializedObject.Update();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Editor Settings", EditorStyles.boldLabel);
                m_UIPanelGeneratedCodeDirectory.stringValue = RelativeFolderPathFieldLayout("UIPanel相关代码生成文件夹", m_UIPanelGeneratedCodeDirectory.stringValue);

                if (EditorGUI.EndChangeCheck())
                {
                    m_SerializedObject.ApplyModifiedProperties();
                    UIKitEditorSettings.instance.Save();
                }
            }
            finally
            {
                EditorGUI.indentLevel--;
            }
        }


        string RelativeFolderPathFieldLayout(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            string path = EditorGUILayout.TextField(value);
            if (GUILayout.Button("...", UnityEngine.GUILayout.Width(30)))
            {
                string newPath = EditorUtility.OpenFolderPanel(label, value, "");
                if (newPath.Length != 0)
                {
                    path = PathUtils.MakeRelativePath(Application.dataPath, newPath);
                }

            }
            EditorGUILayout.EndHorizontal();
            return path;
        }

        string RelativeFilePathFieldLayout(string label, string value, string extension)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            string path = EditorGUILayout.TextField(value);
            if (GUILayout.Button("...", UnityEngine.GUILayout.Width(30)))
            {
                string newPath = EditorUtility.OpenFilePanel(label, value, extension);
                if (newPath.Length != 0)
                {
                    path = PathUtils.MakeRelativePath(Application.dataPath, newPath);
                }

            }
            EditorGUILayout.EndHorizontal();
            return path;
        }
    }
}

internal static class PathUtils
{
    public static string MakeRelativePath(string fromPath, string toPath)
    {
        // MONO BUG: https://github.com/mono/mono/pull/471
        // In the editor, Application.dataPath returns <Project Folder>/Assets. There is a bug in
        // mono for method Uri.GetRelativeUri where if the path ends in a folder, it will
        // ignore the last part of the path. Thus, we need to add fake depth to get the "real"
        // relative path.
        fromPath += "/fake_depth";
        try
        {
            if (string.IsNullOrEmpty(fromPath))
                return toPath;

            if (string.IsNullOrEmpty(toPath))
                return "";

            var fromUri = new System.Uri(fromPath);
            var toUri = new System.Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
                return toPath;

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = System.Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath;
        }
        catch
        {
            return toPath;
        }
    }
}
