using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoonLevelManager : MonoBehaviour
{
    [SerializeField] GameObject fallingRockPrefab;

    PlayerBehavior playerBehaviorComponent;

    void Awake()
    {
        playerBehaviorComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
    }
    
    public void RespawnPlayerIfContacted(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerBehaviorComponent.Respawn();
    }

    public void SpawnFallingRock(GameObject spawner)
    {
        Instantiate<GameObject>(fallingRockPrefab, spawner.transform.position, fallingRockPrefab.transform.rotation);
    }

    public void OnFallingRockCollision(Collider2D collider, GameObject rockObject)
    {
        if (collider.CompareTag("Player"))
            playerBehaviorComponent.Respawn();
        else
            Destroy(rockObject);
    }
}
