using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    public class RecycleScrollList : MonoBehaviour
    {
        public enum ScrollType
        {
            Vertical,
            Horizontal
        }

        public enum VerticalDirection
        {
            TopToBottom,
            BottomToTop,
        }

        public enum HorizontalDirection
        {
            LeftToRight,
            RightToLeft
        }

        [SerializeField]
        private ScrollRect m_SrollRect;
        public ScrollRect ScrollRect => m_SrollRect;

        [SerializeField]
        private GridLayoutGroup m_gridLayout;
        public GridLayoutGroup GridLayout => m_gridLayout;

        [SerializeField]
        public GameObject m_RecycleItemTemplate;
        public GameObject RecycleItemTemplate => m_RecycleItemTemplate;

        [SerializeField]
        private ScrollType m_ScrollType = ScrollType.Vertical;
        /// <summary>
        /// 滚动列表类型
        /// </summary>
        public ScrollType ScrollDirection => m_ScrollType;

        [SerializeField]
        private VerticalDirection m_DirectionV = VerticalDirection.TopToBottom;
        public VerticalDirection DirectionV => m_DirectionV;

        [SerializeField]
        private HorizontalDirection m_DirectionH = HorizontalDirection.LeftToRight;
        public HorizontalDirection DirectionH => m_DirectionH;

        [SerializeField]
        private uint m_Count;
        public uint Count => m_Count;

        [ContextMenu("预览")]
        private void Refresh()
        {
            
        }

        private void SetupLayout()
        {
            if (m_SrollRect == null || m_gridLayout == null)
                return;

            var gridLayoutRectTra = m_gridLayout.GetComponent<RectTransform>();
            m_SrollRect.vertical = m_ScrollType == ScrollType.Vertical;
            m_SrollRect.horizontal = m_ScrollType ==  ScrollType.Horizontal;
            float length = CalculateScrollViewLength();
            if (m_ScrollType == ScrollType.Vertical)
            {
                m_gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
                if (m_DirectionV == VerticalDirection.TopToBottom)
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 1f);//Anchor:top without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 1f);//Pivot:left top
                    gridLayoutRectTra.sizeDelta = new Vector2(0f, length);
                }
                else
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 0f);//Anchor:bottom without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 0f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 0f);//Pivot:left bottom
                    gridLayoutRectTra.sizeDelta = new Vector2(0f, length);
                }
            }
            else
            {
                m_gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
                if (m_DirectionH == HorizontalDirection.LeftToRight)
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 0f);//Anchor:left without strech
                    gridLayoutRectTra.anchorMax = new Vector2(0f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 1f);//Pivot:left top
                    gridLayoutRectTra.sizeDelta = new Vector2(length, 0f);
                }
                else
                {
                    gridLayoutRectTra.anchorMin = new Vector2(1f, 0f);//Anchor:right without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(1f, 1f);//Pivot:right top
                    gridLayoutRectTra.sizeDelta = new Vector2(length, 0f);
                }
            }
        }

        private float CalculateScrollViewLength()
        {
            if (m_ScrollType == ScrollType.Vertical)
            {
                float cellSizeAndSpaceY = GetCellSizeAndSpaceY();
                float rowCount = Mathf.Ceil((float)m_Count / m_gridLayout.constraintCount);
                return m_gridLayout.padding.top + m_gridLayout.padding.bottom + rowCount * cellSizeAndSpaceY;
            }
            else
            {
                float cellSizeAndSpaceX = GetCellSizeAndSpaceX();
                float colCount = Mathf.Ceil((float)m_Count / m_gridLayout.constraintCount);
                return m_gridLayout.padding.left + m_gridLayout.padding.right + colCount * cellSizeAndSpaceX;
            }
        }

        private float GetCellSizeAndSpaceY()
        {
            return m_gridLayout.cellSize.y + m_gridLayout.spacing.y;
        }

        private float GetCellSizeAndSpaceX() 
        {
            return m_gridLayout.cellSize.x + m_gridLayout.spacing.x;
        }
    }

}