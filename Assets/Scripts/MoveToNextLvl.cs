using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLvl : MonoBehaviour
{
    public void LoadNextLevel()
    {
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalide Scene Index!");
            return;
        }

        GameManager.Instance.UnlockAndLoadNextLevel();
    }
}
