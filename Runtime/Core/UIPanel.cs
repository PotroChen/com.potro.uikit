using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.UIKit
{
    public class UIPanel
    {
        public string Name { get; private set; }

        protected ResLoader resLoader = new ResLoader();

        public struct PanelConfig
        {
            /// <summary>
            /// 界面Prefab资源路径（要求全路径）
            /// </summary>
            public string PrefabPath;

            /// <summary>
            /// UI层级
            /// </summary>
            public UILayer UILayer;
        }

        //界面配置数据
        protected virtual PanelConfig ConfigData { get; } = default;

        //界面初始化数据
        protected IData m_InitData = null;

        protected GameObject m_Root;

        public UIPanel()
        {
            Name = GetName(this);
        }


        internal virtual void Load(IData initData)
        {
            if (initData != null)
            {
                m_InitData = initData;
            }
            OnInit(m_InitData);

            void OnLoadCompleted(GameObject assetObject)
            {
                m_Root = GameObject.Instantiate(assetObject);
                UIManager.AttachToLayer(m_Root, ConfigData.UILayer);
                OnLoaded();
                if(m_Root.activeSelf)
                    OnShow();
            }
            resLoader.LoadAssetAsync<GameObject>(ConfigData.PrefabPath, OnLoadCompleted);
        }

        internal virtual void Purge()
        {
            if (m_Root.activeSelf)
                OnHide();
            OnPurge();
            Object.Destroy(m_Root);
            m_Root = null;

            resLoader.ReleaseAllAssets();
        }

        /*
         * Show/Hide暂时是用gameObject.SetActive方式
         * 其实还有移出屏幕和CanvasGroup.alpha = 0
         * 后续再更改
         */

        public void Show()
        {
            if (m_Root.activeSelf)
                return;
            m_Root.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            if (!m_Root.activeSelf)
                return;
            m_Root.SetActive(false);
            OnHide();
        }

        protected virtual void OnInit(IData data) { }

        protected virtual void OnLoaded() { }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        protected virtual void OnPurge() { }


        public static string GetName(UIPanel panel)
        {
            return panel.GetType().Name;
        }

        public static string GetName<TPanel>() where TPanel : UIPanel
        {
            return typeof(TPanel).Name;
        }
    }

}