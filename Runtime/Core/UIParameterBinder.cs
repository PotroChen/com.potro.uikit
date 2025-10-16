using System;
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
        public struct ParameterEntry
        {
            public string Name;
            public ParameterType ParameterType;
            public GameObject Go;
            public Component Component;
        }

        public ParameterEntry[] entries;
    }
}
