using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UIKit
{
    public partial class UIInteraction
    {
        [Serializable]
        public class GraphicColor : InteractionBehaviour
        {
            public Graphic[] graphics;
            public Color color = Color.white;

            public override void Execute()
            {
                if (graphics != null)
                {
                    foreach (var graphic in graphics)
                    {
                        graphic.color = color;
                    }
                }
            }
        }

    }
}