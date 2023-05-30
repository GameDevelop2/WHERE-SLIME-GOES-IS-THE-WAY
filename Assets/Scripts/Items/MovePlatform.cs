using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool horizontalMove;
    [SerializeField] private Vector3 staticMapSensorOffset;
    [SerializeField] private Vector3 staticMapSensorSize;

    private Vector3 moveDirection;

    private LayerMask staticMapLayer; // MapStaticObject 레이어

    void Awake()
    {
        staticMapLayer = LayerMask.NameToLayer("MapStaticObject");
        staticMapLayer = 1 << staticMapLayer;

        if (horizontalMove)
            moveDirection = Vector3.right;
        else
            moveDirection = Vector3.up;
    }

    void FixedUpdate()
    {

        if (CheckDirectionSwitchCondition()) // 이동 방향에 MapStaticObject 레이어가 설정된 물체가 존재하는 경우 방향을 바꾼다.
            moveDirection = -moveDirection;

        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
    }

    bool CheckDirectionSwitchCondition()
    {
        Vector2 sensorCenter = transform.position + moveDirection.x * staticMapSensorOffset;

        // 박스캐스트 범위 디버깅 (Game창에서 Gizmos 활성화로 확인)         
        Vector2 sensorLeftBottom = sensorCenter - new Vector2(staticMapSensorSize.x / 2, staticMapSensorSize.y / 2);
        Vector2 sensorRightTop = sensorCenter + new Vector2(staticMapSensorSize.x / 2, staticMapSensorSize.y / 2);
        Debug.DrawLine(sensorLeftBottom, sensorRightTop, Color.red, 0.05f);
        Debug.DrawLine(new Vector2(sensorLeftBottom.x, sensorRightTop.y), new Vector2(sensorRightTop.x, sensorLeftBottom.y), Color.red, Time.fixedDeltaTime);

        // 박스 캐스트를 수행해 콜라이더 존재 여부를 반환한다. (Collider2D가 아니라 bool을 반환하니 주의!)
        // 거리가 0f이므로 방향은 어디든 상관 X (제자리에서 박스 하나 만큼의 충돌 검사만 함)
        return Physics2D.BoxCast(sensorCenter, staticMapSensorSize, 0f, Vector2.up, 0f, staticMapLayer).collider;
    }
}
