using UnityEngine;
using UnityEngine.Events;

public class RepeatedEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnPlayerContact;
    [SerializeField] private float eventPeriod = 5f;
    [SerializeField] private bool invokeEventOnStart;

    private float currentEventCooltime;

    void Awake()
    {
        if (invokeEventOnStart)
            currentEventCooltime = 0f;
        else
            currentEventCooltime = eventPeriod;
    }
    
    void Update()
    {
        currentEventCooltime -= Time.deltaTime;

        if (currentEventCooltime <= 0f)
        {
            eventOnPlayerContact?.Invoke();
            currentEventCooltime = eventPeriod;
        }
    }
}
