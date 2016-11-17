using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Card))]
public abstract class CardEffect : MonoBehaviour {
    protected Card Card {
        get {
            if (_card == null) {
                _card = GetComponent<Card>();
            }
            return _card;
        }
    }
    protected Card _card;
}
