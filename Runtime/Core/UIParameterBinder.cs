using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public class UIParameterBinder : MonoBehaviour
    {
        public enum ParameterType
        {
            GameObject = 0,
            Component = 1,
        }

        [Serializable]
        public class ParameterEntry
        {
            public string Name;
            public ParameterType ParameterType;
            public GameObject Go;
            public Component Component;
        }

        public ParameterEntry[] entries;

        private bool initted = false;
        private Dictionary<string, ParameterEntry> name2ParameterEntry = new ();
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (initted)
                return;
            name2ParameterEntry.Clear();
            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Name.Trim()))
                {
                    name2ParameterEntry[entry.Name.Trim()] = entry;
                }
            }

            initted = true;
        }

        public GameObject GetGameObject(string name)
        {
            name2ParameterEntry.TryGetValue(name, out var entry);
            return entry.Go;
        }

        public new Component GetComponent(string name)
        {
            name2ParameterEntry.TryGetValue(name, out var entry);
            return entry.Component;
        }
    }
}
