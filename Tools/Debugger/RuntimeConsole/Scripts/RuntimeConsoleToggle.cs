using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RuntimeConsoleToggle : MonoBehaviour
{
    [SerializeField] private Toggle m_toggle;
    [SerializeField] private Text m_countText;

    public void SetToggleValueChanged(UnityAction<bool> unityAction)
    {
        m_toggle.onValueChanged.AddListener(unityAction);
    }

    public void SetCount(int count)
    {
        m_countText.text = count.ToString();
    }
}
