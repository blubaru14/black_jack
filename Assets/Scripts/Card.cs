using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
// Card contains all card faces and back. A card object can
// be any card in the deck.
public class Card : MonoBehaviour
{
    private SpriteRenderer _CardSR;

    public List<Sprite> Faces;
    public Sprite Back;
    public int FaceIndex;

    // Awake is used to initialize any variables or game 
    // state before the game starts.
    private void Awake()
    {
        _CardSR = this.GetComponent<SpriteRenderer>();
    }

    // Rank will return the rank of the card based on the
    // face that is currently set. Number cards return their
    // value. Face cards return 10. Aces return 11.
    public int Rank()
    {
        // 4 cards per rank
        // +2 for starting rank at 2
        int rank = (FaceIndex / 4) + 2;
        
        // if it is a face card (minus aces) then value is 10
        if(rank > 11)
        {
            return 10;
        }
        // pass back rank for all others
        else
        {
            return rank;
        }
    }

    // ToogleFace will set the sprite for the card. Based on
    // boolean value it will show the face or the back.
    public void ToggleFace(bool showFace)
    {
        // show a face
        if (showFace)
        {
            _CardSR.sprite = Faces[FaceIndex];
        }
        // show the back
        else
        {
            _CardSR.sprite = Back;
        }
    }
}
