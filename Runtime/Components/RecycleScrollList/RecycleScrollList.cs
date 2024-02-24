using System.Collections;
using System.Collections.Generic;
using GluonGui;
using UnityEngine;
using UnityEngine.Pool;
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
        private int m_Count;
        public int Count => m_Count;

        private int m_ViewportMaxItemNumber;
        private float m_CellSizeAndSpaceX;
        private float m_CellSizeAndSpaceY;
        private GameObjectPool m_ItemPool;
        private List<IPoolObject<GameObject>> m_ActiveItems = new List<IPoolObject<GameObject>>();

        public void FillGrid(int count)
        {
            m_Count = count;
            Init();
            Refresh();
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            if (m_ItemPool != null)
            {
                m_ItemPool.Dispose();
            }
        }

        private void Init()
        {
            ScrollRect.onValueChanged.RemoveListener(OnValueChange);
            ScrollRect.onValueChanged.AddListener(OnValueChange);

            m_CellSizeAndSpaceX = GetCellSizeAndSpaceX();
            m_CellSizeAndSpaceY = GetCellSizeAndSpaceY();
            
            //计算并设置锚点和长宽
            SetupLayout();

            m_ViewportMaxItemNumber = GetMaxActiveItemNumber();

            //初始化对象池
            int cachedNumber = m_Count < m_ViewportMaxItemNumber ? m_Count : m_ViewportMaxItemNumber;

            if (m_ItemPool == null)
                m_ItemPool = new GameObjectPool(m_RecycleItemTemplate, cachedNumber, m_GridLayout.transform);
            else
                m_ItemPool.MaxSize = cachedNumber;
            m_ItemPool.WarmUp();

        }

        private void OnValueChange(Vector2 position)
        {
            Refresh();
        }

        private void Refresh()
        {
            int activeCount = 0;
            int startRowOrColIndex = 0;
            int endRowOrColIndex = 0;
            //计算startRowOrColIndex
            if (m_ScrollType == ScrollType.Vertical)//垂直
            {
                float inverseCellSizeAndSpaceY = 1 / m_CellSizeAndSpaceY;
                if (m_DirectionV == VerticalDirection.TopToBottom)//从上到下
                {
                    startRowOrColIndex = Mathf.FloorToInt(m_SrollRect.content.anchoredPosition.y * inverseCellSizeAndSpaceY);
                }
                else//从下到上
                {
                    startRowOrColIndex = Mathf.FloorToInt(-m_SrollRect.content.anchoredPosition.y * inverseCellSizeAndSpaceY);
                }
            }
            else//水平方向
            {
                float inverseCellSizeAndSpaceX = 1 / m_CellSizeAndSpaceX;
                if (m_DirectionH == HorizontalDirection.LeftToRight)
                {
                    startRowOrColIndex = Mathf.FloorToInt(-m_SrollRect.content.anchoredPosition.x * inverseCellSizeAndSpaceX);
                }
                else
                {
                    startRowOrColIndex = Mathf.FloorToInt(m_SrollRect.content.anchoredPosition.x * inverseCellSizeAndSpaceX);
                }
            }
            startRowOrColIndex = Mathf.Max(startRowOrColIndex, 0);//为了处理在0行(或列)反向拖拽的情况
            //计算激活Item
            int startItemIndex = startRowOrColIndex * m_GridLayout.constraintCount;
            if (startItemIndex > m_Count - 1)
            {
                activeCount = 0;
            }
            else
            {
                activeCount = m_Count - startItemIndex;//等价于(m_Count-1) - startItemIndex + 1
                activeCount = Mathf.Min(activeCount, m_ViewportMaxItemNumber);
            }

            //激活Item
            for (int i = 0; i < m_ActiveItems.Count; i++)
            {
                m_ActiveItems[i].Recycle();
            }
            m_ActiveItems.Clear();
            for (int i = 0; i < activeCount; i++)
            {
                m_ActiveItems.Add(m_ItemPool.Get(m_SrollRect.content));
            }
            //更新位置
            for (int i = 0; i < activeCount; i++)
            {
                RefreshItemPosition((RectTransform)m_ActiveItems[i].Content.transform, i, startRowOrColIndex, m_ScrollType, m_DirectionV, m_DirectionH);
            }

        }

        //垂直方向暂时不支持选择水平方向（新行未满时，Item是从哪个方向出现）
        //水平方向暂时不支持选择垂直方向（新行未满时，Item是从哪个方向出现）
        private void RefreshItemPosition(RectTransform item,int itemIndex,int startRowOrColIndex, ScrollType scrollType, VerticalDirection verticalDirection,HorizontalDirection horizontalDirection)
        {
            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.pivot = Vector2.one * 0.5f;//同一处理,便于计算

            var padding = m_GridLayout.padding;
            float beginX = 0f;
            float beginY = 0f;

            float positionX = 0f;
            float positionY = 0f;

            int indexInRowOrCol = itemIndex % m_GridLayout.constraintCount;
            int rowOrColIndex = startRowOrColIndex + itemIndex / m_GridLayout.constraintCount;
            if (scrollType == ScrollType.Vertical)//垂直
            {
                beginX = padding.left + m_GridLayout.cellSize.x * 0.5f;//默认是从左边开始
                positionX = beginX + indexInRowOrCol * m_CellSizeAndSpaceX;

                if (verticalDirection == VerticalDirection.TopToBottom)//从上到下
                {
                    beginY = -(padding.top + m_GridLayout.cellSize.y * 0.5f);
                    positionY = beginY - rowOrColIndex * m_CellSizeAndSpaceY;
                }
                else//从下到上
                {
                    beginY = -(m_SrollRect.content.rect.height - padding.bottom - m_GridLayout.cellSize.y * 0.5f);
                    positionY = beginY + rowOrColIndex * m_CellSizeAndSpaceY;
                }
            }
            else
            {
                beginY = -(padding.top + m_GridLayout.cellSize.y * 0.5f);//默认是从上面
                positionY = beginY - indexInRowOrCol * m_CellSizeAndSpaceY;
                if (horizontalDirection == HorizontalDirection.LeftToRight)//从左到右
                {
                    beginX = padding.left + m_GridLayout.cellSize.x * 0.5f;
                    positionX = beginX +  rowOrColIndex * m_CellSizeAndSpaceX;
                }
                else//从右到左
                {
                    beginX = m_SrollRect.content.rect.width - padding.right - m_GridLayout.cellSize.x * 0.5f;
                    positionX = beginX - rowOrColIndex * m_CellSizeAndSpaceX;
                }
            }
            item.anchoredPosition = new Vector2(positionX, positionY);
        }

        #region SetupLayout(计算并设置锚点和长宽)
        //计算并设置锚点和长宽
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
                gridLayoutRectTra.sizeDelta = new Vector2(0f, 0f);
                gridLayoutRectTra.ForceUpdateRectTransforms();//强制Update使宽度正确

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
                gridLayoutRectTra.sizeDelta = new Vector2(0f, 0f);
                gridLayoutRectTra.ForceUpdateRectTransforms();//强制Update使高度正确

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
                float cellSizeAndSpaceY = m_CellSizeAndSpaceY;
                float rowCount = Mathf.Ceil((float)m_Count / m_GridLayout.constraintCount);
                return m_GridLayout.padding.vertical + m_GridLayout.spacing.y + 0.001f + rowCount * cellSizeAndSpaceY;
            }
            else
            {
                float cellSizeAndSpaceX = m_CellSizeAndSpaceX;
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
        #endregion

        //计算Viewport区域最大显示的Item数量
        private int GetMaxActiveItemNumber()
        {
            if (m_ScrollType == ScrollType.Vertical)
            {
                float height = ScrollRect.viewport.rect.height;
                int rowCount = Mathf.CeilToInt(height / m_CellSizeAndSpaceY) +1;//加一是滑动过程中，第一行(或者列)没有消失，但是第二列却要显示的问题
                return m_GridLayout.constraintCount * rowCount;
            }
            else
            {
                float width = ScrollRect.viewport.rect.width;
                int colCount = Mathf.CeilToInt(width / m_CellSizeAndSpaceX) +1;//加一是滑动过程中，第一行(或者列)没有消失，但是第二列却要显示的问题
                return m_GridLayout.constraintCount * colCount;
            }
        }

#if UNITY_EDITOR
        //private void OnValidate()
        //{
        //    Init();
        //}

        [ContextMenu("预览")]
        private void Preview()
        {
            SetupLayout();

            for (int i = 0; i < Count; i++)
            {
                var item = GameObject.Instantiate(m_RecycleItemTemplate, m_GridLayout.transform);
            }
        }
#endif
    }

}