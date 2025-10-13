using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.UIKit
{
    [FilePath("ProjectSettings/UIKitEditorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UIKitEditorSettings : ScriptableSingleton<UIKitEditorSettings>
    {
        /// <summary>
        /// UIPanel相关代码生成文件夹
        /// </summary>
        public static string UIPanelGeneratedCodeDirectory { get { return instance.m_UIPanelGeneratedCodeDirectory; } }

        [SerializeField]
        private string m_UIPanelGeneratedCodeDirectory = "";

        void OnDisable()
        {
            Save();
        }

        public void Save()
        {
            Save(true);
        }

        internal SerializedObject GetSerializedObject()
        {
            return new SerializedObject(this);
        }
    }
}
