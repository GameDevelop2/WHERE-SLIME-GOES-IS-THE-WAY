using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelect"); // "StageSelect"은 실제로 사용하는 씬 이름으로 대체해야 합니다.
    }
}
