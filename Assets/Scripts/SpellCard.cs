using UnityEngine;
using System.Collections;

public class SpellCard : Card {
    public int damage;
//    public Hero target;  // TODO // remove

    public virtual void Play(IDamageable target) {
        target.Damage(damage);
    }

    public virtual void Play() {
        IDamageable target = opponent.hero;
        target.Damage(damage);
    }

    public virtual bool RequiresTarget() {
        return false;
    }

    private void Start() {
        cardText = string.Format("Deal {0} damage to enemy Hero.", damage);
    }
}
