using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageSelectScript: MonoBehaviour
{
    public void GoToStageSelect()
    {
        SceneManager.LoadScene(0);
    }
}
