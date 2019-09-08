using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardStack))]
public class CardStackOps : MonoBehaviour
{
    //private Card _Card;
    
    public CardStack CardStack;
    public Button CheckShuffleButton;

    private void Awake()
    {
        //_Card = CardGO.GetComponent<Card>();
    }
}
