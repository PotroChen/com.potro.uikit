using UnityEngine;

namespace GameFramework.UIKit
{

    public class LabelAttribute : PropertyAttribute
    {
        private string m_Text;
        public string Text => m_Text;
        public LabelAttribute(string text)
        {
            m_Text = text;
        }
    }
}
