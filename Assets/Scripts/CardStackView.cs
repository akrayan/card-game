using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardStack))]
public class CardStackView : MonoBehaviour
{
    [SerializeField] float offset = 0.2f;
    [SerializeField] float selectableOffset = 0.4f;
    [SerializeField] bool hideCards = false;
    [SerializeField] bool center = true;
    [SerializeField] GameObject CardPrefab;

    Dictionary<Card, GameObject> CardModels;
    CardStack stack;

    private void Awake()
    {
        CardModels = new Dictionary<Card, GameObject>();

        stack = GetComponent<CardStack>();
        stack.CardAdded += onCardAdded;
        stack.CardRemoved += onCardRemoved;
    }

    void onCardAdded(List<Card> cards)
    {
        GameObject gb;
        CardModel cm;

        foreach(Card c in cards)
        {
            gb = Instantiate(CardPrefab, transform);
            //gb.transform.localPosition += new Vector3((CardModels.Count * offset) - parentOffset, 0, 0);
            cm = gb.GetComponent<CardModel>();
            cm.Card = c;
            cm.ToggleFace(hideCards);
            CardModels.Add(c, gb);
        }
        RepositionCards();

    }

    void onCardRemoved(List<Card> cards)
    {
        foreach (Card c in cards)
        {
            Destroy(CardModels[c]);
            CardModels.Remove(c);
        }
        RepositionCards();
    }

    void RepositionCards()
    {
        float posX = center ? -((CardModels.Count -1) * offset) / 2 : 0;
        float posZ = 0;

        foreach (GameObject gb in CardModels.Values)
        {
            gb.transform.localPosition = new Vector3(posX, 0, posZ);
            posX += offset;
            posZ -= 0.001f;
        }
    }

    public void ShowSelectable(List<Card> selectable)
    {
        foreach (Card card in selectable)
        {
            CardModels[card].transform.localPosition += Vector3.up * selectableOffset;
            //CardModels[card].transform.localPosition += Vector3.back;
        }
    }

    public void HideSelectable()
    {
        foreach (GameObject gb in CardModels.Values)
            gb.transform.localPosition.Scale(new Vector3(1, 0, 1));
    }
}
