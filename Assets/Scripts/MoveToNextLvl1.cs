using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLvl1 : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalide Scene Index!");
            return;
        }

        if (nextLevelToLoad > PlayerPrefs.GetInt("levelAt"))
            PlayerPrefs.SetInt("levelAt", nextLevelToLoad);

        SceneManager.LoadScene(nextLevelToLoad);
    }
}
