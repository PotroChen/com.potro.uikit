using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public static class  UIUtils
    {
        public static void SetActive(GameObject go,bool active,bool mute = false)
        {
            SetActive(go, null, active, mute);
        }

        public static void SetActive(GameObject go,string nodePath,bool active,bool mute = false)
        {
            if (go == null)
            {
                if (!mute)
                {
                    Debug.LogError("[SetActive] go == null");
                }
                return;
            }

            GameObject nodeGo = FindGo_Internal(go, nodePath);
            if (nodeGo == null)
            {
                if (!mute)
                {
                    Debug.LogError($"[SetActive] can not find node with nodePath {nodePath}");
                }
                return;
            }

            nodeGo.SetActive(active);
        }

        public static void SetImage(GameObject go, Sprite sprite, bool mute = false)
        {
            SetImage(go, null, sprite, mute);
        }

        public static void SetImage(GameObject go,string nodePath, Sprite sprite, bool mute = false)
        {
            if (go == null)
            {
                if (!mute)
                {
                    Debug.LogError("[SetImage] go == null");
                }
                return;
            }

            GameObject nodeGo = FindGo_Internal(go, nodePath);
            if (nodeGo == null)
            {
                if (!mute)
                {
                    Debug.LogError($"[SetImage] can not find node with nodePath {nodePath}");
                }
                return;
            }

            var img = nodeGo.GetComponent<Image>();
            if (img == null)
            {
                if (!mute)
                {
                    if (!string.IsNullOrEmpty(nodePath))
                        Debug.LogError($"[SetImage] node {go.name} path {nodePath} has no image component");
                    else
                        Debug.LogError($"[SetImage] node {go.name} has no image component");
                }
                return;
            }
            img.sprite = sprite;

        }

        public static void SetText(GameObject go, string text, bool mute = false)
        {
            SetText(go, null, text, mute);
        }

        public static void SetText(GameObject go, string nodePath, string text, bool mute = false)
        {
            if (go == null)
            {
                if (!mute)
                {
                    Debug.LogError("[SetText] go == null");
                }
                return;
            }

            GameObject nodeGo = FindGo_Internal(go, nodePath);
            if (nodeGo == null)
            {
                if (!mute)
                {
                    Debug.LogError($"[SetText] can not find node with nodePath {nodePath}");
                }
                return;
            }

            var textCom = nodeGo.GetComponent<Text>();
            if (textCom == null)
            {
                if (!mute)
                {
                    if (!string.IsNullOrEmpty(nodePath))
                        Debug.LogError($"[SetImage] node {go.name} path {nodePath} has no Text component");
                    else
                        Debug.LogError($"[SetImage] node {go.name} has no Text component");
                }
                return;
            }
            textCom.text = text;
        }

        private static GameObject FindGo_Internal(GameObject go, string nodePath)
        {
            GameObject nodeGo = null;
            if (!string.IsNullOrEmpty(nodePath))
            {
                nodeGo = go.transform.Find(nodePath).gameObject;
            }
            else
            {
                nodeGo = go;
            }
            return nodeGo;
        }

    }

}
