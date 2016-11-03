using UnityEngine;
using System.Collections;

public class SpellCard : Card {
    public int damage;
//    public Hero target;  // TODO // remove

    public virtual bool Play(IDamageable target) {
        if (!OwnerCanPlay()) {
            return false;
        }
        target.Damage(damage);
        owner.hero.TappedMana += manaCost;
        return true;
    }

    public virtual bool Play() {
        return false;
//        if (!OwnerCanPlay()) {
//            return false;
//        }
//        IDamageable target = opponent.hero;
//        target.Damage(damage);
//        owner.hero.TappedMana += manaCost;
//        return true;
    }

    public virtual bool RequiresTarget() {
        return true;
    }

    public virtual bool CanUseTarget() {
        return true;
    }

    public virtual bool OwnerCanPlay() {
        return (owner.hero.AvailableMana >= manaCost);
    }

    private void Start() {
        cardText = string.Format("Deal {0} damage to enemy Hero.", damage);
    }
}
