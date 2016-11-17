using UnityEngine;
using System.Collections.Generic;

public class CardHand : MonoBehaviour {
    public GameObject cardPrefab;
    public float cardSpacing, cardTilt, cardHeightAdj, handArcAmplitude, handArcFrequency;
    public int handSize;
    public static CardHand Instance { get; set; }
    public Player player;
//    public static int maxHandSize = 10;

    [SerializeField] private List<Card> cards;
    private Card mouseoverCard;
    private Card mouseoverCardCopy;
    [SerializeField] private LineRenderer lineRenderer;


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
                MouseoverCard(null);
                card.hand = null;
                return card;
            }
        }
        index = -1;
        return null;
    }

    public void MouseoverCard(Card card) {
        if (card == mouseoverCard) {
            return;
        }
        if (mouseoverCard) {
            mouseoverCard.canvas.enabled = true;
            Destroy(mouseoverCardCopy.gameObject);
        }
        mouseoverCard = card;
        if (!card) {
            return;
        }
        mouseoverCardCopy = card.GetMouseoverCopy();
        mouseoverCardCopy.transform.SetParent(transform);
        card.canvas.enabled = false;
        mouseoverCardCopy.transform.rotation = Quaternion.identity;
        mouseoverCardCopy.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, transform.position.z + 4f);
        mouseoverCardCopy.transform.localScale = Card.mouseoverScaleFactor * Vector3.one;
        mouseoverCardCopy.canvas.sortingOrder = cards.Count;
    }

    public void SetLineTarget(Vector3 position) {
        lineRenderer.SetPosition(1, position + Vector3.up);
    }

    public void SetLineVisible(bool visible) {
        lineRenderer.enabled = visible;
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        lineRenderer.SetPosition(0, transform.position + Vector3.up);
    }

    private void Start() {
        temp_BootstrapCardHand();
//        UpdateCardPositions();
    }

    private void temp_BootstrapCardHand() {
        for (int i = 0; i < handSize; i++) {
            Card card = ((GameObject)Instantiate(cardPrefab, transform)).GetComponent<Card>();
            card.manaCost = i;
            SpellEffect effect = card.GetComponent<SpellEffect>();
            effect.damage = i + 1;
            card.CardName = (i + 1) + " Damage";
            AddCard(card);
        }
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
