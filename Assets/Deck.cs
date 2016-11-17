using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
    public Player player;

    [SerializeField]
    private List<GameObject> cardList;

    public Card DrawCard() {
        int drawIndex = cardList.Count - 1;
        Card card = cardList[drawIndex].gameObject.GetComponent<Card>();
        cardList.RemoveAt(drawIndex);
        return card;
    }
}