using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour {
    public string cardName, cardText;
    public int manaCost;
    public Text manaText, descriptionText, nameText;
    public CardHand hand;
    public Canvas canvas;

    public static float highlightScaleFactor = 1.8f;

    private void Awake() {
        canvas = GetComponentInChildren<Canvas>();
    }

    private void Update() {
        UpdateCardText();
    }

    protected void UpdateCardText() {
        manaText.text = manaCost.ToString();
        descriptionText.text = cardText;
        nameText.text = cardName;
    }

    public Card GetHighlightCopy() {
        Card copy = ((GameObject)Instantiate(gameObject)).GetComponent<Card>();
        copy.GetComponent<Collider>().enabled = false;
        return copy;
    }
}