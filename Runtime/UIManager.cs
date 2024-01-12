using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace GameFramework.UIKit
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// 当前打开的主面板（一级界面）
        /// </summary>
        public static UIPanel CurrentPanel 
        {
            get { return instance.m_CurrentPanel; }
            private set { instance.m_CurrentPanel = value; }
        }

        private static UIManager instance = null;

        /// <summary>
        /// 切换下一个界面
        /// </summary>
        public static void Goto<TPanel>(object data = null) where TPanel : UIPanel, new()
        {
            instance.Goto_Internal<TPanel>(data);
        }

        /// <summary>
        /// 返回上一界面
        /// </summary>
        public static void Goback(object data = null)
        {
            instance.Goback_internal(data);
        }

        internal static void AttachToLayer(GameObject uiGo, UILayer uILayer)
        {
            instance.m_uiRoot.AttachToLayer(uiGo, uILayer);
        }

        private UIPanel m_CurrentPanel;
        //界面切换堆栈
        private UIPanelHistory m_PanelHistory = new UIPanelHistory();
        //UIRoot
        [SerializeField]
        private UIRoot m_uiRoot;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("GameRuntime has existed");
                DestroyImmediate(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        private void Goto_Internal<TPanel>(object data) where TPanel : UIPanel, new()
        {
            string panelName = UIPanel.GetName<TPanel>();

            if (CurrentPanel != null && CurrentPanel.Name.Equals(panelName))
            {
                return;
            }

            bool isNewPanel = false;

            UIPanel nextPanel = null;
            if (m_PanelHistory.Contains(panelName))
            {
                nextPanel = m_PanelHistory.Pop(panelName);
            }
            else
            {
                nextPanel = new TPanel();
                isNewPanel = true;
            }

            if (nextPanel != null)
            {
                nextPanel.Load(data);
            }

            UIPanel previousPanel = CurrentPanel;
            if (previousPanel != null)
            {
                previousPanel.Purge();

                if (isNewPanel)
                {
                    m_PanelHistory.Push(previousPanel);
                }
            }

            CurrentPanel = nextPanel;
        }

        /// <summary>
        /// 返回上一界面
        /// </summary>
        private void Goback_internal(object data = null)
        {
            if (CurrentPanel == null)
            {
                return;
            }

            UIPanel nextPanel = null;
            if (m_PanelHistory.Count > 0)
            {
                nextPanel = m_PanelHistory.Pop();
            }

            if (nextPanel != null)
            {
                nextPanel.Load(data);
            }

            UIPanel previousPanel = CurrentPanel;
            if (previousPanel != null)
            {
                previousPanel.Purge();
            }

            CurrentPanel = nextPanel;
        }
    }

}