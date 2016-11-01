using UnityEngine;
using System.Collections;

public class SpellCard : Card {
    public int damage;
    public Hero target;  // TODO // remove

    public void Play(ITargetable target) {
        target.Damage(damage);
    }

    public void Play() {
        target.Damage(damage);
    }

    private void Start() {
        cardText = string.Format("Deal {0} damage to enemy Hero.", damage);
    }
}
