using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public class UIPanelHistory
    {
        private LinkedList<UIPanel> m_Panels = new LinkedList<UIPanel>();
        private Dictionary<string, UIPanel> m_Name2Panel = new Dictionary<string, UIPanel>();

        public int Count
        {
            get
            {
                return m_Panels.Count;
            }
        }

        public UIPanelHistory()
        {
            m_Panels = new LinkedList<UIPanel>();
            m_Name2Panel = new Dictionary<string, UIPanel>();
        }


        public void Push(UIPanel uiPanel)
        {
            if (uiPanel != null)
            {
                m_Name2Panel.Add(uiPanel.Name, uiPanel);
                m_Panels.AddFirst(uiPanel);
            }
        }

        public UIPanel Pop()
        {
            if (Count > 0)
            {
                LinkedListNode<UIPanel> current = m_Panels.First;

                m_Name2Panel.Remove(current.Value.Name);
                m_Panels.Remove(current);

                return current.Value;
            }
            return null;
        }

        public UIPanel Pop(string name)
        {
            if (Count > 0)
            {
                LinkedListNode<UIPanel> current = m_Panels.First;

                while (current != null)
                {
                    LinkedListNode<UIPanel> next = current.Next;

                    //从字典/链表中移除
                    m_Name2Panel.Remove(current.Value.Name);
                    m_Panels.Remove(current);

                    //若为目标界面，则返回（链表中同一时刻同一界面只有一个）
                    if (current.Value.Name.Equals(name))
                    {
                        return current.Value;
                    }
                    current = next;
                }
            }

            return null;
        }

        public UIPanel Peek()
        {
            if (Count > 0)
            {
                return m_Panels.First.Value;
            }
            return null;
        }

        public bool Contains(string name)
        {
            return m_Name2Panel.ContainsKey(name);
        }

        public UIPanel GetPanel(string name)
        {
            if (m_Name2Panel.TryGetValue(name, out UIPanel uiPanel))
            {
                return uiPanel;
            }
            return null;
        }

        public void Clear()
        {
            m_Panels.Clear();
            m_Name2Panel.Clear();
        }
    }
}
