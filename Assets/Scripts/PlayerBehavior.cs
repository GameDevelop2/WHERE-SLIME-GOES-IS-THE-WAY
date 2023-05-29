using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundSensorThreshold = 0.1f; // 플레이어 중심이 땅에서 얼마나 떨어져 있을 때 땅에 닿은 상태로 치는지 임계값
    [SerializeField] private float groundSensorPadding = 0.05f; // 땅에 붙어있는지 확인하는 라인 캐스트의 패딩 (좌우 여백)

    /* 플레이어 입력 매핑 */
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputActionMap fieldActionMap;
    private InputAction moveAction, jumpAction, selectItemAction, spawnItemAction;

    [SerializeField] private List<GameObject> itemList; // 플레이어가 변신할 수 있는 물체들
    private int currentItemIndex; // 위 리스트에서 현재 플레이어가 변신하고 있는 물체의 인덱스
    private ItemPreview itemPreview;

    private Rigidbody2D player_rigidbody;

    private bool isGrounded; // 플레이어가 땅에 붙어있는가? true -> 붙어 있음.
    private int groundLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크
    private bool isOnKinematicObject;
    private int kinematicMapLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크

    private Vector3 playerSpawner; // 플레이어 부활 위치

    void Awake()
    {
        player_rigidbody = GetComponent<Rigidbody2D>();
        itemPreview = GetComponentInChildren<ItemPreview>();
        groundLayer = ~(1 << LayerMask.NameToLayer("Player")); // 이 마스크 적용 시 Player 이외의 모든 레이어와 충돌.
        kinematicMapLayer = LayerMask.NameToLayer("MapKinematicObject");
        playerSpawner = GameObject.FindGameObjectWithTag("Respawn").transform.position;

        fieldActionMap = inputActionAsset.FindActionMap("field", true);
        moveAction = fieldActionMap.FindAction("Move", true);
        jumpAction = fieldActionMap.FindAction("Jump", true);
        selectItemAction = fieldActionMap.FindAction("SelectItem", true);
        spawnItemAction = fieldActionMap.FindAction("SpawnItem", true);

        currentItemIndex = -1;
        isOnKinematicObject = false; 
    }

    void Start()
    {
        itemPreview.InitSpriteList(itemList);
    }

    void OnEnable()
    {
        fieldActionMap.Enable();
        jumpAction.performed += Jump;
        selectItemAction.performed += SelectItem;
        spawnItemAction.performed += SpawnItemAndRespawn;
    }

    void OnDisable()
    {
        fieldActionMap.Disable();
        jumpAction.performed -= Jump;
        selectItemAction.performed -= SelectItem;
        spawnItemAction.performed -= SpawnItemAndRespawn;
    }

    void FixedUpdate()
    {
        CheckIsGrounded();
        Move();        
    }

    private void Move() // 플레이어의 좌우 이동. FixedUpdate에서 호출.
    {
        Vector3 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude >= 0.05f)
            transform.position += moveInput * moveSpeed * Time.fixedDeltaTime;
    }

    private void Jump(InputAction.CallbackContext context) // 플레이어 점프 시 호출
    {
        if (isGrounded)
            player_rigidbody.velocity = new Vector2(player_rigidbody.velocity.x, jumpForce);
    }

    private void CheckIsGrounded() // 플레이어가 땅에 닿아 있는 지 확인.
    {
        Vector2 lineStart = transform.position - new Vector3(transform.lossyScale.x/2 - groundSensorPadding, transform.lossyScale.y/2 + groundSensorThreshold);

        Debug.DrawLine(lineStart, lineStart + new Vector2(transform.lossyScale.x - 2*groundSensorPadding, 0f), Color.red, 0.05f);
        RaycastHit2D hitResult = Physics2D.Linecast(lineStart, lineStart + new Vector2(transform.lossyScale.x, 0f), groundLayer);
        if (hitResult.collider)
        {
            isGrounded = true;
            if (!isOnKinematicObject && hitResult.collider.gameObject.layer == kinematicMapLayer) // 플레이어가 움직이는 Kinematic 물체 위에 있는 지 확인
            {
                isOnKinematicObject = true;
                transform.SetParent(hitResult.collider.transform); // 플레이어가 함께 움직이도록 한다.
            }
        }
        else
            isGrounded = false;

        if (isOnKinematicObject && (!hitResult.collider || hitResult.collider.gameObject.layer != kinematicMapLayer))
        {
            isOnKinematicObject = false;
            transform.SetParent(null);
        }
    }

    private void SelectItem(InputAction.CallbackContext context)
    {
        currentItemIndex = (int)selectItemAction.ReadValue<float>() - 1;
        if (currentItemIndex < 0)
            itemPreview.HideItemPreview();
        else
            itemPreview.ShowItemPreview(currentItemIndex);
    }

    private void SpawnItemAndRespawn(InputAction.CallbackContext context) // 왼쪽 시프트 누르면 아이템 배치 후 재시작.
    {
        if (currentItemIndex >= 0) // -1일 때 (선택된 아이템이 없을 때)는 취소됨.
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
