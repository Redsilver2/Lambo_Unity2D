using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public abstract class LevelUnlocked
{
    private int levelIndex;
    protected bool isUnlocked;

    public int LevelIndex { get { return levelIndex; } }
    public bool IsUnlocked { get { return isUnlocked; } }

    public LevelUnlocked(int levelIndex)
    {
        this.levelIndex = levelIndex;
    }


    public LevelUnlocked(int levelIndex, bool isUnlocked)
    {
        this.levelIndex = levelIndex;
        this.isUnlocked = isUnlocked;
    }
    public abstract void Unlock();
}

[System.Serializable]
public class GameplayLevel : LevelUnlocked
{
    public GameplayLevel(int levelIndex) : base(levelIndex)
    {
        this.isUnlocked = false;
    }

    public GameplayLevel(int levelIndex, bool isUnlocked) : base(levelIndex, isUnlocked)
    {
    }

    public virtual void SetUnlockedState(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
    }

    public override void Unlock()
    {
        isUnlocked = LevelIndex > PlayerPrefs.GetInt("levelAt", 2);
    }
}

public class SecretLevel : GameplayLevel
{
    private int secretLevelRequirement;
    private static int secretLevelUnlockedCount = 0;

    public SecretLevel(int levelIndex) : base(levelIndex)
    {
    }


    public SecretLevel(int levelIndex, int secretLevelRequirement) : base(levelIndex)
    {
        this.secretLevelRequirement = secretLevelRequirement;
    }


    public SecretLevel(int levelIndex, bool isUnlocked) : base(levelIndex, isUnlocked)
    {
    }


    public SecretLevel(int levelIndex, int secretLevelRequirement, bool isUnlocked) : base(levelIndex, isUnlocked)
    {
        this.secretLevelRequirement = secretLevelRequirement;
    }

    public override void Unlock()
    {
        if (secretLevelUnlockedCount > secretLevelRequirement)
            isUnlocked = true;
    }

    public static void SetSecretLevelIndex()
    {
        secretLevelUnlockedCount = PlayerPrefs.GetInt("secretLevel", 0);
    }

    public static void IncreaseUnlockedCount()
    {
        secretLevelUnlockedCount++;
        PlayerPrefs.SetInt("secretLevel", secretLevelUnlockedCount);
    }
}


public class GameManager : MonoBehaviour
{
    private List<LevelUnlocked> levelsUnlocked;
    private List<int> defaultLevelsUnlocked;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        defaultLevelsUnlocked = new List<int>()
        {
            0, 1, 2, 9, 10, 11
        };

        PlayerPrefs.SetInt("levelAt", 0);
        SecretLevel.SetSecretLevelIndex();
        levelsUnlocked = new List<LevelUnlocked>();
    }

    public void SetLevelButtonsAndAddLevels(Button[] buttons, int startIndex, bool isGameplayLevel)
    {
        if (buttons == null) return;

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            int levelIndex = i + 2;

            if (levelIndex >= SceneManager.sceneCountInBuildSettings) break;
            SetLevelButtonAndAddLevel(button, levelIndex, isGameplayLevel);
        }

        foreach (LevelUnlocked levelUnlocked in levelsUnlocked)
            Debug.Log(levelUnlocked.IsUnlocked);
    }

    public void SetLevelButtonAndAddLevel(Button button, int levelIndex, bool isGameplayLevel)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            LoadLevel(levelIndex);
        });

        if (isGameplayLevel)
            AddGameplayLevel(levelIndex);
        else
            AddSecretLevel(levelIndex);

        bool isLevelUnlocked = IsLevelUnlocked(levelIndex);

        Debug.Log(isLevelUnlocked);

        button.interactable = isLevelUnlocked;
        if (!isGameplayLevel) button.gameObject.SetActive(isLevelUnlocked);
    }

    private void AddGameplayLevel(int levelIndex)
    {
        LevelUnlocked level = null;

        if (!TryGetLevel(levelIndex, ref level) && levelsUnlocked != null)
            AddLevel(new GameplayLevel(levelIndex, defaultLevelsUnlocked.Contains(levelIndex)));
    }

    private void AddSecretLevel(int levelIndex)
    {
        LevelUnlocked level = null;

        if (!TryGetLevel(levelIndex, ref level) && levelsUnlocked != null)
            AddLevel(new SecretLevel(levelIndex, 1, defaultLevelsUnlocked.Contains(levelIndex)));
    }

    private void AddLevel(GameplayLevel level)
    {
        if (level != null)
        {
            if (level.IsUnlocked)
                level.Unlock();

            levelsUnlocked.Add(level);
        }
    }
    private void AddLevel(SecretLevel level)
    {
        if (level != null)
        {
            if (level.IsUnlocked)
                level.Unlock();

            levelsUnlocked.Add(level);
        }
    }

    public void RemoveLevel(int levelIndex)
    {
        foreach (LevelUnlocked levelUnlocked in levelsUnlocked)
        {
            if (levelUnlocked.LevelIndex == levelIndex)
            {
                levelsUnlocked.Remove(levelUnlocked);
                break;
            }
        }
    }

    public void UnlockAndLoadNextLevel()
    {
        LevelUnlocked level = null;

        if (TryGetLevel(SceneManager.GetActiveScene().buildIndex + 1, ref level))
        {
            level.Unlock();
            LoadLevel(level);
        }
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        LevelUnlocked level = null;

        if (TryGetLevel(levelIndex, ref level))
            return level.IsUnlocked;

        return false;
    }

    private bool TryGetLevel(int levelIndex, ref LevelUnlocked level)
    {
        if (levelIndex < SceneManager.sceneCountInBuildSettings && levelsUnlocked.Count > 0)
        {
            LevelUnlocked[] levelsUnlocked = this.levelsUnlocked.Where(x => x.LevelIndex == levelIndex).ToArray();

            if (levelsUnlocked.Length > 0)
            {
                level = levelsUnlocked[0];
                return true;
            }
        }

        level = null;
        return false;
    }

    public void LoadLevel(string name)
    {
       SceneManager.LoadScene(name);
    }

    public void LoadLevel(int levelIndex)
    {
        LevelUnlocked level = null;

        if (TryGetLevel(levelIndex, ref level))
            LoadLevel(level);
    }

    private void LoadLevel(LevelUnlocked levelUnlocked)
    {
        if (levelUnlocked != null)
            if (levelUnlocked.IsUnlocked)
                SceneManager.LoadScene(levelUnlocked.LevelIndex);
    }
}
