using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    public RectTransform player1Mana, player2Mana;
    public GameObject availableManaPrefab, tappedManaPrefab;
    public static UIManager Instance { get; set; }

    private Stack<RectTransform> player1Crystals; //, player2Crystals;
    private RectTransform player1ManaPanel; //, player2ManaPanel;
    private Text player1ManaCountText, player2ManaCountText;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        player1ManaPanel = (RectTransform)(player1Mana.Find("Crystal Panel").transform);
//        player2ManaPanel = (RectTransform)(player2Mana.Find("Crystal Panel").transform);
        player1Crystals = new Stack<RectTransform>();
//        player2Crystals = new Stack<RectTransform>();
        player1ManaCountText = player1Mana.GetComponentInChildren<Text>();
        player2ManaCountText = player2Mana.GetComponentInChildren<Text>();
    }

    private void Start() {
//        RefreshManaPanels();
    }

    public void RefreshManaPanels() {
        RectTransform[] panels = { player1ManaPanel }; //, player2ManaPanel };
        Player[] players = { GameManager.Instance.player1 }; // , GameManager.Instance.player2 };
        Stack<RectTransform>[] crystalStacks = { player1Crystals }; //, player2Crystals };

        for (int i = 0; i < panels.Length; i++) {
            RectTransform panel = panels[i];
            Player player = players[i];
            Stack<RectTransform> crystalStack = crystalStacks[i];

            // destroy old mana crystals
            while (crystalStack.Count > 0) {
                RectTransform child = crystalStack.Pop();
                child.SetParent(null);
                Destroy(child.gameObject);
            }

            // add available mana crystals
            for (int j = 0; j < player.hero.AvailableMana; j++) {
                GameObject manaCrystal = Instantiate(availableManaPrefab);
                manaCrystal.transform.SetParent(panel.transform, false);
                crystalStack.Push((RectTransform)manaCrystal.transform);
            }
            // add tapped mana crystals
            for (int j = 0; j < player.hero.TappedMana; j++) {
                GameObject manaCrystal = Instantiate(tappedManaPrefab);
                manaCrystal.transform.SetParent(panel.transform, false);
                crystalStack.Push((RectTransform)manaCrystal.transform);
            }
        }

        player1ManaCountText.text = string.Format("{0}/{1}", GameManager.Instance.player1.hero.AvailableMana, GameManager.Instance.player1.hero.Mana);
        player2ManaCountText.text = string.Format("{0}/{1}", GameManager.Instance.player2.hero.AvailableMana, GameManager.Instance.player2.hero.Mana);
    }
}
