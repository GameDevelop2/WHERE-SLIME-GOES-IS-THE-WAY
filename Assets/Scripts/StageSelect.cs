using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private int movespeed;
    private Vector3 move_distance = new Vector3(20.0f, 0.0f, 0.0f);
    private Vector3 destination;
    private int MaxStageNum;
    private int StageNum;

    void Awake()
    {
        StageNum = 0;
        destination = transform.position;
        MaxStageNum = SceneManager.sceneCountInBuildSettings;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * movespeed);
    }

    void OnMoveLeft()
    {
        if(StageNum > 0)
        {
            StageNum--;
            destination -= move_distance;
        }
    }

    void OnMoveRight()
    {
        if(StageNum < MaxStageNum-1)
        {
            StageNum++;
            destination += move_distance;
        }
    }

    void OnLoadStage()
    {
        if(StageNum != 0)
            SceneManager.LoadScene(StageNum);
    }
}
