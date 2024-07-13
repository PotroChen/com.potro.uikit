using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFramework.UIKit
{
    [AddComponentMenu("UI框架/点击穿透(EventPass)")]
    public class EventPass : MonoBehaviour, IPointerClickHandler
    {
        private List<RaycastResult> tempResult = new List<RaycastResult>();
        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = eventData.position
            };

            // 存储射线检测结果,找到下一层物体,将EventData传递过去
            EventSystem.current.RaycastAll(pointerData, tempResult);
            int currentIndex = tempResult.FindIndex((RaycastResult result) => { return result.gameObject == this.gameObject; });
            if (currentIndex != -1 && currentIndex + 1 < tempResult.Count)
            {
                ExecuteEvents.Execute(tempResult[currentIndex + 1].gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            }
            tempResult.Clear();
        }
    }
}
