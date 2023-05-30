using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] bool breakOnBreakerCollision = true;
    public void Break()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (breakOnBreakerCollision && collision.gameObject.CompareTag("Breaker"))
        {
            Break();
        }
    }
}
