using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameMenu is the menu for the game. It has all the necessary 
// game objects and operations to support the menu.
public class GameMenu : MonoBehaviour
{
    // game objects
    public Button PlayButton;
    public Slider PlayerCashSlider;
    public Slider BetRatioSlider;
    public TMP_Text PlayerCashValueText;
    public TMP_Text BetRatioValueText;

    // Awake is used to initialize any variables or game 
    // state before the game starts.
    private void Awake()
    {
        SetDefaultOptions(false);
    }

    // SetDefaultOptions will set the options to the default for the game. If the
    // function receives the override boolean then it will override what ever is
    // saved for the settings. If no settings have been made then it will set them
    // to the default.
    public void SetDefaultOptions(bool overRide)
    {
        // override
        // OR PLAYER_CASH DNE
        if (overRide || !PlayerPrefs.HasKey("PLAYER_CASH"))
        {
            PlayerPrefs.SetInt("PLAYER_CASH", Constants.DEFAULT_PLAYER_CASH);
        }

        // override
        // OR BET_RATIO DNE
        if (overRide || !PlayerPrefs.HasKey("BET_RATIO"))
        {
            PlayerPrefs.SetInt("BET_RATIO", Constants.DEFAULT_BET_RATIO);
        }

        // update the slider and text fields
        PlayerCashSlider.value = PlayerPrefs.GetInt("PLAYER_CASH");
        PlayerCashValueText.text = "$" + PlayerPrefs.GetInt("PLAYER_CASH").ToString();
        BetRatioSlider.value = PlayerPrefs.GetInt("BET_RATIO");
        BetRatioValueText.text = PlayerPrefs.GetInt("BET_RATIO").ToString() + ":1";
    }

    // PlayGame is used by the play button. It loads the game scene.
    public void PlayGame()
    {
        // load the game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // PlayGame is used by the quit button. It quits the application.
    public void QuitGame()
    {
        Application.Quit();
    }

    // SetCashPlayerPref is used by the player cash slider. The player cash 
    // slider has an onchange listener that will trigger this function. All the
    // function does is set the PLAYER_CASH player preference and set the
    // player cash text to whatever the player cash slider value is.
    public void SetCashPlayerPref(float value)
    {
        PlayerPrefs.SetInt("PLAYER_CASH", Mathf.RoundToInt(value));
        PlayerCashValueText.text = "$" + Mathf.RoundToInt(value).ToString();
    }

    // SetBetRatioPref is used by the bet ratio slider. The bet ratio 
    // slider has an onchange listener that will trigger this function. All the
    // function does is set the BET_RATIO player preference and set the
    // bet ratio text to whatever the bet ratio slider value is.
    public void SetBetRatioPref(float value)
    {
        PlayerPrefs.SetInt("BET_RATIO", Mathf.RoundToInt(value));
        BetRatioValueText.text = Mathf.RoundToInt(value).ToString() + ":1";
    }
}
