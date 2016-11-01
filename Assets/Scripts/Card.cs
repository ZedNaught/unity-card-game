using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour {
    public string cardName, cardText;
    public int manaCost;
    public Text manaText, descriptionText, nameText;

    private void Update() {
        UpdateCardText();
    }

    protected void UpdateCardText() {
        manaText.text = manaCost.ToString();
        descriptionText.text = cardText;
        nameText.text = cardName;
    }
}