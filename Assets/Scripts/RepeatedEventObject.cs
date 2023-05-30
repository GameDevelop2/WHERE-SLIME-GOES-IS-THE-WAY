using UnityEngine;
using UnityEngine.Events;

public class RepeatedEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnEveryPeriod;
    [SerializeField] private UnityEvent<GameObject> eventOnEveryPeriod_GameObjectParam;
    [SerializeField] private float eventPeriod = 5f;
    [SerializeField] private float initialDelayBeforeEventStart = 0f;

    private float currentEventCooltime;

    void Awake()
    {
        currentEventCooltime = initialDelayBeforeEventStart;
    }
    
    void Update()
    {
        currentEventCooltime -= Time.deltaTime;

        if (currentEventCooltime <= 0f)
        {
            eventOnEveryPeriod?.Invoke();
            eventOnEveryPeriod_GameObjectParam?.Invoke(gameObject);
            currentEventCooltime = eventPeriod;
        }
    }
}
