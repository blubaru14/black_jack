using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameController is a the controller for the game. It has all the
// necessary game objects and operations to support the game.
public class GameController : MonoBehaviour
{
    private bool _HideDealerScore = false;
    private int _PlayerCash;
    private int _BetRatio;

    // game objects
    public CardStack Deck;
    public CardStack Dealer;
    public CardStack Player;
    public Slider BetSlider;
    public Button HitButton;
    public Button StandButton;
    public Button DealButton;
    public TMP_Text PlayerCashValueText;
    public TMP_Text BetValueText;
    public TMP_Text PlayerValueText;
    public TMP_Text DealerValueText;
    public TMP_Text ResultsText;
    
    // Start is called on the frame when a script is enabled just 
    // before any of the Update methods are called the first time.
    private void Start()
    {
        // disable these buttons since no round is going on
        HitButton.interactable = false;
        StandButton.interactable = false;
        // enable these since they are needed to start a round
        DealButton.interactable = true;
        BetSlider.interactable = true;

        // blank the results text
        ResultsText.text = "";

        // get the player cash and bet ratio from the player prefs 
        // set in game menu scene
        _PlayerCash = PlayerPrefs.GetInt("PLAYER_CASH");
        _BetRatio = PlayerPrefs.GetInt("BET_RATIO");
        // update the max bet value to the players current cash
        BetSlider.maxValue = _PlayerCash;
        BetSlider.value = 0;

        // clear the card stacks
        Deck.Clear();
        Dealer.Clear();
        Player.Clear();
    }

    // Update is called once per frame and used to update objects.
    private void Update()
    {
        // update the players cash value
        PlayerCashValueText.text = "$" + _PlayerCash.ToString();
        // update the players score aka current card stack value
        PlayerValueText.text = Player.Value().ToString();

        // if it is the players turn
        if (_HideDealerScore)
        {
            // update the dealers score aka current card stack value
            // hide the rank of the dealers 2nd card (aka index 1)
            DealerValueText.text = (Dealer.Value() - Dealer.GetCard(1).GetComponent<Card>().Rank()).ToString() + " + ?";
        }
        // it is the dealers turn
        else
        {
            // update the dealers score aka current card stack value
            DealerValueText.text = Dealer.Value().ToString();
        }
    }

    // DealersTurn is a coroutine that gets kick off after the player stands
    // or has a card stack value >= 21. It will execute the automated dealer
    // hit/stand. After the dealers hand value > 17 then it will decide the
    // turn out of the round.
    private IEnumerator DealersTurn()
    {
        // disable these buttons since it is not longer the players turn
        HitButton.interactable = false;
        StandButton.interactable = false;

        // since it is the dealers turn then don't hide their score or cards
        _HideDealerScore = false;
        Dealer.GetCard(1).GetComponent<Card>().ToggleFace(true);

        // loop will the dealers card stack value is less than 17
        while (Dealer.Value() < 17)
        {
            // wait 1 sec in between cards for suspense
            yield return new WaitForSeconds(1);
            Dealer.AddCard(Deck.RemoveCard(), true);
        }

        // dealer has 21 or less 
        // AND (player has 21 or more 
        //      OR dealer has higher value than player)
        if (Dealer.Value() <= 21 && (Player.Value() > 21 || Dealer.Value() > Player.Value()))
        {
            ResultsText.text = "Dealer Wins";
        }
        // player has 21 or less 
        // AND (dealer has 21 or more 
        //      OR player has higher value than dealer)
        else if (Player.Value() <= 21 && (Dealer.Value() > 21 || Player.Value() > Dealer.Value()))
        {
            // give the player back their bet * bet ratio
            _PlayerCash += Mathf.RoundToInt(BetSlider.value * _BetRatio);
            ResultsText.text = "Player Wins";
        }
        // player has 21 or less
        // AND dealer has 21 or less 
        // AND player and dealer have equal value
        else if (Player.Value() <= 21 && Player.Value() <= 21 && Player.Value() == Dealer.Value())
        {
            // give the player back their bet
            _PlayerCash += Mathf.RoundToInt(BetSlider.value);
            ResultsText.text = "Draw";
        }
        // player and dealer have over 21
        else
        {
            ResultsText.text = "House Wins";
        }

        // force the player to restart if they have zero cash
        if (_PlayerCash > 0)
        {
            // enable these since they are needed to start a round
            BetSlider.interactable = true;
            DealButton.interactable = true;
        }
        
        // update the max bet value to the players current cash
        BetSlider.maxValue = _PlayerCash;
        BetSlider.value = 0;
    }

    // Deal is used by the deal button. Before the function is called the
    // player will set their bet. The player and dealer will get their
    // initial 2 cards.
    public void Deal()
    {
        // enable these buttons since it is the players turn
        HitButton.interactable = true;
        StandButton.interactable = true;
        // disable these since we do not want them used mid round
        BetSlider.interactable = false;
        DealButton.interactable = false;

        // new round so blank the results text
        ResultsText.text = "";

        // hide the dealers score since it is the players turn
        _HideDealerScore = true;
        // subtract the players bet from their cash
        _PlayerCash -= Mathf.RoundToInt(BetSlider.value);

        // reset the deck if there is less than 14 cards
        // 14 is magic number here. The logic is that 14 cards is the 
        // max amount of cards to get a game total of 42. e.g. 
        // 4 2s + 4 3s + 4 4s + 2 5s = 46
        if(Deck.GetCardCount() < 14)
        {
            // clear and make a new deck
            Deck.Clear();
            Deck.MakeDeck();
        }
        
        // clear the card stacks
        Dealer.Clear();
        Player.Clear();

        // loop 2 times
        for (int i = 1; i <= 2; i++)
        {
            // give the player a card face up
            Player.AddCard(Deck.RemoveCard(), true);

            // if it is the 2nd card 
            if (i == 2)
            {
                // give the dealer a card face down
                Dealer.AddCard(Deck.RemoveCard(), false);
            }
            // first card
            else
            {
                // give the dealer a card face up
                Dealer.AddCard(Deck.RemoveCard(), true);
            }
        }

        // players hand is 21 so jump to dealers turn
        if (Player.Value() == 21)
        {
            StartCoroutine(DealersTurn());
        }
    }

    // Restart is used by the hit button. It gives the player a card.
    public void Hit()
    {
        Player.AddCard(Deck.RemoveCard(), true);

        // players hand is 21 or more so don't continue players turn
        if(Player.Value() >= 21)
        {
            StartCoroutine(DealersTurn());
        }
    }

    // Restart is used by the stand button. It kicks off the dealers turn.
    public void Stand()
    {
        StartCoroutine(DealersTurn());
    }

    // Restart is used by the exit button. It loads the game menu scene.
    public void Exit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // SetBet is used by the bet slider. The bet slider has a 
    // onchange listener that will trigger this function. All the
    // function does is set the bet text to whatever the bet slider
    // value is.
    public void SetBetText(float value)
    {
        BetValueText.text = "$" + Mathf.RoundToInt(value).ToString();
    }
}
