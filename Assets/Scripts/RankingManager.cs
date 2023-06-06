using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public TMP_Text rankingText;
    public GameObject rankingPanel;

    private static RankingManager instance;
    public static RankingManager Instance
    {
        get { return instance; }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public float playTime;
    }

    private List<PlayerData> rankingData = new List<PlayerData>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateRanking(float score, string playerName)
    {
        PlayerData playerData = new PlayerData();
        playerData.playerName = playerName;
        playerData.playTime = score;

        rankingData.Add(playerData);
        rankingData.Sort((a, b) => a.playTime.CompareTo(b.playTime));

        if (rankingData.Count > 5)
        {
            rankingData.RemoveAt(rankingData.Count - 1);
        }

        SaveRankingData();
        DisplayRanking();
    }

    public void DisplayRanking()
    {
        string rankingText = "";
        for (int i = 0; i < rankingData.Count; i++)
        {
            rankingText += (i + 1) + " - " + rankingData[i].playerName + ": " + rankingData[i].playTime.ToString("F2") + " sec\n";
        }

        this.rankingText.text = rankingText;
    }

    private void LoadRankingData()
    {
        rankingData.Clear();
        for (int i = 0; i < 5; i++)
        {
            string playerName = PlayerPrefs.GetString("RankingPlayerName_" + i);
            float playTime = PlayerPrefs.GetFloat("RankingPlayTime_" + i);

            if (!string.IsNullOrEmpty(playerName))
            {
                PlayerData playerData = new PlayerData();
                playerData.playerName = playerName;
                playerData.playTime = playTime;
                rankingData.Add(playerData);
            }
        }
    }

    private void SaveRankingData()
    {
        for (int i = 0; i < rankingData.Count; i++)
        {
            PlayerPrefs.SetString("RankingPlayerName_" + i, rankingData[i].playerName);
            PlayerPrefs.SetFloat("RankingPlayTime_" + i, rankingData[i].playTime);
        }

        PlayerPrefs.Save();
    }

    private void Start()
    {
        LoadRankingData();
        DisplayRanking();
    }
}




