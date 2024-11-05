using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public enum UILayer
    {
        Normal,
        Message,
        Top,
        World
    }

    public class UIRoot : MonoBehaviour
    {
        [SerializeField]
        private Transform worldLayer;

        [SerializeField]
        private Transform normalLayer;

        [SerializeField]
        private Transform messageLayer;

        [SerializeField]
        private Transform topLayer;

        public Transform WorldLayer => worldLayer;
        public Transform NormalLayer => normalLayer;
        public Transform MessageLayer => messageLayer;
        public Transform TopLayer => topLayer;

        internal void AttachToLayer(GameObject uiGo, UILayer uILayer)
        {
            Transform layerTra = null;
            switch (uILayer)
            {
                case UILayer.Normal:
                    layerTra = normalLayer;
                    break;
                case UILayer.Message:
                    layerTra = messageLayer;
                    break;
                case UILayer.Top:
                    layerTra = topLayer;
                    break;
                case UILayer.World:
                    layerTra = worldLayer;
                    break;
                default:
                    throw new Exception("");
            }
            uiGo.transform.SetParent(layerTra, false);
        }
    }

}