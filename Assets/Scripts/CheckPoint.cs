using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameObject playerSpawner;
    private bool isChecked;

    private void Awake()
    {
        isChecked = false;
        playerSpawner = GameObject.FindGameObjectWithTag("Respawn");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isChecked && collision.CompareTag("Player") && playerSpawner)
        {
            isChecked = true;
            playerSpawner.transform.position = transform.position;
        }
    }
}
