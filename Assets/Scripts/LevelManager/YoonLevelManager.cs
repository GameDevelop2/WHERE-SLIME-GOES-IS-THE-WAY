using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoonLevelManager : MonoBehaviour
{
    [SerializeField] GameObject fallingRockObject;

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


}
