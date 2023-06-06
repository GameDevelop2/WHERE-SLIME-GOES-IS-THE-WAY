using UnityEngine;

public class PeriodMoveObject : MonoBehaviour
{
    [Tooltip("방향 전환 주기")]
    [SerializeField] private float directionSwitchPeriod = 1f;
    [Tooltip("방향 전환까지 이동하는 거리\n*주의: 오브젝트 양 끝이 아니라 중심이 이동하는 거리임.")]
    [SerializeField] private float moveRange = 1f;

    [SerializeField] private bool horizontalMove;
    [Tooltip("초기 이동 방향을 뒤집을 지.\n*시작 이동 방향: 좌우 이동 시 우, 상하 이동 시 상")]
    [SerializeField] private bool reverseInitialDirection = false;

    private Vector3 moveDirection;
    private float currentSwitchCoolTime;
    private float moveSpeed;

    void Awake()
    {
        moveSpeed = moveRange / directionSwitchPeriod;

        currentSwitchCoolTime = directionSwitchPeriod;
        if (horizontalMove)
            moveDirection = reverseInitialDirection? Vector3.left : Vector3.right;
        else
            moveDirection = reverseInitialDirection ? Vector3.down : Vector3.up;
    }

    void FixedUpdate()
    {
        currentSwitchCoolTime -= Time.fixedDeltaTime;
        if (currentSwitchCoolTime <= 0f)
        {
            currentSwitchCoolTime = directionSwitchPeriod;
            moveDirection = -moveDirection;
        }
        transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
