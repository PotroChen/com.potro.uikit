using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameFramework.UIKit
{
    public static class BinderCodeGenerator
    {
        public static void GenerateBinderCode_Panel(string className)
        {
            string codeGenDirectory = UIKitEditorSettings.UIPanelGeneratedCodeDirectory?.Trim();
            if (string.IsNullOrEmpty(codeGenDirectory))
            {
                EditorUtility.DisplayDialog("Tip", "UIKitEditorSettings.UIPanelGeneratedCodeDirectory is empty!","ok");
                return;
            }

            string directory = $"{Application.dataPath}/{codeGenDirectory}/Panels";
            string shortFileName = $"{className}.gen.cs";
            GenerateBinderCode(className, directory, shortFileName);
            AssetDatabase.Refresh();
        }

        private static void GenerateBinderCode(string className,string directory, string shortFileName)
        {
            string filePath = $"{directory}/{shortFileName}";
            CodeNamespace ns = new CodeNamespace(UIKitEditorSettings.DefaultNameSpace);
            
            CodeTypeDeclaration uiClass = new CodeTypeDeclaration(className);
            uiClass.IsPartial = true;
            //添加基类
            uiClass.BaseTypes.Add(new CodeTypeReference("UIPanel"));

            ns.Types.Add(uiClass);
            ns.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            ns.Imports.Add(new CodeNamespaceImport("GameFramework.UIKit"));

            CodeCompileUnit compileUnit = new CodeCompileUnit();
            //设置所属Namespace
            compileUnit.Namespaces.Add(ns);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            GenerateCSharpCode(compileUnit, filePath);
        }

        private static void GenerateCSharpCode(CodeCompileUnit compileunit, string filePath)
        {
            Encoding encoding = Encoding.UTF8;
            CSharpCodeProvider provider = new CSharpCodeProvider();
            using (StreamWriter sw = new StreamWriter(filePath, false, encoding))
            {
                //会有auto-generated的注释，决定先留下(后续如果想要去掉,去掉TextWriter的前几行)
                //用4个空格作为Tab
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";//左花括号换行

                provider.GenerateCodeFromCompileUnit(compileunit, tw,
                    options);

                tw.Close();
            }

            var content = File.ReadAllText(filePath, encoding);
            content = SetLineEndings(content, EditorSettings.lineEndingsForNewScripts);
            File.WriteAllText(filePath, content, encoding);
        }

        #region Comdom Utils
        /// <summary>
        /// 注释
        /// </summary>
        /// <param name="content"></param>
        public static CodeCommentStatement Comment(string content)
        {
            CodeComment comment = new CodeComment(content, false);
            CodeCommentStatement commentStatement = new CodeCommentStatement(comment);
            return commentStatement;
        }

        //从源码拷贝出来的
        internal static string SetLineEndings(string content, LineEndingsMode lineEndingsMode)
        {
            const string windowsLineEndings = "\r\n";
            const string unixLineEndings = "\n";

            string preferredLineEndings;

            switch (lineEndingsMode)
            {
                case LineEndingsMode.OSNative:
                    if (Application.platform == RuntimePlatform.WindowsEditor)
                        preferredLineEndings = windowsLineEndings;
                    else
                        preferredLineEndings = unixLineEndings;
                    break;
                case LineEndingsMode.Unix:
                    preferredLineEndings = unixLineEndings;
                    break;
                case LineEndingsMode.Windows:
                    preferredLineEndings = windowsLineEndings;
                    break;
                default:
                    preferredLineEndings = unixLineEndings;
                    break;
            }

            content = Regex.Replace(content, @"\r\n?|\n", preferredLineEndings);

            return content;
        }


        #endregion
    }
}
