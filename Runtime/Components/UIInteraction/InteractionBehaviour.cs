using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public partial class UIInteraction
    {
        [Serializable]
        public class InteractionBehaviour 
        {
            public virtual void Execute()
            { 
            }
        }
    }

}