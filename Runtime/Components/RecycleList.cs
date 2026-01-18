using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public class RecycleList : MonoBehaviour
    {
        public GameObject Template;

        private GameObjectPool goPool;
        private List<IPoolObject<GameObject>> poolObjects = new List<IPoolObject<GameObject>>();

        public void Awake()
        {
            if (Template != null)
                Template.SetActive(false);
        }

        public void FillList(int count,Action<int,GameObject> onItemCallback)
        {
            if (goPool == null)
            {
                goPool = new GameObjectPool(Template, count,transform);
            }
            for (int i = poolObjects.Count - 1; i >= 0; i--)
            {
                var poolObject = poolObjects[i];
                poolObject.Recycle();
                poolObjects.RemoveAt(i);
            }
            for (int i = 0; i < count; i++)
            {
                var poolObject = goPool.Get(transform);
                poolObjects.Add(poolObject);
                onItemCallback.Invoke(i, poolObject.Content);
            }
        }

        private void OnDestroy()
        {
            for (int i = poolObjects.Count - 1; i >= 0; i--)
            {
                var poolObject = poolObjects[i];
                poolObject.Recycle();
                poolObjects.RemoveAt(i);
            }

            if (goPool != null)
            {
                goPool.Dispose();
            }
        }
    }
}
