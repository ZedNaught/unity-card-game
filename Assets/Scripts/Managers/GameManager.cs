using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; set; }
    public Player player1, player2;
    public Player CurrentPlayer { get; private set; }

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
        if (CurrentPlayer == null || CurrentPlayer == player2 ) {
            CurrentPlayer = player1;
        }
        else {
            CurrentPlayer = player2;
        }

        CurrentPlayer.hero.StartTurn();
    }
}
