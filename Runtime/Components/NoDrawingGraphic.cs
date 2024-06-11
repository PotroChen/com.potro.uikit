using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI���/�Զ���������(NoDrawingGraphic)")]
    public class NoDrawingGraphic : Graphic
    {
        public override void SetMaterialDirty()
        {
            return;
        }

        public override void SetVerticesDirty()
        {
            return;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}
