using UnityEngine;
using UnityEngine.Events;

public class ContactEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider2D> eventOnContact;
    [SerializeField] private UnityEvent<Collider2D, GameObject> eventOnContact_GameObjectParam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventOnContact?.Invoke(collision);
        eventOnContact_GameObjectParam?.Invoke(collision, gameObject);
    }
}
