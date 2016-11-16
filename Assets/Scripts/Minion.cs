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
        }
    }

    private int _health;
    private bool highlighted = false;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Text healthText;
    [SerializeField] private Text attackText;

    public void Damage(int amount) {
        Health -= amount;
    }

    public void Highlight(Color? color = null) {
        highlighted = true;
        highlightImage.color = color ?? Color.green;
    }

    private void Awake() {
        Health = maxHealth;
    }

    private void Update() {
        UpdateText();
    }

    private void LateUpdate() {
        highlightImage.gameObject.SetActive(highlighted);
        highlighted = false;
    }

    private void UpdateText() {
        healthText.text = Health.ToString();
        attackText.text = attack.ToString();
    }
}
