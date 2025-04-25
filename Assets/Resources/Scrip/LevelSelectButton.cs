using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public Level levelData;  
    public Button levelButton;
    public GameObject Lock;
    public GameObject Level;


    void Start()
    {
          int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); 

            if (levelData.levelNumber <= unlockedLevel)
            {
                levelButton.interactable = true;
                Level.SetActive(true);
                Lock.SetActive(false);

            }
            else
            {
                levelButton.interactable = false;
                Level.SetActive(false);
                Lock.SetActive(true);
        }
     

    }

    public void OnLevelButtonClicked()
    {
        LevelHolder.Instance.selectedLevel = levelData;
        SceneManager.LoadScene("Main");
    }
}
