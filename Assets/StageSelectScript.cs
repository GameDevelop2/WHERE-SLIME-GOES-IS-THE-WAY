using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScript: MonoBehaviour
{
    GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }

    public void GoToStageSelect()
    {
        gameManager.ResumeTime();
        SceneManager.LoadScene(0);
    }
}
