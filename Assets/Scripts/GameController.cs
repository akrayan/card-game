using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    static GameController m_instance = null;
    public static GameController Instance { get { return m_instance; } }

    public float TimeBetweenTurns = 0.5f;
    public GameObject VictoryScreen;
    // public TextMe

    public TMP_Text history;
    public TMP_Text victoryText;

    public CardStack Deck;
    public CardStack PlayedCard;

    //public CardStack[] PlayersStack;
    List<ICharacterController> Players;

    Card m_lastCardplayed;
    int m_playerTurn = 0;

    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        Players = FindInterfaces.Find<ICharacterController>();
        StartGame();
    }

    // Update is called once per frame
    void InitDeck()
    {
        List<Card> deck = new List<Card>();

        foreach (CardColor c in Enum.GetValues(typeof(CardColor)))
        {
            for (ushort i = 1; i <= 13; i++)
                deck.Add(new Card(c, i));
        }
        Deck.InitAndShuffle(deck);
    }

    void DistributeCard()
    {
        List<Card> deck = Deck.GetCards();
        foreach (ICharacterController player in Players)
        {
            for (int i = 0; i < 8; i++)
            {
                player.Stack.AddCard(deck[deck.Count - 1]);
                Deck.RemoveCard(deck[deck.Count - 1]);
            }
        }
    }

    bool CheckVictory()
    {
        if (Players[m_playerTurn].Stack.GetCards().Count != 0)
            return false;
        VictoryScreen.SetActive(true);
        victoryText.text = "Player " + (m_playerTurn + 1) + " win !";
        return true;
    }

    void PlayNextTurn()
    {
        Players[m_playerTurn].PlayTurn();
    }

    void StartGame()
    {
        VictoryScreen.SetActive(false);
        InitDeck();
        DistributeCard();

        List<Card> deck = Deck.GetCards();

        m_playerTurn = 0;
        PlayedCard.AddCard(deck[deck.Count - 1]);
        m_lastCardplayed = deck[deck.Count - 1];
        Deck.RemoveCard(deck[deck.Count - 1]);
        history.text = "";
        PlayNextTurn();
    }

    bool IsPlacable(Card card)
    {
        return card.Color == m_lastCardplayed.Color || card.Value == m_lastCardplayed.Value || card.Value == 8;
    }

    public void PlaceACard(CardStack stack, Card card, CardColor changeColor = 0)
    {
        if (IsPlacable(card))
        {
            m_lastCardplayed = card;
            PlayedCard.AddCard(card);
            stack.RemoveCard(card);
            if (card.Value == 8)
                m_lastCardplayed.Color = changeColor;
        }
        history.text = "Player " + (m_playerTurn + 1) + " put a " + (card.Value) + " of " + card.Color.ToString();
    }

    public void PickACard(CardStack stack)
    {
        List<Card> deck = Deck.GetCards();

        stack.AddCard(deck[deck.Count - 1]);
        Deck.RemoveCard(deck[deck.Count - 1]);
        history.text = "Player " + (m_playerTurn + 1) + " picked a card";
    }

    public List<Card> CheckPlacableCard(List<Card> cards)
    {
        return cards.FindAll(IsPlacable);
    }

    void RefillDeck()
    {
        List<Card> tmp = PlayedCard.GetCards();
        //tmp.Remove(m_lastCardplayed);
        Deck.InitAndShuffle(tmp);
        PlayedCard.Clear();
        Deck.RemoveCard(m_lastCardplayed);
        PlayedCard.AddCard(m_lastCardplayed);
    }
    public void EndTurn()
    {
        if (CheckVictory())
            return;
        if (Deck.GetCards().Count == 0)
            RefillDeck();
        m_playerTurn++;
        if (m_playerTurn >= Players.Count)
            m_playerTurn = 0;
        Invoke("PlayNextTurn", TimeBetweenTurns);
    }

    public void RestartGame()
    {
        foreach (ICharacterController player in Players)
            player.Stack.Clear();
        Deck.Clear();
        PlayedCard.Clear();
        StartGame();
    }
}
