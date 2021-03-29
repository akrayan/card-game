using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, ICharacterController
{
    public CardStack Stack { get; private set; }

    private void Awake()
    {
        //I do it in Awake to make sure that the stack is accessible from the controller at Start
        Stack = GetComponent<CardStack>();
    }

    CardColor FindBestColor()
    {
        int[] counts = new int[] { 0, 0, 0, 0 };
        int best = 0;

        foreach (Card card in Stack.GetCards())
            if (card.Value != 8) counts[(int)card.Color]++;
        for (int i = 1; i < 3; i++)
            best = counts[i] > counts[best] ? i : best;
        return (CardColor)best;
    }

    public void PlayTurn()
    {
        List<Card> placables = GameController.Instance.CheckPlacableCard(Stack.GetCards());

        if (placables.Count == 0)
            GameController.Instance.PickACard(Stack);
        else
        {
            if (placables[0].Value == 8)
                GameController.Instance.PlaceACard(Stack, placables[0], FindBestColor());
            else
                GameController.Instance.PlaceACard(Stack, placables[0]);
        }
        GameController.Instance.EndTurn();

    }
}
