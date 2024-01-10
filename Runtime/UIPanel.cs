using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.UIKit
{

    public abstract partial class UIPanel : MonoBehaviour
    {
        public struct PanelConfig
        {
            public string PrefabPath;
        }

        private ResLoader resLoader;

        protected virtual void Init(object data)
        {
            OnInit(data);
        }

        protected virtual void UnInit()
        {
            OnUnInit();
        }


        private void OnEnable()
        {
            OnShow();
        }

        private void OnDisable()
        {
            OnHide();
        }


        protected abstract PanelConfig ConfigData 
        {
            get;
        }
        protected abstract void OnInit(object data);

        protected abstract void OnUnInit();

        protected abstract void OnShow();

        protected abstract void OnHide();

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }


    }

}