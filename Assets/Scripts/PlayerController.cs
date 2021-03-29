using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardStack))]
public class PlayerController : MonoBehaviour, ICharacterController
{
    [SerializeField] GameObject ColorSelection;
    [SerializeField] GameObject PickButton;
    public CardStack Stack { get; private set; }
    CardStackView m_stackView;


    List<Card> m_selectableCard;
    int m_selectedCardId = -1;
    bool m_isMyTurn = false;

    private void Awake()
    {
        //I do it in Awake to make sure that the stack is accessible from the controller at Start
        Stack = GetComponent<CardStack>();
        m_stackView = GetComponent<CardStackView>();
    }

    void Start()
    {
        //m_selectableCard = new List<Card>();
        ColorSelection.SetActive(false);
        PickButton.SetActive(false);
    }

    void EndTurn()
    {
        ColorSelection.SetActive(false);
        PickButton.SetActive(false);
        m_stackView.HideSelectable();
        m_isMyTurn = false;
        GameController.Instance.EndTurn();
    }

    void PlaceACard(CardColor changeColor = 0)
    {
        if (m_selectableCard[m_selectedCardId].Value == 8)
            GameController.Instance.PlaceACard(Stack, m_selectableCard[m_selectedCardId]);
        else
            GameController.Instance.PlaceACard(Stack, m_selectableCard[m_selectedCardId], changeColor);
        EndTurn();
    }

    void HandleCardSelection()
    {
        if (m_selectableCard[m_selectedCardId].Value == 8)
            ColorSelection.SetActive(true);
        else
            PlaceACard();
    }

    void HandleCardClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 20, LayerMask.GetMask("Card"));
        if (!hit) return;

        CardModel cardModel = hit.transform.GetComponent<CardModel>();
        if ((m_selectedCardId = m_selectableCard.IndexOf(cardModel.Card)) == -1)
            return;
        HandleCardSelection();
    }


    void Update()
    {
        if (m_isMyTurn && Input.GetMouseButtonDown(0))
        {
            HandleCardClick();
        }

    }

    public void PlayTurn()
    {
        m_isMyTurn = true;
        m_selectableCard = GameController.Instance.CheckPlacableCard(Stack.GetCards());
        m_stackView.ShowSelectable(m_selectableCard);
        PickButton.SetActive(true);
    }

    /// <summary>
    /// Will be call by the UI and allow to choose a new
    /// </summary>
    /// <param name="i"> must be between 0 and 4</param>
    public void ChooseColor(int i)
    {
        i = i < 0 ? 0 : i > 3 ? 3 : i;

        CardColor changeColor = (CardColor)i;
        PlaceACard(changeColor);
    }

    public void PickACard()
    {
        GameController.Instance.PickACard(Stack);
        EndTurn();
    }
}
