using TEDCore.UnitTesting;
using TEDCore.Event;
using UnityEngine;

public class UnitTesting_Event : BaseUnitTesting
{
    public enum TestEnum
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    [SerializeField] private int m_updateCount;
    [SerializeField] private int m_fixedUpdateCount;
    [SerializeField] private int m_lateUpdateCount;
    private EventListener m_eventListener;

    private void Awake()
    {
        Application.targetFrameRate = 9999;
        QualitySettings.vSyncCount = 0;
        m_eventListener = new EventListener();
    }

    [TestButton]
    public void RegisterUpdateEvent()
    {
        m_eventListener.ListenForEvent(TestEnum.Update.GetHashCode(), OnUpdate);
    }

    [TestButton]
    public void RegisterFixedUpdateEvent()
    {
        m_eventListener.ListenForEvent(TestEnum.FixedUpdate.GetHashCode(), OnFixedUpdate);
    }

    [TestButton]
    public void RegisterLateUpdateEvent()
    {
        m_eventListener.ListenForEvent(TestEnum.LateUpdate.GetHashCode(), OnLateUpdate);
    }

    private void Update()
    {
        EventManager.Instance.SendEvent(TestEnum.Update.GetHashCode());
    }

    private void FixedUpdate()
    {
        EventManager.Instance.SendEvent(TestEnum.FixedUpdate.GetHashCode());
    }

    private void LateUpdate()
    {
        EventManager.Instance.SendEvent(TestEnum.LateUpdate.GetHashCode());
    }

    private EventResult OnUpdate(object eventData)
    {
        m_updateCount++;
        return null;
    }

    private EventResult OnFixedUpdate(object eventData)
    {
        m_fixedUpdateCount++;
        return null;
    }

    private EventResult OnLateUpdate(object eventData)
    {
        m_lateUpdateCount++;
        return null;
    }
}
