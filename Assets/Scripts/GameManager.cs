using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LevelNode
{
    public LevelNode     next;
    public GameplayLevel mainLevel;
    public SecretLevel   secretLevel;

    public LevelNode(GameplayLevel mainLevel, SecretLevel secretLevel)
    {
        this.mainLevel   = mainLevel;
        this.secretLevel = secretLevel;
    }
}

[System.Serializable]
public abstract class LevelUnlocked
{
    private   int levelIndex;
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
        if(PlayerPrefs.GetInt("levelAt") >= LevelIndex)
        {
            isUnlocked = true;
            Debug.Log(PlayerPrefs.GetInt("levelAt"));
        }
    }
}

[System.Serializable]
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
    private LevelNode defaultLevel;
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

        if (!PlayerPrefs.HasKey("levelAt"))
                PlayerPrefs.SetInt("levelAt", 2);
        else if (PlayerPrefs.GetInt("levelAt") < 2)
            PlayerPrefs.SetInt("levelAt", 2);

        defaultLevelsUnlocked = new List<int>()
        {
            0, 1, 2, 9, 10, 11
        };

        SecretLevel.SetSecretLevelIndex();
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
    }

    public void SetLevelButtonAndAddLevel(Button button, int levelIndex, bool isGameplayLevel)
    {
        if (button == null) return;

        AddLevelNode(new GameplayLevel(levelIndex));
        StartCoroutine(SetMainLevelButton(button, levelIndex));
    }

    private IEnumerator SetMainLevelButton(Button button, int levelIndex)
    {
        LevelNode currentNode = defaultLevel;
        button.onClick.RemoveAllListeners();

        while (currentNode != null)
        {
            if(currentNode.mainLevel != null)
            {
                if (currentNode.mainLevel.LevelIndex == levelIndex && button != null)
                {

                    currentNode.mainLevel.Unlock();
                    button.onClick.AddListener(() =>
                    {
                        LoadLevel(currentNode.mainLevel);
                    });

                    button.interactable = currentNode.mainLevel.IsUnlocked;
                    Debug.Log(currentNode.mainLevel.IsUnlocked);
                    break;
                }
            }

            currentNode = currentNode.next;
            yield return null;
        }
    }

    private IEnumerator SetSecretLevelButton(Button button, int levelIndex)
    {
        LevelNode currentNode = defaultLevel;
        button.onClick.RemoveAllListeners();

        while (currentNode != null)
        {
            if (currentNode.mainLevel != null)
            {
                if (currentNode.mainLevel.LevelIndex == levelIndex)
                {

                    currentNode.mainLevel.Unlock();
                    button.onClick.AddListener(() =>
                    {
                        LoadLevel(currentNode.mainLevel);
                    });

                    button.interactable = currentNode.mainLevel.IsUnlocked;
                    Debug.Log(currentNode.mainLevel.IsUnlocked);
                    break;
                }
            }

            currentNode = currentNode.next;
            yield return null;
        }
    }

    private IEnumerator LoadAndUnlockNextMainLevel(int levelIndex)
    {
        LevelNode currentNode = defaultLevel;

        while (currentNode != null)
        {
            if (currentNode.mainLevel != null)
            {
                if (currentNode.mainLevel.LevelIndex == levelIndex)
                {
                    currentNode.mainLevel.SetUnlockedState(true);
                    LoadLevel(currentNode.mainLevel);
                    break;
                }
            }

            currentNode = currentNode.next;
            yield return null;
        }
    }

    public void AddLevelNode(GameplayLevel gameplayLevel)
    {
        AddLevelNode(gameplayLevel, null);
    }

    private void AddLevelNode(GameplayLevel gameplayLevel, SecretLevel secretLevel)
    {
        if (gameplayLevel == null) return;
        LevelNode nextNode = new LevelNode(gameplayLevel, secretLevel);

        if (defaultLevel == null)
        {
            defaultLevel = nextNode;
        }
        else
        {
            AddLevelNode(defaultLevel, nextNode);
        }
    }
    private void AddLevelNode(LevelNode currentLevelNode, LevelNode nextNode)
    {
        if (currentLevelNode.next != null)
        {
            AddLevelNode(currentLevelNode.next, nextNode);
        }
        else
        {
            currentLevelNode.next = nextNode;
        }
    }

    public void RemoveLevelNode(int sceneIndex)
    {
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings) return;
        RemoveLevelNode(sceneIndex, defaultLevel);
    }
    public void RemoveLevelNode(int sceneIndex, LevelNode node)
    {
        if (node != null)
        {
            if (node.mainLevel.LevelIndex == sceneIndex)
            {
                node.next = null;
            }
            else
            {
                RemoveLevelNode(sceneIndex, node.next);
            }
        }
    }

    public void AddSecretLevel(int sceneIndex, SecretLevel secretLevel)
    {
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings) return;
    }

    public void AddSecretLevel(int sceneIndex, SecretLevel level, LevelNode node)
    {
        if (node != null)
        {
            if (node.mainLevel == null)
            {
                if (node.mainLevel.LevelIndex == sceneIndex)
                {
                    node.secretLevel = level;
                }
            }

            AddSecretLevel(sceneIndex, level, node.next);
        }
    }
    public void RemoveSecretLevel(int sceneIndex, LevelNode node)
    {
        if (node != null)
        {
            if (node.mainLevel == null)
            {
                if (node.mainLevel.LevelIndex == sceneIndex)
                {
                    node.secretLevel = null;
                }
            }

            RemoveLevelNode(sceneIndex, node.next);
        }
    }

    private void IsMainLevelUnlocked(LevelNode node, int levelIndex, ref bool isUnlocked)
    {
        if (node != null)
        {
            if (node.mainLevel != null)
            {
                if (node.mainLevel.LevelIndex == levelIndex)
                {
                    isUnlocked = node.mainLevel.IsUnlocked;
                    return;
                }
            }

            IsMainLevelUnlocked(node.next, levelIndex, ref isUnlocked);
        }

        isUnlocked = false;
    }

    public bool IsMainLevelUnlocked(int levelIndex)
    {
        bool isUnlocked = false;
        IsMainLevelUnlocked(defaultLevel, levelIndex, ref isUnlocked);
        return isUnlocked;
    }

    private void GetMainLevel(LevelNode node, int levelIndex, ref GameplayLevel level)
    {
        if (node != null)
        {
            if (node.mainLevel != null)
            { 
                if (node.mainLevel.LevelIndex == levelIndex)
                {
                    level = node.mainLevel;
                    return;
                }
            }

            GetMainLevel(node.next, levelIndex, ref level);
        }

        level = null;
    }

    public GameplayLevel GetMainLevel(int levelIndex)
    {
        GameplayLevel level = null;
        GetMainLevel(defaultLevel, levelIndex, ref level);
        return level;
    }



    public void UnlockAndLoadNextLevel()
    {
        StartCoroutine(LoadAndUnlockNextMainLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLevel(string name)
    {
       SceneManager.LoadScene(name);
    }

    public void LoadLevel(int levelIndex)
    {
        if (defaultLevelsUnlocked.Contains(levelIndex))
            SceneManager.LoadScene(levelIndex);
        else
        {
            LevelUnlocked level = GetMainLevel(levelIndex);

            if (level != null)
                LoadLevel(level);
        }
    }

    private void LoadLevel(LevelUnlocked levelUnlocked)
    {
        if (levelUnlocked != null)
            if (levelUnlocked.IsUnlocked)
                SceneManager.LoadScene(levelUnlocked.LevelIndex);
    }
}
