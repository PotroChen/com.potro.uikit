using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    /// <summary>
    /// GridLayoutGroupForRecycleScrollList(配合RecycleScrollList使用),不控制Child Item的RectTransform
    /// </summary>
    public class GridLayoutGroupForRecycleScrollList : GridLayoutGroup
    {
        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }

}