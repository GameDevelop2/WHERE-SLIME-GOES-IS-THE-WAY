using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool horizontalMove;

    [Tooltip("MapStaticObject를 감지하는 정사각형 센서의 위치 오프셋.\n*주의: 오프셋의 x값은 좌우 방향일 때만, y값은 상하 방향일 때만 적용됨.")]
    [SerializeField] private Vector3 sensorOffset;
    [Tooltip("MapStaticObject를 감지하는 정사각형 센서의 이동 방향과 무관한 위치 오프셋.")]
    [SerializeField] private Vector3 sensorNonDirectionalOffset;
    [Tooltip("MapStaticObject를 감지하는 정사각형 센서의 크기.")]
    [SerializeField] private Vector3 sensorSize;

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
        Vector2 sensorCenter = transform.position + new Vector3(sensorOffset.x * moveDirection.x, sensorOffset.y * moveDirection.y) + sensorNonDirectionalOffset;

        // 박스캐스트 범위 디버깅 (Game창에서 Gizmos 활성화로 확인)         
        Vector2 sensorLeftBottom = sensorCenter - new Vector2(sensorSize.x / 2, sensorSize.y / 2);
        Vector2 sensorRightTop = sensorCenter + new Vector2(sensorSize.x / 2, sensorSize.y / 2);
        Debug.DrawLine(sensorLeftBottom, sensorRightTop, Color.red, 0.05f);
        Debug.DrawLine(new Vector2(sensorLeftBottom.x, sensorRightTop.y), new Vector2(sensorRightTop.x, sensorLeftBottom.y), Color.red, Time.fixedDeltaTime);

        // 박스 캐스트를 수행해 콜라이더 존재 여부를 반환한다. (Collider2D가 아니라 bool을 반환하니 주의!)
        // 거리가 0f이므로 방향은 어디든 상관 X (제자리에서 박스 하나 만큼의 충돌 검사만 함)
        return Physics2D.BoxCast(sensorCenter, sensorSize, 0f, Vector2.up, 0f, staticMapLayer).collider;
    }
}
