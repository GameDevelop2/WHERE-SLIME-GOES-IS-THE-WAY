using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private bool isGameClear;
    private bool isTimeStopped;
    public TMP_Text timeText;
    private float playTime;
    
    private TMP_Text rankingText;
    public GameObject GameClearImage;
    public TMP_Text rankingTitle;

    public GameObject itemScore;
    

   
    /*private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("GameManager");
                    instance = managerObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    } */

    private void Awake()
    {
        /*if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); */
    } 


    private void Start()
    {
        isGameClear = false;
        isTimeStopped = false;
        GameClearImage.SetActive(false);
        rankingText = GameClearImage.GetComponent<TMP_Text>();

        InitializePlayTime();
    }

    private void Update()
    {
        if (!isGameClear && !isTimeStopped)
        {
            
            playTime += Time.deltaTime;
            timeText.text = playTime.ToString("F1") + " sec";
        }
    }
    public void GameClear()
    {
        StopTime();
        isGameClear = true;
        ItemPlacementManager itemplacementmanager = GameObject.Find("ItemPlaceManager").GetComponent<ItemPlacementManager>();
        RankingManager.Instance.DisplayTotalScore(playTime, itemplacementmanager.totalScore); 
        RankingManager.Instance.UpdateRanking(playTime, itemplacementmanager.totalScore); 
        RankingManager.Instance.DisplayRanking(); 
        GameClearImage.SetActive(true);
        GameObject gameClearUI = GameObject.FindWithTag("GameClearUI");
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("StageSelect");
        }

    }


    private void InitializePlayTime()
    {
        playTime = 0f;
        timeText.text = "0s";
    }

    public float GetPlayTime()
    {
        return playTime;
    }
    public void StopTime()
    {
        isTimeStopped = true;
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        isTimeStopped = false;
        Time.timeScale = 1f;
    }

}