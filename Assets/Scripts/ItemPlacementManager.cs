using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ItemPlacementManager : MonoBehaviour
{
    public float timeThreshold = 100f; // 키 입력을 유효하게 인식하는 시간 임계값
    private float lastKeyPressTime; // 마지막으로 키를 누른 시간
    private KeyCode lastKeyCode; // 마지막으로 누른 키의 KeyCode

    public TMP_Text scoreText; // 점수를 표시할 UI 텍스트(Text) 요소

    public int totalScore; // 총 합산 점수

    private void Start()
    {
        UpdateScoreText();
    }

    private void Update()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            if (Time.time - lastKeyPressTime <= timeThreshold)
            {
                int score = GetScoreByKeyCode(lastKeyCode);
                AddScore(score);
                Debug.Log("Point used: " + score);
            }
        }

        // 키 입력 처리
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            lastKeyPressTime = Time.time;
            lastKeyCode = KeyCode.S;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            lastKeyPressTime = Time.time;
            lastKeyCode = KeyCode.D;
        }
        else if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            lastKeyPressTime = Time.time;
            lastKeyCode = KeyCode.F;
        }
        else if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            lastKeyPressTime = Time.time;
            lastKeyCode = KeyCode.G;
        }
        else if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            lastKeyPressTime = Time.time;
            lastKeyCode = KeyCode.Z;
        }
    }
    
    private void AddScore(int score)
    {
        totalScore += score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    private void GameClear()
    {
        Debug.Log("Total Score: " + totalScore);
        // 게임 클리어 처리 및 점수 초기화 등의 로직
    }

    private int GetScoreByKeyCode(KeyCode keyCode)
    {
        // 키에 따라 점수를 반환하는 로직을 구현
        // 예시:
        if (keyCode == KeyCode.S)
            return 10;
        else if (keyCode == KeyCode.D)
            return 20;
        else if (keyCode == KeyCode.F)
            return 30;
        else if (keyCode == KeyCode.G)
            return 40;
        else if (keyCode == KeyCode.Z)
            return 50;
    
        return 0;
    }
}
