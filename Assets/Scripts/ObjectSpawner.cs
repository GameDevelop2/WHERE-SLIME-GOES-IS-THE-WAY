using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Common Setting")]
    [SerializeField] private GameObject objectToSpawn;
    [Tooltip("스포너의 Awake 시점 부터 오브젝트 최초 스폰 까지의 시간")]
    [SerializeField] private float initialDelayBeforeSpawn = 0f;
    [Tooltip("오브젝트가 스폰될 상대적 위치")]
    [SerializeField] private Vector3 spawnPositionOffset;
    [Tooltip("오브젝트 스폰 후 스스로를 Destroy 할 지 여부")]
    [SerializeField] private bool destroySelfAfterObjectSpawn;

    [Space(10f)]
    [Header("Repeat Setting")]
    [Tooltip("오브젝트를 반복적으로 스폰할 지 여부")]
    [SerializeField] private bool repeatSpawning = false;
    [SerializeField] private float spawnPeriod = 1f;
    private float currentSpawnCoolTime = 1f;

    void Awake()
    {
        currentSpawnCoolTime = initialDelayBeforeSpawn;
    }

    void Update()
    {
        currentSpawnCoolTime -= Time.deltaTime;
        if (currentSpawnCoolTime <= 0f)
        {
            Instantiate<GameObject>(objectToSpawn, transform.position + spawnPositionOffset, objectToSpawn.transform.rotation);

            if (destroySelfAfterObjectSpawn)
                Destroy(gameObject);
            else if (repeatSpawning)
                currentSpawnCoolTime = spawnPeriod;            
        }
    }
}
