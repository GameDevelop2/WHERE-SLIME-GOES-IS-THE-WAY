using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool horizontalMove;
    [SerializeField] private float switchDirectionThreshold = 0.1f;

    private Vector3 moveDirection;
    private Rigidbody2D rigidbody;

    private LayerMask staticMapMask; // MapStaticObject 레이어

    void Awake()
    {
        staticMapMask = LayerMask.NameToLayer("MapStaticObject");
        staticMapMask = 1 << staticMapMask;

        rigidbody = GetComponent<Rigidbody2D>();

        if (horizontalMove)
            moveDirection = Vector3.right;
        else
            moveDirection = Vector3.up;
    }

    void FixedUpdate()
    {
        Vector2 lineStart, lineEnd; // 이동 방향에 물체가 존재하는 지 확인하기 위한 라인 캐스트의 시작/끝 점
        if (horizontalMove)
        {
            lineStart = transform.position + new Vector3((transform.localScale.x/2 + switchDirectionThreshold) * moveDirection.x, transform.localScale.y/2, 0);
            lineEnd = lineStart - new Vector2(0, transform.localScale.y);
        }
        else
        {
            lineStart = transform.position + new Vector3(transform.localScale.x/2, (transform.localScale.y/2 + switchDirectionThreshold) * moveDirection.y, 0);
            lineEnd = lineStart - new Vector2(transform.localScale.x, 0);
        }

        Debug.DrawLine(lineStart, lineEnd, Color.blue, 0.05f); // 라인캐스트 범위 디버깅 (Game창에서 Gizmos 활성화로 확인) 
        if (Physics2D.Linecast(lineStart, lineEnd, staticMapMask).collider) // 이동 방향에 MapStaticObject 레이어가 설정된 물체가 존재하는 경우 방향을 바꾼다.
            moveDirection = -moveDirection;

        rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
