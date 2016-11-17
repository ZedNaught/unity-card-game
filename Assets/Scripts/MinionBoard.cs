using UnityEngine;
using System.Collections.Generic;

public class MinionBoard : MonoBehaviour {
    public GameObject temp_MinionPrefab;

    [SerializeField]
    private int maxMinions;
    [SerializeField]
    private float minionWidth;
    private List<Minion> minions;

    public bool CanAddMinion() {
        return minions.Count < maxMinions;
    }

    public bool AddMinion(Minion minion, int index=-1) {
        if (!CanAddMinion() || index < -1 || index > minions.Count) {
            return false;
        }
        index = (index == -1) ? minions.Count : index;
        minions.Insert(index, minion);
        minion.minionBoard = this;
        return true;
    }

    public bool RemoveMinion(Minion minion) {
        return minions.Remove(minion);
    }

    private void Awake() {
        minions = new List<Minion>();
    }

    private void Start() {
        temp_PopulateBoard();
    }

    private void Update() {
        UpdateMinionPositions();
    }

    private void UpdateMinionPositions() {
        float totalWidth = minions.Count * minionWidth;
        for (int i = 0; i < minions.Count; i++) {
            Minion minion = minions[i];
            float xOffset = (-totalWidth + minionWidth) / 2f + minionWidth * i;
            minion.transform.position = transform.position + xOffset * Vector3.right;
        }
    }

    private void temp_PopulateBoard() {
        for (int i = 0; i < 5; i++) {
            GameObject minionObject = (GameObject)Instantiate(temp_MinionPrefab, transform);
            minionObject.name = "Minion " + i;
            Minion minion = minionObject.GetComponent<Minion>();
            bool minionAdded = AddMinion(minion);
            if (!minionAdded) {
                Debug.Log("failed to add minion; destroying");
                Destroy(minion);
            }
        }
    }
}
