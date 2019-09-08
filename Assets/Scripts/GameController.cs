using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private bool HideDealerScore = false;
    public CardStack Deck;
    public CardStack Dealer;
    public CardStack Player;
    public Button HitButton;
    public Button StandButton;
    public Button PlayButton;
    public Text PlayerScore;
    public Text DealerScore;
    public Text Results;

    public void Hit()
    {
        Player.AddCard(Deck.RemoveCard(), true);

        if(Player.Value() > 21)
        {
            StartCoroutine(DealersPlay());
        }
    }

    public void Stand()
    {
        StartCoroutine(DealersPlay());
    }

    public void Play()
    {
        HitButton.interactable = true;
        StandButton.interactable = true;
        HideDealerScore = true;

        Results.text = "";

        Deck.Clear();
        Dealer.Clear();
        Player.Clear();

        Deck.MakeDeck();

        for(int i = 0; i < 2; i++)
        {
            Player.AddCard(Deck.RemoveCard(), true);

            //if it is the second card for the dealer then make it face down
            if (i == 1)
            {
                Dealer.AddCard(Deck.RemoveCard(), false);
            }
            else
            {
                Dealer.AddCard(Deck.RemoveCard(), true);
            }            
        }
    }

    IEnumerator DealersPlay()
    {
        HitButton.interactable = false;
        StandButton.interactable = false;
        HideDealerScore = false;

        // get the dealers 2nd card (index 1) and show the face
        Dealer.GetCard(1).GetComponent<Card>().ToggleFace(true);

        while (Dealer.Value() < 17)
        {
            yield return new WaitForSeconds(1);
            Dealer.AddCard(Deck.RemoveCard(), true);
        }

        //dealer has 21 or less AND (player has 21 or more OR dealer has higher value than player)
        if(Dealer.Value() <= 21 && (Player.Value() > 21 || Dealer.Value() > Player.Value()))
        {
            Results.text = "Dealer Wins";
        }
        //player has 21 or less AND (dealer has 21 or more OR player has higher value than dealer)
        else if (Player.Value() <= 21 && (Dealer.Value() > 21 || Player.Value() > Dealer.Value()))
        {
            Results.text = "Player Wins";
        }
        //player and dealer have 21 or less AND player and dealer have equal value
        else if(Player.Value() <= 21 && Player.Value() <= 21 && Player.Value() == Dealer.Value())
        {
            Results.text = "Draw";
        }
        //player and dealer have over 21
        else
        {
            Results.text = "House Wins";
        }
    }

    // Use this for initialization
    void Start () {
        HitButton.interactable = false;
        StandButton.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
        PlayerScore.text = Player.Value().ToString();

        if(HideDealerScore)
        {
            // hide the rank of the dealers 2nd card (index 1)
            DealerScore.text = (Dealer.Value() - Dealer.GetCard(1).GetComponent<Card>().Rank()).ToString() + " + ?";
        }
        else
        {
            DealerScore.text = Dealer.Value().ToString();
        }
    }
}
