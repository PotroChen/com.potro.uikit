using UnityEngine;

namespace GameFramework.UIKit
{
    public abstract class ToastPanelBase : UIPanel
    {
        protected override PanelConfig ConfigData => new PanelConfig()
        {
            PrefabPath = this.PrefabPath,
            UILayer =  UILayer.Message
        };

        protected abstract string PrefabPath { get; }

        public virtual void ToastInfo(string msg)
        {
            Debug.Log(msg);
        }
    }
}
