using UnityEngine;
using System.Collections;

public class MinionEffect : CardEffect {
    public Minion Minion {
        get {
            return _minion;
        }
    }
//    public Hero target;  // TODO // remove

    [SerializeField]
    private Minion _minion;

    public virtual bool Play(int minionIndex) {
        return false;
    }
}
