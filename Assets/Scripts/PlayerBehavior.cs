using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Vector3 groundSensorOffset; 
    [SerializeField] private Vector2 groundSensorSize;

    /* 플레이어 입력 매핑 */
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputActionMap fieldActionMap;
    private InputAction moveAction, jumpAction, selectItemAction, spawnItemAction, removeItemAction;

    [SerializeField] private List<GameObject> itemList; // 플레이어가 변신할 수 있는 물체들
    private int currentItemIndex; // 위 리스트에서 현재 플레이어가 변신하고 있는 물체의 인덱스
    private ItemPreview itemPreview;
    Stack<GameObject> spawnedItemStack;

    private Rigidbody2D player_rigidbody;

    private bool isGrounded; // 플레이어가 땅에 붙어있는가? true -> 붙어 있음.
    private int groundLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크
    private bool isOnKinematicObject;
    private int kinematicMapLayer; // isGrounded 체크를 위해 땅에 레이캐스트 시 적용할 레이어 마스크

    private Transform playerSpawner; // 플레이어 부활 위치

    void Awake()
    {
        player_rigidbody = GetComponent<Rigidbody2D>();
        itemPreview = GetComponentInChildren<ItemPreview>();
        groundLayer = ~(1 << LayerMask.NameToLayer("Player")); // 이 마스크 적용 시 Player 이외의 모든 레이어와 충돌.
        kinematicMapLayer = LayerMask.NameToLayer("MapKinematicObject");
        playerSpawner = GameObject.FindGameObjectWithTag("Respawn").transform;

        fieldActionMap = inputActionAsset.FindActionMap("field", true);
        moveAction = fieldActionMap.FindAction("Move", true);
        jumpAction = fieldActionMap.FindAction("Jump", true);
        selectItemAction = fieldActionMap.FindAction("SelectItem", true);
        spawnItemAction = fieldActionMap.FindAction("SpawnItem", true);
        removeItemAction = fieldActionMap.FindAction("RemoveItem", true);

        spawnedItemStack = new Stack<GameObject>();

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
        removeItemAction.performed += RemoveLastSpawnedItem;
    }

    void OnDisable()
    {
        fieldActionMap.Disable();
        jumpAction.performed -= Jump;
        selectItemAction.performed -= SelectItem;
        spawnItemAction.performed -= SpawnItemAndRespawn;
        removeItemAction.performed -= RemoveLastSpawnedItem;
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
        Vector2 sensorCenter = transform.position + groundSensorOffset;

        // Debug
        Vector2 sensorLeftBottom = sensorCenter - new Vector2(groundSensorSize.x / 2, groundSensorSize.y / 2);
        Vector2 sensorRightTop = sensorCenter + new Vector2(groundSensorSize.x / 2, groundSensorSize.y / 2);
        Debug.DrawLine(sensorLeftBottom, sensorRightTop, Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(new Vector2(sensorLeftBottom.x, sensorRightTop.y), new Vector2(sensorRightTop.x, sensorLeftBottom.y), Color.red, Time.fixedDeltaTime);

        RaycastHit2D hitResult = Physics2D.BoxCast(sensorCenter, groundSensorSize, 0f, Vector2.down, 0f, groundLayer);
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
            spawnedItemStack.Push(Instantiate(itemList[currentItemIndex], transform.position, transform.rotation));
            Respawn();
        }
    }

    private void RemoveLastSpawnedItem(InputAction.CallbackContext context)
    {
        if (spawnedItemStack.Count <= 0)
            return;

        GameObject lastSpawnedItem = spawnedItemStack.Pop();
        lastSpawnedItem.SetActive(false);
        Destroy(lastSpawnedItem);
    }

    public void Respawn()
    {
        transform.position = playerSpawner.position;
    }
}
