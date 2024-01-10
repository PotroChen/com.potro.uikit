using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.UIKit
{
    public partial class UIPanel
    {
        protected static Dictionary<string, UIPanel> panels = new Dictionary<string, UIPanel>();
        public static void OpenPanel<TPanel>(object data,UILayer uiLayer = UILayer.Common) where TPanel : UIPanel
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

                panels.Add(instance.name, panel);
                panel.Init(data);
            }
        }

        public static void ClosePanel(UIPanel panel)
        {
            panel.UnInit();
            Destroy(panel.gameObject);
            panels.Remove(panel.name);
            panel.resLoader.ReleaseAllAssets();
        }
    }
}
