using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero : MonoBehaviour, IDamageable {
    public string heroName;
    public int maxHealth, health, maxMana;
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

    private void Start() {
        health = maxHealth;
    }

    private void Update() {
        UpdateHeroText();
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
            Mana += 3;
        }
        TappedMana = 1;
    }

    private void DrawCard() {
        // TODO //
    }
}
