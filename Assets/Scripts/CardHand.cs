using UnityEngine;
using System.Collections.Generic;

public class CardHand : MonoBehaviour {
    public GameObject cardPrefab;
    public float cardSpacing, cardTilt, cardHeightAdj, handArcAmplitude, handArcFrequency;
    public int handSize;
    public static CardHand Instance { get; set; }
//    public static int maxHandSize = 10;

    [SerializeField] private List<Card> cards;
    private Card highlightedCard;
    private Card highlightedCardCopy;


    public void AddCard(Card card) {
        cards.Add(card);
        card.hand = this;
    }

    public void AddCard(Card card, int index) {
        cards.Insert(index, card);
        card.hand = this;
    }

    public Card PopCard(Card card, out int index) {
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i] == card) {
                index = i;
                cards.RemoveAt(i);
                card.canvas.sortingOrder = cards.Count;
                HighlightCard(null);
                card.hand = null;
                return card;
            }
        }
        index = -1;
        return null;
    }

    public void HighlightCard(Card card) {
        if (card == highlightedCard) {
            return;
        }
        if (highlightedCard) {
            highlightedCard.canvas.enabled = true;
            Destroy(highlightedCardCopy.gameObject);
        }
        highlightedCard = card;
        if (!card) {
            return;
        }
        highlightedCardCopy = card.GetHighlightCopy();
        highlightedCardCopy.transform.SetParent(transform);
        card.canvas.enabled = false;
        highlightedCardCopy.transform.rotation = Quaternion.identity;
        highlightedCardCopy.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, transform.position.z + 3f);
        highlightedCardCopy.transform.localScale = Card.highlightScaleFactor * Vector3.one;
        highlightedCardCopy.canvas.sortingOrder = cards.Count;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        for (int i = 0; i < handSize; i++) {
            Card card = ((GameObject)Instantiate(cardPrefab, transform)).GetComponent<Card>();
            card.manaCost = i;
            card.cardName = "Card " + i;
            ((SpellCard) card).damage = i + 1;
            AddCard(card);
        }
//        UpdateCardPositions();
    }

    private void Update() {
        UpdateCardPositions();
    }

    private void UpdateCardPositions() {
        int numCards = cards.Count;
        float totalDistance = (numCards - 1) * cardSpacing;
        for (int i = 0; i < numCards; i++) {
            // dirty eyeballed math to set positions of cards in hand 
            float thisCardTilt = ((-(float) (numCards - 1) / 2) + i) * cardTilt;
            cards[i].transform.rotation = Quaternion.Euler(thisCardTilt * Vector3.up);
            cards[i].transform.position = transform.position +
                                          (-totalDistance / 2f + i * cardSpacing) * Vector3.right +
                                          (-handArcAmplitude / 2f + handArcAmplitude * Mathf.Cos(handArcFrequency * thisCardTilt * Mathf.Deg2Rad)) * cardHeightAdj * Vector3.forward;
            cards[i].transform.localScale = Vector3.one;
            cards[i].canvas.sortingOrder = i;
        }
    }
}
