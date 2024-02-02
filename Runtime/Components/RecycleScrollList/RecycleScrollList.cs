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
        private GridLayoutGroup m_GridLayout;
        public GridLayoutGroup GridLayout => m_GridLayout;

        [SerializeField]
        private GameObject m_RecycleItemTemplate;
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
        private bool m_AutoCalculateConstraintCount = true;

        [SerializeField]
        private uint m_Count;
        public uint Count => m_Count;

        [ContextMenu("预览")]
        private void Preview()
        {
            SetupLayout();

            for (int i = 0; i < Count; i++)
            {
                var item = GameObject.Instantiate(m_RecycleItemTemplate, m_GridLayout.transform);
            }
        }

        private void SetupLayout()
        {
            if (m_SrollRect == null || m_GridLayout == null)
                return;

            var gridLayoutRectTra = m_GridLayout.GetComponent<RectTransform>();
            m_SrollRect.vertical = m_ScrollType == ScrollType.Vertical;
            m_SrollRect.horizontal = m_ScrollType ==  ScrollType.Horizontal;
            
            if (m_ScrollType == ScrollType.Vertical)
            {
                if (m_DirectionV == VerticalDirection.TopToBottom)
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 1f);//Anchor:top without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 1f);//Pivot:left top
                }
                else
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 0f);//Anchor:bottom without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 0f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 0f);//Pivot:left bottom
                }

                m_GridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
                m_GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

                if (m_AutoCalculateConstraintCount)//计算每行多少列
                    m_GridLayout.constraintCount = CalculateConstraintCount_Col();


                float length = CalculateScrollViewLength();
                gridLayoutRectTra.sizeDelta = new Vector2(0f, length);
            }
            else
            {
                m_GridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
                m_GridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;

                if (m_DirectionH == HorizontalDirection.LeftToRight)
                {
                    gridLayoutRectTra.anchorMin = new Vector2(0f, 0f);//Anchor:left without strech
                    gridLayoutRectTra.anchorMax = new Vector2(0f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(0f, 1f);//Pivot:left top
                }
                else
                {
                    gridLayoutRectTra.anchorMin = new Vector2(1f, 0f);//Anchor:right without strech
                    gridLayoutRectTra.anchorMax = new Vector2(1f, 1f);
                    gridLayoutRectTra.pivot = new Vector2(1f, 1f);//Pivot:right top
                }

                m_GridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
                m_GridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;

                if (m_AutoCalculateConstraintCount)//计算每列多少行
                    m_GridLayout.constraintCount = CalculateConstraintCount_Row();

                float length = CalculateScrollViewLength();
                gridLayoutRectTra.sizeDelta = new Vector2(length, 0f);
            }
        }

        private int CalculateConstraintCount_Col()
        {
            var gridLayoutRectTra = m_GridLayout.GetComponent<RectTransform>();
            float width = gridLayoutRectTra.rect.width;

            //copy from sourcecode
            return Mathf.Max(1, Mathf.FloorToInt((width - m_GridLayout.padding.horizontal + m_GridLayout.spacing.x + 0.001f) / (m_GridLayout.cellSize.x + m_GridLayout.spacing.x)));
        }

        private int CalculateConstraintCount_Row() 
        {
            var gridLayoutRectTra = m_GridLayout.GetComponent<RectTransform>();
            float height = gridLayoutRectTra.rect.height;

            //copy from sourcecode
            return Mathf.Max(1, Mathf.FloorToInt((height - m_GridLayout.padding.vertical + m_GridLayout.spacing.y + 0.001f) / (m_GridLayout.cellSize.y + m_GridLayout.spacing.y)));
        }

        private float CalculateScrollViewLength()
        {

            if (m_ScrollType == ScrollType.Vertical)
            {
                float cellSizeAndSpaceY = GetCellSizeAndSpaceY();
                float rowCount = Mathf.Ceil((float)m_Count / m_GridLayout.constraintCount);
                return m_GridLayout.padding.vertical + m_GridLayout.spacing.y + 0.001f + rowCount * cellSizeAndSpaceY;
            }
            else
            {
                float cellSizeAndSpaceX = GetCellSizeAndSpaceX();
                float colCount = Mathf.Ceil((float)m_Count / m_GridLayout.constraintCount);
                return m_GridLayout.padding.horizontal + m_GridLayout.spacing.x + 0.001f +colCount * cellSizeAndSpaceX;
            }
        }

        private float GetCellSizeAndSpaceY()
        {
            return m_GridLayout.cellSize.y + m_GridLayout.spacing.y;
        }

        private float GetCellSizeAndSpaceX() 
        {
            return m_GridLayout.cellSize.x + m_GridLayout.spacing.x;
        }
    }

}