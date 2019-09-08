using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOps : MonoBehaviour
{
    public int FaceIndex;
    public Card Card;
    public Button FlipThroughButton;
    public Text RankText;

    public void FlipThrough()
    {
        Card.FaceIndex = FaceIndex;

        if (FaceIndex == Card.Faces.Count)
        {
            Card.ToggleFace(false);
            RankText.text = "";
            FaceIndex = 0;
        }
        else
        {
            Card.ToggleFace(true);
            RankText.text = "Rank: " + Card.Rank();
            FaceIndex++;
        }
    }
}
