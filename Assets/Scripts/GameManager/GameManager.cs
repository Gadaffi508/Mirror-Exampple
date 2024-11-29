using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        
        SceneManager.LoadScene("Work");
    }
    
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Main");

        Screen.SetResolution(720, 480, false);
    }
}
