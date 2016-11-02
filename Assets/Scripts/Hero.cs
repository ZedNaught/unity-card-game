using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hero : MonoBehaviour, IDamageable {
    public int maxHealth, health, maxMana, mana;

    public Text healthText;

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

    private void StartTurn() {
        IncreaseMana();
    }

    private void IncreaseMana() {
        mana = Mathf.Min(mana + 1, maxMana);
    }
}
