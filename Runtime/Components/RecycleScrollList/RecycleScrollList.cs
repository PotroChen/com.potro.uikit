using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    /// <summary>
    /// ����ѭ�������б�
    /// </summary>
    [AddComponentMenu("UI���/����ѭ�������б�(RecycleScrollList)")]
    public class RecycleScrollList : ScrollRect
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

        #region GridLayoutProperties

        [SerializeField]
        private RectOffset m_Padding = new RectOffset();
        public RectOffset Padding => m_Padding;

        [SerializeField]
        private Vector2 m_CellSize;
        public Vector2 CellSize => m_CellSize;
        [SerializeField]
        private Vector2 m_Spacing;
        public Vector2 Spacing => m_Spacing;

        private int m_ConstraintCount;
        #endregion

        [SerializeField]
        private GameObject m_RecycleItemTemplate;
        public GameObject RecycleItemTemplate => m_RecycleItemTemplate;


        //[SerializeField]
        //private ScrollType ScrollDirection = ScrollType.Vertical;
        /// <summary>
        /// �����б�����
        /// </summary>
        public ScrollType ScrollDirection
        {
            get
            {
                if(horizontal == vertical)
                    return ScrollType.Vertical;

                return horizontal == true ? ScrollType.Horizontal : ScrollType.Vertical;
            }
        }

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

        public event Action<int, GameObject> OnItemRefresh;

        private int m_ViewportMaxItemNumber;
        private float m_CellSizeAndSpaceX;
        private float m_CellSizeAndSpaceY;
        private GameObjectPool m_ItemPool;
        private List<IPoolObject<GameObject>> m_ActiveItems = new List<IPoolObject<GameObject>>();
        private RectTransform m_RectTransform;
        private int lastStartRowOrColIndex = -1;
        public void FillGrid(int count)
        {
            lastStartRowOrColIndex = -1;
            m_Count = count;
            Init();
            Refresh();
        }

        protected override void Start()
        {
            base.Start();
            if(Application.isPlaying)
                Init();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (m_ItemPool != null)
            {
                m_ItemPool.Dispose();
            }
        }

        private void Init()
        {
            onValueChanged.RemoveListener(OnValueChange);
            onValueChanged.AddListener(OnValueChange);

            m_CellSizeAndSpaceX = GetCellSizeAndSpaceX();
            m_CellSizeAndSpaceY = GetCellSizeAndSpaceY();
            
            //���㲢����ê��ͳ���
            SetupLayout();

            m_ViewportMaxItemNumber = GetMaxActiveItemNumber();

            //��ʼ�������
            int cachedNumber = m_Count < m_ViewportMaxItemNumber ? m_Count : m_ViewportMaxItemNumber;

            if (m_ItemPool == null)
                m_ItemPool = new GameObjectPool(m_RecycleItemTemplate, cachedNumber, content);
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
            //����startRowOrColIndex
            if (ScrollDirection == ScrollType.Vertical)//��ֱ
            {
                float inverseCellSizeAndSpaceY = 1 / m_CellSizeAndSpaceY;
                if (m_DirectionV == VerticalDirection.TopToBottom)//���ϵ���
                {
                    startRowOrColIndex = Mathf.FloorToInt(content.anchoredPosition.y * inverseCellSizeAndSpaceY);
                }
                else//���µ���
                {
                    startRowOrColIndex = Mathf.FloorToInt(-content.anchoredPosition.y * inverseCellSizeAndSpaceY);
                }
            }
            else//ˮƽ����
            {
                float inverseCellSizeAndSpaceX = 1 / m_CellSizeAndSpaceX;
                if (m_DirectionH == HorizontalDirection.LeftToRight)
                {
                    startRowOrColIndex = Mathf.FloorToInt(-content.anchoredPosition.x * inverseCellSizeAndSpaceX);
                }
                else
                {
                    startRowOrColIndex = Mathf.FloorToInt(content.anchoredPosition.x * inverseCellSizeAndSpaceX);
                }
            }
            startRowOrColIndex = Mathf.Max(startRowOrColIndex, 0);//Ϊ�˴�����0��(����)������ק�����
            //���㼤��Item
            int startItemIndex = startRowOrColIndex * m_ConstraintCount;
            if (startItemIndex > m_Count - 1)
            {
                activeCount = 0;
            }
            else
            {
                activeCount = m_Count - startItemIndex;//�ȼ���(m_Count-1) - startItemIndex + 1
                activeCount = Mathf.Min(activeCount, m_ViewportMaxItemNumber);
            }

            if (lastStartRowOrColIndex != startRowOrColIndex)//������������ʾ��Itemû�仯,��û��Ҫ���¼���
            {
                //����Item
                for (int i = 0; i < m_ActiveItems.Count; i++)
                {
                    m_ActiveItems[i].Recycle();
                }
                m_ActiveItems.Clear();
                for (int i = 0; i < activeCount; i++)
                {
                    m_ActiveItems.Add(m_ItemPool.Get(content));
                }
            }


            //����λ��
            for (int i = 0; i < activeCount; i++)
            {
                RefreshItemPosition((RectTransform)m_ActiveItems[i].Content.transform, i, startRowOrColIndex, ScrollDirection, m_DirectionV, m_DirectionH);
            }

            if (lastStartRowOrColIndex != startRowOrColIndex)// ������������ʾ��Itemû�仯,��û��Ҫ���µ���ˢ�»ص�
            {
                //�����ص�
                for (int i = 0; i < activeCount; i++)
                {
                    int itemIndex = startItemIndex + i;
                    OnItemRefresh?.Invoke(itemIndex, m_ActiveItems[i].Content);
                }
            }
            lastStartRowOrColIndex = startRowOrColIndex;

        }

        //��ֱ������ʱ��֧��ѡ��ˮƽ��������δ��ʱ��Item�Ǵ��ĸ�������֣�
        //ˮƽ������ʱ��֧��ѡ��ֱ��������δ��ʱ��Item�Ǵ��ĸ�������֣�
        private void RefreshItemPosition(RectTransform item,int itemIndex,int startRowOrColIndex, ScrollType scrollType, VerticalDirection verticalDirection,HorizontalDirection horizontalDirection)
        {
            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.pivot = Vector2.one * 0.5f;//ͬһ����,���ڼ���

            var padding = m_Padding;
            float beginX = 0f;
            float beginY = 0f;

            float positionX = 0f;
            float positionY = 0f;

            int indexInRowOrCol = itemIndex % m_ConstraintCount;
            int rowOrColIndex = startRowOrColIndex + itemIndex / m_ConstraintCount;
            if (scrollType == ScrollType.Vertical)//��ֱ
            {
                beginX = padding.left + m_CellSize.x * 0.5f;//Ĭ���Ǵ���߿�ʼ
                positionX = beginX + indexInRowOrCol * m_CellSizeAndSpaceX;

                if (verticalDirection == VerticalDirection.TopToBottom)//���ϵ���
                {
                    beginY = -(padding.top + m_CellSize.y * 0.5f);
                    positionY = beginY - rowOrColIndex * m_CellSizeAndSpaceY;
                }
                else//���µ���
                {
                    beginY = -(content.rect.height - padding.bottom - m_CellSize.y * 0.5f);
                    positionY = beginY + rowOrColIndex * m_CellSizeAndSpaceY;
                }
            }
            else
            {
                beginY = -(padding.top + m_CellSize.y * 0.5f);//Ĭ���Ǵ�����
                positionY = beginY - indexInRowOrCol * m_CellSizeAndSpaceY;
                if (horizontalDirection == HorizontalDirection.LeftToRight)//������
                {
                    beginX = padding.left + m_CellSize.x * 0.5f;
                    positionX = beginX +  rowOrColIndex * m_CellSizeAndSpaceX;
                }
                else//���ҵ���
                {
                    beginX = content.rect.width - padding.right - m_CellSize.x * 0.5f;
                    positionX = beginX - rowOrColIndex * m_CellSizeAndSpaceX;
                }
            }
            item.anchoredPosition = new Vector2(positionX, positionY);
        }

        #region SetupLayout(���㲢����ê��ͳ���)
        //���㲢����ê��ͳ���
        private void SetupLayout()
        {
            var gridLayoutRectTra = content;
            vertical = ScrollDirection == ScrollType.Vertical;
            horizontal = ScrollDirection ==  ScrollType.Horizontal;
            
            if (ScrollDirection == ScrollType.Vertical)
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
                gridLayoutRectTra.ForceUpdateRectTransforms();//ǿ��Updateʹ�����ȷ

                if (m_AutoCalculateConstraintCount)//����ÿ�ж�����
                    m_ConstraintCount = CalculateConstraintCount_Col();


                float length = CalculateScrollViewLength();
                gridLayoutRectTra.sizeDelta = new Vector2(0f, length);
            }
            else
            {
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
                gridLayoutRectTra.ForceUpdateRectTransforms();//ǿ��Updateʹ�߶���ȷ

                if (m_AutoCalculateConstraintCount)//����ÿ�ж�����
                    m_ConstraintCount = CalculateConstraintCount_Row();

                float length = CalculateScrollViewLength();
                gridLayoutRectTra.sizeDelta = new Vector2(length, 0f);
            }
        }

        private int CalculateConstraintCount_Col()
        {
            var gridLayoutRectTra = content;
            float width = gridLayoutRectTra.rect.width;

            //copy from sourcecode
            return Mathf.Max(1, Mathf.FloorToInt((width - m_Padding.horizontal + m_Spacing.x + 0.001f) / (m_CellSize.x + m_Spacing.x)));
        }

        private int CalculateConstraintCount_Row() 
        {
            var gridLayoutRectTra = content;
            float height = gridLayoutRectTra.rect.height;

            //copy from sourcecode
            return Mathf.Max(1, Mathf.FloorToInt((height - m_Padding.vertical + m_Spacing.y + 0.001f) / (m_CellSize.y + m_Spacing.y)));
        }

        private float CalculateScrollViewLength()
        {

            if (ScrollDirection == ScrollType.Vertical)
            {
                float cellSizeAndSpaceY = m_CellSizeAndSpaceY;
                float rowCount = Mathf.Ceil((float)m_Count / m_ConstraintCount);
                return m_Padding.vertical + m_Spacing.y + 0.001f + rowCount * cellSizeAndSpaceY;
            }
            else
            {
                float cellSizeAndSpaceX = m_CellSizeAndSpaceX;
                float colCount = Mathf.Ceil((float)m_Count / m_ConstraintCount);
                return m_Padding.horizontal + m_Spacing.x + 0.001f +colCount * cellSizeAndSpaceX;
            }
        }

        private float GetCellSizeAndSpaceY()
        {
            return m_CellSize.y + m_Spacing.y;
        }

        private float GetCellSizeAndSpaceX() 
        {
            return m_CellSize.x + m_Spacing.x;
        }
        #endregion

        //����Viewport���������ʾ��Item����
        private int GetMaxActiveItemNumber()
        {
            if (ScrollDirection == ScrollType.Vertical)
            {
                float height = viewport.rect.height;
                int rowCount = Mathf.CeilToInt(height / m_CellSizeAndSpaceY) +1;//��һ�ǻ��������У���һ��(������)û����ʧ�����ǵڶ���ȴҪ��ʾ������
                return m_ConstraintCount * rowCount;
            }
            else
            {
                float width = viewport.rect.width;
                int colCount = Mathf.CeilToInt(width / m_CellSizeAndSpaceX) +1;//��һ�ǻ��������У���һ��(������)û����ʧ�����ǵڶ���ȴҪ��ʾ������
                return m_ConstraintCount * colCount;
            }
        }

#if UNITY_EDITOR
        //private void OnValidate()
        //{
        //    Init();
        //}

        [ContextMenu("Ԥ��")]
        private void Preview()
        {
            FillGrid(m_Count);
        }
#endif
    }

}