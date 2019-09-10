using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button PlayButton;
    public Slider PlayerCashSlider;
    public Slider BetRatioSlider;
    public TMP_Text PlayerCashValueText;
    public TMP_Text BetRatioValueText;

    private void Awake()
    {
        SetDefaultOptions(false);
    }

    public void SetDefaultOptions(bool Override)
    {
        if (Override || !PlayerPrefs.HasKey("PLAYER_CASH"))
        {
            PlayerPrefs.SetInt("PLAYER_CASH", Constants.DEFAULT_PLAYER_CASH);
        }

        if (Override || !PlayerPrefs.HasKey("BET_RATIO"))
        {
            PlayerPrefs.SetInt("BET_RATIO", Constants.DEFAULT_BET_RATIO);
        }

        PlayerCashSlider.value = PlayerPrefs.GetInt("PLAYER_CASH");
        PlayerCashValueText.text = "$" + PlayerPrefs.GetInt("PLAYER_CASH").ToString();
        BetRatioSlider.value = PlayerPrefs.GetInt("BET_RATIO");
        BetRatioValueText.text = PlayerPrefs.GetInt("BET_RATIO").ToString() + ":1";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // SetCashPlayerPref is used by the cash slider. The bet slider has a 
    // onchange listener that will trigger this function. All the
    // function does is set the bet text to whatever the bet slider
    // value is.
    public void SetCashPlayerPref(float value)
    {
        PlayerPrefs.SetInt("PLAYER_CASH", Mathf.RoundToInt(value));
        PlayerCashValueText.text = "$" + Mathf.RoundToInt(value).ToString();
    }

    // SetBetRatioPref is used by the cash slider. The bet slider has a 
    // onchange listener that will trigger this function. All the
    // function does is set the bet text to whatever the bet slider
    // value is.
    public void SetBetRatioPref(float value)
    {
        PlayerPrefs.SetInt("BET_RATIO", Mathf.RoundToInt(value));
        BetRatioValueText.text = Mathf.RoundToInt(value).ToString() + ":1";
    }
}
