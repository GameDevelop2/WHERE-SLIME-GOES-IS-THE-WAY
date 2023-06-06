using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnterNameRanking : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    private string playerName = null;
    public RankingManager rankingManager;

    private void Awake()
    {
        if (playerNameInput != null)
        {
            playerName = playerNameInput.text;
        }
    }

    private void Update()
    {
        if (playerName.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
        }
    }

    public void InputName()
    {
        playerName = playerNameInput.text;
        PlayerPrefs.SetString("CurrentPlayerName", playerName);
        RankingManager.Instance.UpdateRanking(GameManager.Instance.GetPlayTime(), playerName);
        RankingManager.Instance.DisplayRanking(); 
    }
}

