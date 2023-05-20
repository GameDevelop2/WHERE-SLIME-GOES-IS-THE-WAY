using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 250f;
    [SerializeField] private float groundedThreshold = 0.26f; // 플레이어 중심이 땅에서 얼마나 떨어져 있을 때 땅에 닿은 상태로 치는지 임계값

    [SerializeField] private List<GameObject> itemList; // 플레이어가 변신할 수 있는 물체들
    private int currentItemIndex; // 위 리스트에서 현재 플레이어가 변신하고 있는 물체의 인덱스

    private Rigidbody2D rigidbody;

    private bool isGrounded; // 플레이어가 땅에 붙어있는가? true -> 붙어 있음.
    private int groundLayerMask; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크

    private Vector3 playerSpawner; // 플레이어 부활 위치

    void Awake()
    {
        currentItemIndex = 0;
        rigidbody = GetComponent<Rigidbody2D>();
        groundLayerMask = ~(1 << LayerMask.NameToLayer("Player")); // 이 마스크 적용 시 Player 이외의 모든 레이어와 충돌.
        playerSpawner = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void FixedUpdate()
    {
        CheckIsGrounded();
        Locomotion();        
    }

    private void Update()
    {
        LocateItemAndRespawn();
    }

    private void Locomotion() // 플레이어의 이동을 제어
    {
        // 나중에 Input 클래스 대신 Input System Package 쓰는 걸로 수정.
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0f)
            transform.position += Vector3.right * horizontal * moveSpeed * Time.fixedDeltaTime;
        if (isGrounded && Input.GetButton("Jump"))
            rigidbody.AddForce(Vector2.up * jumpForce);
    }

    private void CheckIsGrounded() // 플레이어가 땅에 닿아 있는 지 확인. 중앙에서 쏘는 걸론 문제가 있으므로 이후 다른 방식으로 대체.
    {
        Debug.DrawRay(transform.position, Vector2.down * groundedThreshold, Color.red);
        if (Physics2D.Raycast(transform.position, Vector2.down, groundedThreshold, groundLayerMask).collider)
            isGrounded = true;
        else
            isGrounded = false;
    }

    private void LocateItemAndRespawn() 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 시프트 누르면 아이템 배치 후 재시작. 임시로 GetKeyDown 사용
        {
            Instantiate(itemList[currentItemIndex], transform.position, transform.rotation);
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = playerSpawner;
    }
}
