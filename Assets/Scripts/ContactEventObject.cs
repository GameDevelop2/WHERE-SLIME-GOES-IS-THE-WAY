using UnityEngine;
using UnityEngine.Events;

public class ContactEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> eventOnContact;
    [SerializeField] private UnityEvent<Collider2D, GameObject> eventOnContact_GameObjectParam;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        eventOnContact?.Invoke(collider);
        eventOnContact_GameObjectParam?.Invoke(collider, gameObject);
    }
}
