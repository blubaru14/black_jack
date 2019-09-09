using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button PlayButton;
    public InputField PlayerCashInputField;

    private void Awake()
    {
        PlayButton.interactable = false;
    }

    public void PlayGame()
    {
        Debug.Log(PlayerCashInputField.text);
        PlayerPrefs.SetInt("playerCash", int.Parse(PlayerCashInputField.text));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CheckPlayerCash(string text)
    {
        // if the player cash is not set
        if (string.IsNullOrEmpty(text) || int.Parse(text) == 0)
        {
            PlayButton.interactable = false;
        }
        // player cash is set
        else
        {
            PlayButton.interactable = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
