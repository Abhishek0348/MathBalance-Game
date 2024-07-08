using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLevel(int levelid)
    {
        string levelName = "Level_" + levelid;
        SceneManager.LoadScene(levelName);
    }
}
