using UnityEngine.UI;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Button[] gameplayLevelButtons;
    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt",2);
        for(int i = 0; i < gameplayLevelButtons.Length; i++)
        {
          if(i + 2 > levelAt)
          {
            gameplayLevelButtons[i].interactable = false;
          }
        }
    }
}
