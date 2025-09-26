using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ChangeScenes(int numberOfScene)
    {
        SceneManager.LoadScene(numberOfScene);
    }

    public void SetDifficulty(int difficultyLevel)
    {
        DifficultyManager.level = difficultyLevel;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
