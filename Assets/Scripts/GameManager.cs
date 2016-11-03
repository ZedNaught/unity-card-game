using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; set; }
    public Player player1, player2, currentPlayer;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        StartTurn();
    }

    private void StartTurn() {
        if (currentPlayer == null || currentPlayer == player2 ) {
            currentPlayer = player1;
        }
        else {
            currentPlayer = player2;
        }

        currentPlayer.hero.StartTurn();
    }
}
