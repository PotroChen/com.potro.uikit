using UnityEngine;

namespace GameFramework.UIKit
{
    /// <summary>
    /// Toast 提示面板基类。固定在 Message 层，提供 Info / Warning / Error 三种提示接口。
    /// </summary>
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
        }

        public virtual void ToastWarning(string msg)
        {
        }

        public virtual void ToastError(string msg)
        {
        }
    }
}
