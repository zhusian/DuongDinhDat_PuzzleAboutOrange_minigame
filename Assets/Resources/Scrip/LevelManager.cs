using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int currentLevel = 1; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public bool IsLevelUnlocked(int level)
    {
        return PlayerPrefs.GetInt("UnlockedLevel", 1) >= level; 
    }
}
