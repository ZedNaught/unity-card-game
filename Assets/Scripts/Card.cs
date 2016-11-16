using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardEffect))]
public class Card : MonoBehaviour, IHighlightable {
    public string CardName {
        get {
            return _cardName;
        }
        set {
            _cardName = value;
            gameObject.name = "(Card) " + _cardName;
        }
    }
    public string cardText;
    public int manaCost;
    public Text manaText, descriptionText, nameText;
    public CardHand hand;
    public Canvas canvas;
    public Player owner, opponent;
    public static float mouseoverScaleFactor = 1.8f;

    [SerializeField] private Image highlightImage;
    private bool highlighted;
    [SerializeField] private string _cardName;
    private CardEffect CardEffect {
        get {
            if (_cardEffect == null) {
                _cardEffect = GetComponent<CardEffect>();
            }
            return _cardEffect;
        }
    }
    private CardEffect _cardEffect;

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

    public bool Play(ITargetable target=null) {
        return CardEffect.Play(target);
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
        nameText.text = CardName;
    }
}