using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Level");
    }
    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnNextButtonClicked()
    {
        SceneManager.LoadScene("Level");
    }
}
