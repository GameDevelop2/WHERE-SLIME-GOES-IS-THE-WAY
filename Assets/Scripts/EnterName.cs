using UnityEngine;
using UnityEngine.UI;
 
public class EnterName : MonoBehaviour
{
    public InputField playerNameInput;
    private string playerName = null;
 
    private void Awake()
    {
        playerName = playerNameInput.GetComponent<InputField>().text;
    }
 
    private void Update()
    {
        //키보드
        if (playerName.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
        }
    }
    public void InputName()
    {
        playerName = playerNameInput.text;
        PlayerPrefs.SetString("CurrentPlayerName", playerName);
        
    }
 
    
}
