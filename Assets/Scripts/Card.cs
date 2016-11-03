using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour {
    public string cardName, cardText;
    public int manaCost;
    public Text manaText, descriptionText, nameText;
    public CardHand hand;
    public Canvas canvas;
    public Player owner, opponent;

    public static float mouseoverScaleFactor = 1.8f;

    private void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        owner = GameManager.Instance.player1;
        opponent = GameManager.Instance.player2;
    }

    private void Update() {
        UpdateCardText();
    }

    protected void UpdateCardText() {
        manaText.text = manaCost.ToString();
        descriptionText.text = cardText;
        nameText.text = cardName;
    }

    public Card GetMouseoverCopy() {
        Card copy = ((GameObject)Instantiate(gameObject)).GetComponent<Card>();
        copy.name = "Moused-over Card";
        copy.GetComponent<Collider>().enabled = false;
        return copy;
    }
}