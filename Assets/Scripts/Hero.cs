using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero : MonoBehaviour, ITargetable {
    public string heroName;
    public int maxHealth, health, maxMana;

    private bool highlighted = false;
    [SerializeField] private Image highlightImage;

    public int Mana {
        get {
            return _mana;
        }
        set {
            _mana = value;
            UIManager.Instance.RefreshManaPanels();
        }
    }
    public int TappedMana {
        get {
            return _tappedMana;
        }
        set {
            _tappedMana = value;
            UIManager.Instance.RefreshManaPanels();
        }
    }
    public int AvailableMana {
        get {
            return Mana - TappedMana;
        }
    }
    public Text healthText;

    private int _mana = 0, _tappedMana = 0;


    public void Damage(int amount) {
        health -= amount;
    }

    public void Highlight(Color? color = null) {
        highlighted = true;
        highlightImage.color = color ?? Color.green;
    }

    private void Start() {
        health = maxHealth;
    }

    private void Update() {
        UpdateHeroText();
    }

    private void LateUpdate() {
        highlightImage.gameObject.SetActive(highlighted);
        highlighted = false;
    }

    private void UpdateHeroText() {
        healthText.text = health.ToString();
    }

    public void StartTurn() {
        IncreaseAndRefreshMana();
        DrawCard();
    }

    private void IncreaseAndRefreshMana() {
        if (Mana < maxMana) {
            Mana += 1;
        }
        TappedMana = 0;
    }

    private void DrawCard() {
        // TODO // add card drawing
        Debug.Log("TODO: add card drawing");
    }
}
