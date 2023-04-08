using System.Collections.Generic;
using UnityEngine;
using GameFramework.ResKit;

namespace GameFramework.UIKit
{
    public class UIPanel : MonoBehaviour
    {

        protected static Dictionary<string, UIPanel> panels = new Dictionary<string, UIPanel>();

        private ResLoader resLoader;

        protected virtual void Init()
        {
            panels.Add(this.name, this);
        }

        public static void OpenPanel<TPanel>(UILayer uiLayer = UILayer.Common) where TPanel : UIPanel
        {
            string panelName = typeof(TPanel).ToString();
            string[] splitedName = panelName.Split('.');
            panelName = splitedName[splitedName.Length - 1];

            if (panels.ContainsKey(panelName))
            {
                panels[panelName].Open();
            }
            else
            {
                ResLoader resLoader = new ResLoader();

                GameObject prefab = resLoader.LoadAsset<GameObject>(panelName);
                GameObject instance = null;
                switch (uiLayer)
                {
                    case UILayer.Common:
                        instance = GameObject.Instantiate(prefab, UIRoot.CommonLayer);
                        break;
                    case UILayer.Bottom:
                        instance = GameObject.Instantiate(prefab, UIRoot.BottomLayer);
                        break;
                    case UILayer.Top:
                        instance = GameObject.Instantiate(prefab, UIRoot.TopLayer);
                        break;
                }
                instance.name = panelName;

                TPanel panel = instance.GetComponent<TPanel>();
                panel.resLoader = resLoader;
                panel.Init();
            }
        }

        public void ClosePanel()
        {
            Destroy(this.gameObject);
            panels.Remove(this.name);
            resLoader.UnLoadAll();
        }


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