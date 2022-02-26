using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.ResKit;

namespace GameFramework.UIKit
{
    public enum UILayer
    {
        Bottom,
        Common,
        Top
    }

    public class UIRoot : MonoBehaviour
    {

        private static UIRoot instance;

        public static Transform BottomLayer
        {
            get
            {
                return instance.bottomLayer;
            }
        }

        public static Transform CommonLayer
        {
            get
            {
                return instance.commonLayer;
            }
        }

        public static Transform TopLayer
        {
            get
            {
                return instance.topLayer;
            }
        }

        public Transform bottomLayer;
        public Transform commonLayer;
        public Transform topLayer;



        // Use this for initialization
        void Start()
        {
            if (!instance)
                instance = this;
            else
            {
                Destroy(this);
                return;
            }

            ResMgr.Init();
            //UIPanel.OpenPanel<UIGamePanel>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}