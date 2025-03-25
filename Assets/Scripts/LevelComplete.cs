using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public void LoadNextLevel(){
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextLevelToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalide Scene Index!");
            return;
        }

        if (nextLevelToLoad > PlayerPrefs.GetInt("levelAt"))
            PlayerPrefs.SetInt("levelAt", nextLevelToLoad);

        SceneManager.LoadScene(nextLevelToLoad);
    }
}
