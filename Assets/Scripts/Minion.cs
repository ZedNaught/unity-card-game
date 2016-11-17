using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour, ITargetable {
    public string minionName;
    public int maxHealth, attack;
    public int Health {
        get {
            return _health;
        }
        private set {
            _health = value;
            if (_health <= 0) {
                Die();
            }
        }
    }
    public float Width {
        get {
            return canvas.pixelRect.width;
        }
    }
    public float Height {
        get {
            return canvas.pixelRect.height;
        }
    }
    public MinionBoard minionBoard;

    private int _health;
    private bool highlighted = false;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Text healthText;
    [SerializeField] private Text attackText;
    private Canvas canvas;

    public void Damage(int amount) {
        Health -= amount;
    }

    public void Highlight(Color? color = null) {
        highlighted = true;
        highlightImage.color = color ?? Color.green;
    }

    private void Awake() {
        Health = maxHealth;
        canvas = GetComponentInChildren<Canvas>();
    }

    private void Update() {
        UpdateText();
    }

    private void LateUpdate() {
        highlightImage.gameObject.SetActive(highlighted);
        highlighted = false;
    }

    private void OnDestroy() {
        if (minionBoard != null) {
            minionBoard.RemoveMinion(this);
            minionBoard = null;
        }
    }

    private void UpdateText() {
        healthText.text = Health.ToString();
        attackText.text = attack.ToString();
    }

    private void Die() {
        Destroy(gameObject);
    }
}
