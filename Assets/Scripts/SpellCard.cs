using UnityEngine;
using System.Collections;

public class SpellCard : Card {
    public int damage;
//    public Hero target;  // TODO // remove

    public virtual void Play(IDamageable target) {
        target.Damage(damage);
    }

    public virtual bool Play() {
        if (!OwnerCanPlay()) {
            return false;
        }
        IDamageable target = opponent.hero;
        target.Damage(damage);
        owner.hero.TappedMana += manaCost;
        return true;
    }

    public virtual bool RequiresTarget() {
        return false;
    }

    public virtual bool OwnerCanPlay() {
        return (owner.hero.AvailableMana >= manaCost);
    }

    private void Start() {
        cardText = string.Format("Deal {0} damage to enemy Hero.", damage);
    }
}
