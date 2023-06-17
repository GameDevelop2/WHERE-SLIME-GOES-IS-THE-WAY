using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            
            //GameManager.Instance.GameClear();
            GameManager gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gamemanager.GameClear();
            
        }
    }
}
