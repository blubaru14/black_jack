using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    private SpriteRenderer _CardSR;

    public List<Sprite> Faces;
    public Sprite Back;
    public int FaceIndex;
    public bool ShowFace;

    private void Awake()
    {
        _CardSR = this.GetComponent<SpriteRenderer>();
    }

    public int Rank()
    {
        //4 cards per rank
        //+2 for starting rank at 2
        int rank = (FaceIndex / 4) + 2;
        
        //if it is a face card (minus aces) then value is 10
        if(rank > 11)
        {
            return 10;
        }
        //pass back rank for all others
        else
        {
            return rank;
        }
    }

    public void ToggleFace(bool showFace)
    {
        if(showFace)
        {
            _CardSR.sprite = Faces[FaceIndex];
        }
        else
        {
            _CardSR.sprite = Back;
        }

        ShowFace = showFace;
    }
}
