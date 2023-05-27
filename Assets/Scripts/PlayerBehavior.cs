using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundedThreshold = 0.1f; // 플레이어 중심이 땅에서 얼마나 떨어져 있을 때 땅에 닿은 상태로 치는지 임계값

    [SerializeField] private List<GameObject> itemList; // 플레이어가 변신할 수 있는 물체들
    private int currentItemIndex; // 위 리스트에서 현재 플레이어가 변신하고 있는 물체의 인덱스
    private ItemPreview itemPreview;

    private Rigidbody2D rigidbody;

    private bool isGrounded; // 플레이어가 땅에 붙어있는가? true -> 붙어 있음.
    private int groundLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크
    private int kinematicMapLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크

    private Vector3 playerSpawner; // 플레이어 부활 위치

    void Awake()
    {
        currentItemIndex = -1;
        rigidbody = GetComponent<Rigidbody2D>();
        itemPreview = GetComponentInChildren<ItemPreview>();
        groundLayer = ~(1 << LayerMask.NameToLayer("Player")); // 이 마스크 적용 시 Player 이외의 모든 레이어와 충돌.
        kinematicMapLayer = LayerMask.NameToLayer("MapKinematicObject");
        playerSpawner = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void Start()
    {
        itemPreview.InitSpriteList(itemList);
    }

    void FixedUpdate()
    {
        CheckIsGrounded();
        Locomotion();        
    }

    private void Update()
    {
        SelectItem();
        LocateItemAndRespawn();
    }

    private void Locomotion() // 플레이어의 이동을 제어
    {
        // 나중에 Input 클래스 대신 Input System Package 쓰는 걸로 수정.
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0f)
            transform.position += Vector3.right * horizontal * moveSpeed * Time.fixedDeltaTime;
        if (isGrounded && Input.GetButton("Jump"))
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
    }

    private void CheckIsGrounded() // 플레이어가 땅에 닿아 있는 지 확인.
    {
        Vector2 lineStart = transform.position - new Vector3(transform.lossyScale.x/2, transform.lossyScale.y/2 + groundedThreshold);

        Debug.DrawLine(lineStart, lineStart + new Vector2(transform.lossyScale.x, 0f), Color.red, 0.05f);
        if (Physics2D.Linecast(lineStart, lineStart + new Vector2(transform.lossyScale.x, 0f), groundLayer).collider)
            isGrounded = true;
        else
            isGrounded = false;
    }

    private void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentItemIndex = 0;
            itemPreview.ShowItemPreview(currentItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentItemIndex = 1;
            itemPreview.ShowItemPreview(currentItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentItemIndex = 2;
            itemPreview.ShowItemPreview(currentItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentItemIndex = 3;
            itemPreview.ShowItemPreview(currentItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentItemIndex = 4;
            itemPreview.ShowItemPreview(currentItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            currentItemIndex = -1;
            itemPreview.HideItemPreview();
        }
    }

    private void LocateItemAndRespawn() 
    {
        if (currentItemIndex != -1 && Input.GetKeyDown(KeyCode.LeftShift)) // 왼쪽 시프트 누르면 아이템 배치 후 재시작. 임시로 GetKeyDown 사용
        {
            Instantiate(itemList[currentItemIndex], transform.position, transform.rotation);
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = playerSpawner;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == kinematicMapLayer)
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == kinematicMapLayer)
            transform.SetParent(null);
    }
}
