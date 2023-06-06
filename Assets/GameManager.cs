using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isGameClear;
    public TMP_Text timeText;
    private float playTime;
   
    private static GameManager instance;
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
    }

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

    public void GameClear()
    {
        isGameClear = true;
        SceneManager.LoadScene("Ranking");
    }

    private void Start()
    {
        isGameClear = false;

        InitializePlayTime();
    }

    private void Update()
    {
        if (!isGameClear)
        {
            playTime += Time.deltaTime;
            timeText.text = playTime.ToString("F1") + "sec";
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

}

