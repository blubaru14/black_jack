using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// CardStack is a collection of Card objects and operations to
// support interactions with it.
public class CardStack : MonoBehaviour
{
    private static System.Random _Rand = new System.Random();
    private List<GameObject> _Cards;
    
    public GameObject CardPrefab;
    public float cardSpacing;

    // Awake is used to initialize any variables or game 
    // state before the game starts.
    private void Awake()
    {
        _Cards = new List<GameObject>();
    }

    // Shuffle will create a list of ints that is shuffled based on
    // Fisher–Yates algorithm.
    private static List<int> Shuffle(int count)
    {
        // create a list of ints starting at 0 and iterating count amount 
        // of times
        List<int> shuffledPositions = Enumerable.Range(0, count).ToList();

        // execute Fisher–Yates algorithm
        for (int position = shuffledPositions.Count - 1; position > 0; position--)
        {
            int randPosition = _Rand.Next(position + 1);
            var temp = shuffledPositions[position];
            shuffledPositions[position] = shuffledPositions[randPosition];
            shuffledPositions[randPosition] = temp;
        }

        return shuffledPositions;
    }

    // GetCardCount will return the card count. This is needed since
    // Cards is a private object (we don't want anyone messing with
    // that).
    public int GetCardCount()
    {
        // check the cards list is not null
        if (_Cards != null)
        {
            return _Cards.Count;
        }
        // empty cards list or bad index
        else
        {
            return 0;
        }
    }

    // GetCard will get a card and return it. This is needed since
    // Cards is a private object (we don't want anyone messing with
    // that).
    public GameObject GetCard(int index)
    {
        // check the cards list is not null and that the index exists
        if (_Cards != null && _Cards.Count > index)
        {
            return _Cards[index];
        }
        // empty cards list or bad index
        else
        {
            Debug.Log("No Card in the Card Stack at index: " + index);
            return null;
        }
    }

    // AddCard is basically a stack push function. It pushes to the
    // tail of the card stack. It will set itself as the parent of
    // the card.
    public void AddCard(GameObject cardGO, bool showFace)
    {
        // setting up the x position with the given card spacing and 
        // card count
        float xPosition = (_Cards.Count + 1f) * cardSpacing;

        // move the card to a new parent card stack
        cardGO.transform.SetParent(this.transform);
        cardGO.transform.localPosition = new Vector3(xPosition, 0f, 0f);

        Card card = cardGO.GetComponent<Card>();
        // set the card face up or down based on the given boolean
        card.ToggleFace(showFace);

        SpriteRenderer cardSR = cardGO.GetComponent<SpriteRenderer>();
        // set the sorting order so cards aren't displayed out of order
        // + 1 to put it on top of the background
        cardSR.sortingOrder = _Cards.Count + 1;

        _Cards.Add(cardGO);
    }

    // RemoveCard is basically a stack pop function. It pops the tail
    // of the card stack and then returns it. This does not destroy 
    // the card object as we will move the card to another card stack 
    // and that card stack will become the parent of the object.
    public GameObject RemoveCard()
    {
        // make sure there are cards 
        if (_Cards.Count > 0)
        {
            GameObject card = _Cards[_Cards.Count - 1];
            _Cards.RemoveAt(_Cards.Count - 1);

            return card;
        }
        // no cards
        else
        {
            Debug.Log("No Cards left in Card Stack to Remove");
            return null;
        }
    }

    // Clear is for cleaning up a card stack. It will loop through 
    // all of its cards and destroy them.
    public void Clear()
    {
        // loop through all of cards
        foreach (GameObject cardGO in _Cards)
        {
            Destroy(cardGO);
        }

        _Cards.Clear();
    }

    // Value gets the rank of all the cards in the card stack.
    // Aces is a special case where it can have rank 1 or 11.
    // So if using 11 knocks the value over 21 then use 1 for 
    // its rank.
    public int Value()
    {
        int value = 0;

        // loop through all the cards
        foreach (GameObject cardGO in _Cards)
        {
            int rank = cardGO.GetComponent<Card>().Rank();

            // if aces and using 11 as rank brings value over 21
            if (rank == 11 && (value + rank) > 21)
            {
                value += 1;
            }
            // else use given rank
            else
            {
                value += rank;
            }
        }

        return value;
    }

    // MakeDeck will make the deck for the game. It shuffles the deck
    // based on the Shuffle function.
    public void MakeDeck()
    {
        int numFaces = CardPrefab.GetComponent<Card>().Faces.Count;
        // get the shuffled positions for the deck
        List<int> shuffledPositions = Shuffle(numFaces);

        // loop through all the faces
        for (int cardIndex = 0; cardIndex < numFaces; cardIndex++)
        {
            // instantiate the card
            GameObject cardGO = (GameObject)Instantiate(CardPrefab);
            // setting up the x position with the given card spacing
            float xPosition = (cardIndex + 1f) * cardSpacing;

            cardGO.transform.SetParent(this.transform, false);
            cardGO.transform.localPosition = new Vector3(xPosition, 0f, 0f);

            Card card = cardGO.GetComponent<Card>();
            // set the face to the shuffled index
            card.FaceIndex = shuffledPositions[cardIndex];
            // set the card face down since it is the deck
            card.ToggleFace(false);

            SpriteRenderer cardSR = cardGO.GetComponent<SpriteRenderer>();
            // set the sorting order so cards aren't displayed out of order
            // + 1 to put it on top of the background
            cardSR.sortingOrder = cardIndex + 1;

            _Cards.Add(cardGO);
        }
    }
}
