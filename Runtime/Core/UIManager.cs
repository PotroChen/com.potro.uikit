using UnityEngine;


namespace GameFramework.UIKit
{
    public class UIManager : MonoBehaviour
    {
        public static UIRoot UIRoot => instance.m_uiRoot;
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
        public static void Goto<TPanel>(IData data = null) where TPanel : UIPanel, new()
        {
            instance.Goto_Internal<TPanel>(data);
        }

        /// <summary>
        /// 返回上一界面
        /// </summary>
        public static void Goback(IData data = null)
        {
            instance.Goback_internal(data);
        }

        /// <summary>
        /// 打开一个面板（独立于 Goto/GoBack 的管理方式）。
        /// 若该面板已被管理，则打开失败并输出警告。
        /// </summary>
        public static void Open<TPanel>(IData data = null) where TPanel : UIPanel, new()
        {
            instance.Open_Internal<TPanel>(data);
        }

        /// <summary>
        /// 关闭一个面板（独立于 Goto/GoBack 的管理方式）。
        /// 只关闭自身并从窗口栈中移除，不影响其他面板。
        /// </summary>
        public static void Close<TPanel>() where TPanel : UIPanel
        {
            instance.Close_Internal<TPanel>();
        }

        internal static void AttachToLayer(GameObject uiGo, UILayer uILayer)
        {
            instance.m_uiRoot.AttachToLayer(uiGo, uILayer);
        }

        private UIPanel m_CurrentPanel;
        //窗口堆栈
        private UIPanelStack m_PanelStack = new UIPanelStack();
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

        private void Goto_Internal<TPanel>(IData data) where TPanel : UIPanel, new()
        {
            string panelName = UIPanel.GetName<TPanel>();

            if (CurrentPanel != null && CurrentPanel.Name.Equals(panelName))
            {
                return;
            }

            bool isNewPanel = false;

            UIPanel nextPanel = null;
            if (m_PanelStack.Contains(panelName))
            {
                nextPanel = m_PanelStack.Pop(panelName);
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
                if(!previousPanel.IsPermanent)
                    previousPanel.Purge();

                if (isNewPanel)
                {
                    m_PanelStack.Push(previousPanel);
                }
            }

            CurrentPanel = nextPanel;
        }

        /// <summary>
        /// 返回上一界面
        /// </summary>
        private void Goback_internal(IData data = null)
        {
            if (CurrentPanel == null)
            {
                return;
            }

            UIPanel nextPanel = null;
            if (m_PanelStack.Count > 0)
            {
                nextPanel = m_PanelStack.Pop();
            }

            if (nextPanel != null && !nextPanel.IsPermanent)
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

        private void Open_Internal<TPanel>(IData data) where TPanel : UIPanel, new()
        {
            string panelName = UIPanel.GetName<TPanel>();

            // 已被管理（在栈中或是当前主面板）则打开失败
            if (m_PanelStack.Contains(panelName) || (CurrentPanel != null && CurrentPanel.Name.Equals(panelName)))
            {
                Debug.LogWarning($"[UIManager] Open failed: panel '{panelName}' is already open.");
                return;
            }

            UIPanel panel = new TPanel();
            panel.Load(data);
            m_PanelStack.Push(panel);
        }

        private void Close_Internal<TPanel>() where TPanel : UIPanel
        {
            string panelName = UIPanel.GetName<TPanel>();

            UIPanel panel = m_PanelStack.Remove(panelName);
            if (panel != null)
            {
                panel.Purge();
                return;
            }

            Debug.LogWarning($"[UIManager] Close failed: panel '{panelName}' is not managed by Open/Close.");
        }
    }

}
