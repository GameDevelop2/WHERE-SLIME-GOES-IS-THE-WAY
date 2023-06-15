using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public TMP_Text rankingText;
    public GameObject rankingPanel;
    public TMP_Text totalScoreText;

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
        public int itemScore;
        public float totalScore;
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

    public void UpdateRanking(float timeScore, int itemScore)
    {
        PlayerData playerData = new PlayerData();
        playerData.playTime = timeScore;
        playerData.itemScore = itemScore;
        playerData.totalScore = timeScore + itemScore;

        rankingData.Add(playerData);
        rankingData.Sort((a, b) => a.totalScore.CompareTo(b.totalScore)); // 오름차순으로 정렬 (낮은 스코어가 상위 순위)

        if (rankingData.Count > 5)
        {
            rankingData.RemoveAt(rankingData.Count - 1);
        }

        SaveRankingData();
        DisplayRanking();
    }

    public void DisplayTotalScore(float timeScore, int itemScore)
    {
        string totalScoreText = "   Time: " + timeScore.ToString("F2") + "\n"
            + " + Item: " + itemScore.ToString("F2") + "\n"
            + "--------------------" + "\n"
            + "  Total: " + (timeScore + itemScore).ToString("F2");

        this.totalScoreText.text = totalScoreText;
    }

    public void DisplayRanking()
    {
        string rankingText = "";
        for (int i = 0; i < rankingData.Count; i++)
        {
            
            rankingText += (i + 1) + ": " + rankingData[i].totalScore.ToString("F2") + "\n";
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
            float totalScore = PlayerPrefs.GetFloat("RankingTotalScore_" + i); // 토탈 스코어 불러오기

        
            PlayerData playerData = new PlayerData();
            playerData.playerName = playerName;
            playerData.playTime = playTime;
            playerData.totalScore = totalScore; // 토탈 스코어 저장
            rankingData.Add(playerData);
        
        }
    }

    private void SaveRankingData()
    {
        for (int i = 0; i < rankingData.Count; i++)
        {
            PlayerPrefs.SetString("RankingPlayerName_" + i, rankingData[i].playerName);
            PlayerPrefs.SetFloat("RankingPlayTime_" + i, rankingData[i].playTime);
            PlayerPrefs.SetFloat("RankingTotalScore_" + i, rankingData[i].totalScore); // 토탈 스코어 저장
        }

        PlayerPrefs.Save();
    }
}
