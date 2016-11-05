using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Hero hero;
    public CardHand hand;

    public bool PlayersTurn() {
        return GameManager.Instance.CurrentPlayer == this;
    }
}
