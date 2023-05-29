using UnityEngine;
using UnityEngine.Events;

public class RepeatedEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnEveryPeriod;
    [SerializeField] private UnityEvent<GameObject> eventOnEveryPeriod_GameObjectParam;
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
            eventOnEveryPeriod?.Invoke();
            eventOnEveryPeriod_GameObjectParam?.Invoke(gameObject);
            currentEventCooltime = eventPeriod;
        }
    }
}
