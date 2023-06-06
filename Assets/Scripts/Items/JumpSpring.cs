using UnityEngine;

public class JumpSpring : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 10f;
    private Rigidbody2D playerRigidbody;
    Animator animator;

    private void Awake()
    {
        if (!playerRigidbody)
            playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpSpeed);
            animator.SetTrigger("Bounce");
        }
    }
}
