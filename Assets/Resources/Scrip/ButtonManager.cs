using UnityEngine;
using UnityEngine.SceneManagement; 

public class ButtonManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Level");
        Debug.Log("click");
    }
}
