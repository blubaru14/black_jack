using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardStack : MonoBehaviour {
    private List<GameObject> Cards;
    public GameObject CardPrefab;
    public float cardSpacing;

    private static System.Random Rand = new System.Random();

    private void Awake()
    {
        Cards = new List<GameObject>();
    }

    public int Value()
    {
        int value = 0;

        foreach (GameObject cardGO in Cards)
        {
            int rank = cardGO.GetComponent<Card>().Rank();

            //if aces and using 11 as rank brings value over 21
            if (rank == 11 && (value + rank) > 21)
            {
                value += 1;
            }
            else
            {
                value += rank;
            }
        }

        return value;
    }

    public void MakeDeck()
    {
        int numFaces = CardPrefab.GetComponent<Card>().Faces.Count;

        List<int> shuffledPositions = Shuffle(numFaces);

        for (int i = 0; i < numFaces; i++)
        {
            GameObject cardGO = (GameObject)Instantiate(CardPrefab);
            float co = (i + 1f) * cardSpacing;

            cardGO.transform.SetParent(this.transform, false);
            cardGO.transform.localPosition = new Vector3(co, 0f, 0f);

            Card card = cardGO.GetComponent<Card>();
            card.FaceIndex = shuffledPositions[i];
            card.ToggleFace(false);

            SpriteRenderer cardSR = cardGO.GetComponent<SpriteRenderer>();
            cardSR.sortingOrder = i;
      
            Cards.Add(cardGO);
        }
    }

    public static List<int> Shuffle(int count)
    {
        List<int> shuffledPositions = Enumerable.Range(0, count).ToList();

        for (int position = shuffledPositions.Count - 1; position > 0; position--)
        {
            int randPosition = Rand.Next(position + 1);
            var temp = shuffledPositions[position];
            shuffledPositions[position] = shuffledPositions[randPosition];
            shuffledPositions[randPosition] = temp;
        }

        return shuffledPositions;
    }

    public GameObject GetCard(int index)
    {
        if(Cards[index] == null)
        {
            return null;
        }
        else
        {
            return Cards[index];
        }
    }

    public void AddCard(GameObject cardGO, bool showFace)
    {
        float co = (Cards.Count + 1f) * cardSpacing;

        cardGO.transform.SetParent(this.transform);
        cardGO.transform.localPosition = new Vector3(co, 0f, 0f);

        Card card = cardGO.GetComponent<Card>();
        card.ToggleFace(showFace);

        SpriteRenderer cardSR = cardGO.GetComponent<SpriteRenderer>();
        cardSR.sortingOrder = Cards.Count;

        Cards.Add(cardGO);
    }

    public GameObject RemoveCard()
    {
        GameObject card = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);

        return card;
    }

    public void Clear()
    {
        foreach (GameObject cardGO in Cards)
        {
            Destroy(cardGO);
        }

        Cards.Clear();
    }
}
