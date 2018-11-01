using UnityEngine;
using System;

namespace TEDCore.DefineSymbol
{
    [Serializable]
    public class DefineSymbol
    {
        public bool Enable { get { return m_enable; } }
        [SerializeField] private bool m_enable;
        public string Key { get { return m_key; } }
        [SerializeField] private string m_key;
        public string Description { get { return m_description; } }
        [SerializeField] private string m_description;

        public DefineSymbol(string key, string description, bool enable)
        {
            m_key = key;
            m_description = description;
            m_enable = enable;
        }

        public void Update(string key, string description, bool enable)
        {
            m_key = key;
            m_description = description;
            m_enable = enable;
        }
    }
}
