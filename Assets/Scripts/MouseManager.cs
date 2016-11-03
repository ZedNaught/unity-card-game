using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
//    public GameObject board;
    public static MouseManager Instance { get; set; }

    private float lerpSpeed = 0.3f;
    private int piecesLayer;
    private Card draggedCard;
    private int draggedCardHandIndex;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        piecesLayer = LayerMask.NameToLayer("Pieces");
    }

    private void Update() {
        HandleInput();
        DragCard();
    }

    private void HandleInput() {
        bool mouseOverDropZone = MouseOverDropZone();

        if (!draggedCard) {
            GameObject piece = GetPieceUnderMouse();
            if (piece) {
                Card card = piece.GetComponent<Card>();
                if (card) {
                    if (Input.GetMouseButtonDown(0)) {
                        StartDrag(card);
                    }
                    else {
                        CardHand.Instance.HighlightCard(card);
                    }
                }
                else {
                    CardHand.Instance.HighlightCard(null);
                }
            }
            else {
                CardHand.Instance.HighlightCard(null);
            }
        }

        else {
            if (mouseOverDropZone) {
                Board.Instance.dropZoneMeshRenderer.material.color = Color.white;
            }
            else {
                Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
            }

            if (!Input.GetMouseButton(0)) {
                if (mouseOverDropZone) {
                    PlayDraggedCard();
                }
                else {
                    StopDrag();
                }
                Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
            }
        }
    }

    private void DragCard() {
        if (draggedCard) {
            Vector3? possiblePointOnBoard = GetBoardPointUnderMouse();
            if (possiblePointOnBoard.HasValue) {
                Vector3 pointOnBoard = (Vector3)possiblePointOnBoard;
                draggedCard.transform.position = Vector3.Lerp(draggedCard.transform.position, pointOnBoard, lerpSpeed);
            }
        }
    }

    private void PlayDraggedCard() {
        if (draggedCard is SpellCard) {
            SpellCard spellCard = draggedCard as SpellCard;
            if (!spellCard.RequiresTarget()) {    
                if (spellCard.Play()) {
                    Destroy(draggedCard.gameObject);
                    draggedCard = null;
                    draggedCardHandIndex = -1;
                }
                else {
                    StopDrag();
                    return;
                }
            }
            else {
                Debug.Log("need to implement playing spell card with target");  // TODO //
            }
        }
    }

    private void StartDrag(Card card) {
        int cardHandIndex;
        card.hand.PopCard(card, out cardHandIndex);
        draggedCard = card;
        draggedCardHandIndex = cardHandIndex;
        card.transform.localScale = Card.highlightScaleFactor * Vector3.one;
        card.transform.rotation = Quaternion.identity;
    }

    private void StopDrag() {
        if (draggedCard) {
            CardHand.Instance.AddCard(draggedCard, draggedCardHandIndex);
        }

        draggedCard = null;
        draggedCardHandIndex = -1; 
    }

    private GameObject GetPieceUnderMouse() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(mouseRay, 1000f, 1 << piecesLayer);
        if (hits.Length > 0) {
            GameObject pieceUnderMouse = hits[0].collider.gameObject;
            foreach (RaycastHit hit in hits) {
                if (hit.collider.GetComponentInChildren<Canvas>().sortingOrder > pieceUnderMouse.GetComponentInChildren<Canvas>().sortingOrder) {
                    pieceUnderMouse = hit.collider.gameObject;
                }
            }
            return pieceUnderMouse;
        }
        return null;
    }

    public Vector3? GetBoardPointUnderMouse() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Board.Instance.boardCollider.Raycast(mouseRay, out hitInfo, 1000f)) {
            return hitInfo.point;
        }
        return null;
    }

    public bool MouseOverDropZone() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        return Board.Instance.dropZoneCollider.Raycast(mouseRay, out hitInfo, 1000f);
    }
}
