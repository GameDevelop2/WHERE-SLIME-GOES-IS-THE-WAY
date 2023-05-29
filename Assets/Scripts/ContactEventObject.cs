using UnityEngine;
using UnityEngine.Events;

public class ContactEventObject : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnContact;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventOnContact?.Invoke();
    }
}
