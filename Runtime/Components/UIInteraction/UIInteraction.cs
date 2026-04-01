using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameFramework.UIKit.UIInteraction;
/*
 * 事件
 *  点击
 *  长按
 *  ValueChange(IsOn)
 * 响应
 *  开关节点
 *  播放动画
 */
namespace GameFramework.UIKit
{
    [ExecuteAlways]
    public partial class UIInteraction : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        #region 
        [SerializeReference, SubclassSelector]
        public InteractionBehaviour[] interactions_onPointerEnter;
        [SerializeReference, SubclassSelector]
        public InteractionBehaviour[] interactions_onPointerExit;
        #endregion
        public bool isToggle = false;
        [SerializeReference, SubclassSelector]
        public InteractionBehaviour[] interactions_IsOn;

        [SerializeReference, SubclassSelector]
        public InteractionBehaviour[] interactions_IsOff;

        private Toggle m_toggle;

        public void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            UnInit();
        }

        private void Init()
        {
            if (isToggle)
            {
                m_toggle = GetComponent<Toggle>();
                if (m_toggle != null)
                {
                    m_toggle.onValueChanged.RemoveListener(OnToggleValueChange);
                    m_toggle.onValueChanged.AddListener(OnToggleValueChange);
                    OnToggleValueChange(m_toggle.isOn);//刷新一下
                }
                
            }
        }

        private void UnInit()
        {
            if (m_toggle != null)
            {
                m_toggle.onValueChanged.RemoveListener(OnToggleValueChange);
            }
        }

        private void OnToggleValueChange(bool isOn)
        {
            if (isOn)
            {
                foreach (var interaction in interactions_IsOn)
                {
                    interaction.Execute();
                }
            }
            else
            {
                foreach (var interaction in interactions_IsOff)
                {
                    interaction.Execute();
                }
            }
        }

        private void OnValidate()
        {
            Init();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var interaction in interactions_onPointerEnter)
            {
                interaction.Execute();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var interaction in interactions_onPointerExit)
            {
                interaction.Execute();
            }
        }
    }

}