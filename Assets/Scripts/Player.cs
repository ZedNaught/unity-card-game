using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Hero hero;
    public CardHand hand;
    public Deck deck;

    public bool PlayersTurn() {
        return GameManager.Instance.CurrentPlayer == this;
    }

    private void Awake() {
        deck.player = this;
        hero.player = this;
        hand.player = this;
    }
}
