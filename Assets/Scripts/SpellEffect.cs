using UnityEngine;
using System.Collections;

public class SpellEffect : CardEffect {
    public int damage;
//    public Hero target;  // TODO // remove

    public virtual bool Play(ITargetable target=null) {
        if (!Card.OwnerCanPlay()) {
            return false;
        }
        target.Damage(damage);
        Card.owner.hero.TappedMana += Card.manaCost;
        return true;
    }

//    public virtual bool Play() {
//        return false;
////        if (!OwnerCanPlay()) {
////            return false;
////        }
////        IDamageable target = opponent.hero;
////        target.Damage(damage);
////        owner.hero.TappedMana += manaCost;
////        return true;
//    }

    public virtual bool RequiresTarget() {
        return true;
    }

    public virtual bool CanUseTarget() {
        return true;
    }

    private void Start() {
        Card.cardText = string.Format("Deal {0} damage.", damage);
    }
}
