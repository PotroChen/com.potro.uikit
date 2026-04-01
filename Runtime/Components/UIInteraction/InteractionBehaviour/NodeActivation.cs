using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public partial class UIInteraction
    {
        [Serializable]
        public class NodeActivation : InteractionBehaviour
        {
            public List<GameObject> nodesToActive = new List<GameObject>();
            public List<GameObject> nodesToUnActive = new List<GameObject>();

            public override void Execute()
            {
                if (nodesToActive != null && nodesToActive.Count > 0)
                {
                    foreach (GameObject node in nodesToActive) 
                    {
                        if (node != null)
                            node.SetActive(true);
                    }
                }

                if (nodesToUnActive != null && nodesToUnActive.Count > 0)
                {
                    foreach (GameObject node in nodesToUnActive)
                    {
                        if (node != null)
                            node.SetActive(false);
                    }
                }
            }
        }
    }
}
