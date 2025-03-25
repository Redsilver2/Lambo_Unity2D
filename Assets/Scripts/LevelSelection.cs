using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button[] gameplayLevelButtons;
    // Start is called before the first frame update
    void Start()
    {
        int buildIndex      = SceneManager.GetActiveScene().buildIndex;
        GameManager manager = GameManager.Instance;

        if (manager != null)
        {
            manager.SetLevelButtonsAndAddLevels(gameplayLevelButtons,
                                                buildIndex, true);
        }
    }
}
