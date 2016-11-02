using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
    public MeshRenderer dropZoneMeshRenderer;
    public Collider dropZoneCollider;
    public MeshRenderer boardMeshRenderer;
    public Collider boardCollider;
    public static Board Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
    }
}
