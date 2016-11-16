using UnityEngine;
using System.Collections;

public abstract class CardEffect : MonoBehaviour {
    public abstract bool Play(ITargetable target=null);
}
