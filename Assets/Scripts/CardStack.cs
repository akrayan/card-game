using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardStack : MonoBehaviour
{
    List<Card> m_cards;

    public UnityAction<List<Card>> CardAdded;
    public UnityAction<List<Card>> CardRemoved;

    private void Awake()
    {
        m_cards = new List<Card>();
    }

    void Sort()
    {
        m_cards.Sort();
    }

    void Shuffle()
    {
        int rndIdx = 0;
        Card tmp;
        for (int i = 0; i < m_cards.Count; i++)
        {
            rndIdx = Random.Range(0, m_cards.Count);
            tmp = m_cards[i];
            m_cards[i] = m_cards[rndIdx];
            m_cards[rndIdx] = tmp;
        }
    }

    public void InitAndShuffle(List<Card> cards)
    {
        m_cards = cards;
        Shuffle();
        CardAdded?.Invoke(cards);
    }
    public void AddCard(Card card)
    {
        List<Card> cards = new List<Card>();

        m_cards.Add(card);
        cards.Add(card);
        CardAdded?.Invoke(cards);
    }

    public void AddCards(List<Card> cards)
    {
        foreach (Card c in cards)
            m_cards.Add(c);
        CardAdded?.Invoke(cards);
    }

    public void RemoveCard(Card card)
    {
        List<Card> cards = new List<Card>();
        cards.Add(card);
        m_cards.Remove(card);
        CardRemoved?.Invoke(cards);
    }

    public void RemoveCards(List<Card> cards)
    {
        foreach (Card c in cards)
            m_cards.Remove(c);
        CardRemoved?.Invoke(cards);

    }

    public void Clear()
    {
        CardRemoved?.Invoke(m_cards);
        m_cards.Clear();
    }

    public List<Card> GetCards()
    {
        return m_cards;
    }

}
