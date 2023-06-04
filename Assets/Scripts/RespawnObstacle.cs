using UnityEngine;

public class RespawnObstacle : MonoBehaviour
{
    [Tooltip("충돌 발생 시 스스로를 Destroy 할 지 여부")]
    [SerializeField] private bool destroyOnCollision = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어에 닿은 경우
            collision.gameObject.GetComponent<PlayerBehavior>().Respawn();

        if (collision.CompareTag("Portal")) // 포탈에 닿은 경우엔 스스로를 삭제하지 않는다. (단, MapStaticObject 레이어로 설정 시엔 설정 상 포탈과 충돌 판정이 발생하지 않음)
            return;
        else if (destroyOnCollision)
            Destroy(gameObject);
    }
}
