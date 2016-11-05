using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour, IHighlightable {
    public string cardName, cardText;
    public int manaCost;
    public Text manaText, descriptionText, nameText;
    public CardHand hand;
    public Canvas canvas;
    public Player owner, opponent;

    public static float mouseoverScaleFactor = 1.8f;
    [SerializeField] private Image highlightImage;
    private bool highlighted;

    public void Highlight(Color? color = null) {
        highlighted = true;
        highlightImage.color = color ?? Color.green;
    }

    public Card GetMouseoverCopy() {
        Card copy = ((GameObject)Instantiate(gameObject)).GetComponent<Card>();
        copy.name = "Moused-over Card";
        copy.GetComponent<Collider>().enabled = false;
//        copy.highlighted = highlighted; // TODO // fix highlight flicker
        return copy;
    }

    public bool OwnerCanPlay() {
        return (owner.hero.AvailableMana >= manaCost);
    }

    public void SetVisible(bool isVisible) {
        canvas.enabled = isVisible;
    }

    private void Awake() {
        canvas = GetComponentInChildren<Canvas>();
        owner = GameManager.Instance.player1;
        opponent = GameManager.Instance.player2;
    }

    private void Update() {
        if (owner.PlayersTurn() && OwnerCanPlay())
            Highlight(Color.green);
        UpdateCardText();
    }

    private void LateUpdate() {
        highlightImage.gameObject.SetActive(highlighted);
        highlighted = false;
    }

    protected void UpdateCardText() {
        manaText.text = manaCost.ToString();
        descriptionText.text = cardText;
        nameText.text = cardName;
    }
}