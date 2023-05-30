using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoonLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject fallingRockObject;

    private PlayerBehavior playerBehaviorComponent;
    private Transform exitPortalTransform;

    void Awake()
    {
        playerBehaviorComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Portal"))
        {
            if (gameObject.name.Equals("ExitPortal"))
            {
                exitPortalTransform = gameObject.transform;
                break;
            }
        }
    }
    
    public void RespawnPlayerIfContacted(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerBehaviorComponent.Respawn();
    }

    public void SpawnFallingRock(GameObject spawner)
    {
        // 부하 심할 시 오브젝트 풀링 방식으로 변경
        GameObject fallingRockCopy = Instantiate<GameObject>(fallingRockObject, spawner.transform.position, fallingRockObject.transform.rotation);
        fallingRockCopy.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void OnFallingRockCollision(Collider2D collider, GameObject rockObject)
    {
        if (collider.CompareTag("Player"))
            playerBehaviorComponent.Respawn();
        else
            Destroy(rockObject, 0.03f);
    }

    public void OnEnterPortalContact(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerBehaviorComponent.gameObject.transform.position = exitPortalTransform.position;
    }
}
