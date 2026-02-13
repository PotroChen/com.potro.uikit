using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    public class ExToggle : Toggle, IPointerEnterHandler
    {
        [SerializeField]
        internal UnityEvent m_Click;

        public UnityEvent Click => m_Click;

        [SerializeField]
        internal UnityEvent m_PointerEnter;
        public UnityEvent PointerEnter => m_PointerEnter;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            m_Click?.Invoke();

        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PointerEnter?.Invoke();
        }
    }

}