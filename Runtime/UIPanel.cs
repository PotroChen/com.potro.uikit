using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.UIKit
{
    public abstract class UIPanel
    {
        public string Name { get; private set; }

        protected ResLoader resLoader;

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
        protected abstract PanelConfig ConfigData{get;}

        //界面初始化数据
        protected object m_InitData = null;

        protected GameObject m_Root;

        public UIPanel()
        {
            Name = GetName(this);
        }


        internal virtual void Load(object initData)
        {
            if (initData != null)
            {
                m_InitData = initData;
            }
            OnLoad(m_InitData);

            void OnLoadCompleted(GameObject assetObject)
            {
                m_Root = GameObject.Instantiate(assetObject);
                UIManager.AttachToLayer(m_Root, ConfigData.UILayer);
                OnCreated();
            }
            resLoader.LoadAssetAsync<GameObject>(ConfigData.PrefabPath, OnLoadCompleted);
        }

        internal virtual void Purge()
        {
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

        protected abstract void OnLoad(object data);

        protected abstract void OnCreated();

        protected abstract void OnShow();

        protected abstract void OnHide();

        protected abstract void OnPurge();


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