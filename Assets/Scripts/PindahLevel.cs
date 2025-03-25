using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahLevel : MonoBehaviour
{
    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D coll){
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalide Scene Index!");
            return;
        }

        GameManager.Instance.UnlockAndLoadNextLevel();
    }
}
