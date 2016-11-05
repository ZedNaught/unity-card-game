using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
//    public GameObject board;
    public static MouseManager Instance { get; set; }

    private float lerpSpeed = 0.3f;
    private int piecesLayer;
    private Card draggedCard;
    private int draggedCardHandIndex;
    private GameObject pieceUnderMouse;
    private ITargetable targetUnderMouse;

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
        GetPieceUnderMouse();
//        if (pieceUnderMouse != null) {
//            IDamageable target = pieceUnderMouse.GetComponent<IDamageable>();
//            if (target != null) {
//                Debug.Log("target " + target);
//            }
//        }
        HandleInput();
        DragCard();
        MouseOverCastZone();
    }

    private void HandleInput() {
        bool mouseOverDropZone = MouseOverDropZone();

        if (draggedCard == null) {
            if (pieceUnderMouse != null) {
                Card card = pieceUnderMouse.GetComponent<Card>();
                if (card != null) {
                    if (Input.GetMouseButtonDown(0)) {
                        StartDrag(card);
                    }
                    else {
                        CardHand.Instance.MouseoverCard(card);
                    }
                }
                else {
                    CardHand.Instance.MouseoverCard(null);
                }
            }
            else {
                CardHand.Instance.MouseoverCard(null);
            }
        }

        else {
            if (mouseOverDropZone) {
                Board.Instance.dropZoneMeshRenderer.material.color = Color.white;
            }
            else {
                Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
            }

            if (DragCancelled()) {
                if (mouseOverDropZone || targetUnderMouse != null) {
                    PlayDraggedCard();
                }
                else {
                    StopDrag();
                }
                Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
            }
        }
    }

    private bool DragCancelled() {
        return (!Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1));
    }

    private void DragCard() {
        if (draggedCard != null) {
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
            if (spellCard.CanUseTarget() && pieceUnderMouse != null && targetUnderMouse != null) {
                if (spellCard.Play(targetUnderMouse)) {
                    Destroy(draggedCard.gameObject);
                    draggedCard = null;
                    draggedCardHandIndex = -1;
                }
                else {
                    StopDrag();
                }
            }
            else if (!spellCard.RequiresTarget()) {    
                if (spellCard.Play()) {
                    Destroy(draggedCard.gameObject);
                    draggedCard = null;
                    draggedCardHandIndex = -1;
                }
                else {
                    StopDrag();
                }
            }
            else {
                StopDrag();
            }
        }
    }

    private void StartDrag(Card card) {
        int cardHandIndex;
        card.hand.PopCard(card, out cardHandIndex);
        draggedCard = card;
        draggedCardHandIndex = cardHandIndex;
//        card.transform.localScale = Card.mouseoverScaleFactor * Vector3.one;
        card.transform.rotation = Quaternion.identity;
        draggedCard.GetComponent<Collider>().enabled = false;
    }

    private void StopDrag() {
        if (draggedCard != null) {
            draggedCard.GetComponent<Collider>().enabled = true;
            CardHand.Instance.AddCard(draggedCard, draggedCardHandIndex);
        }

        draggedCard = null;
        draggedCardHandIndex = -1; 
    }

    private void GetPieceUnderMouse() {
        pieceUnderMouse = null;
        targetUnderMouse = null;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(mouseRay, 1000f, 1 << piecesLayer);
        if (hits.Length > 0) {
            pieceUnderMouse = hits[0].collider.gameObject;
            foreach (RaycastHit hit in hits) {
                if (hit.collider.GetComponentInChildren<Canvas>().sortingOrder > pieceUnderMouse.GetComponentInChildren<Canvas>().sortingOrder) {
                    pieceUnderMouse = hit.collider.gameObject;
                }
            }
            targetUnderMouse = pieceUnderMouse.GetComponent<ITargetable>();
            if (targetUnderMouse != null) {
                targetUnderMouse.Highlight(Color.white);
            }
        }
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

    public bool MouseOverCastZone() {
        float mouseYRelative = Input.mousePosition.y / Screen.height;
        bool mouseOverCastZone = (mouseYRelative >= 0.33f && mouseYRelative < 1f);
        if (draggedCard != null) {
            draggedCard.SetVisible(!mouseOverCastZone);
        }
        Debug.Log(mouseOverCastZone);
        return mouseOverCastZone;
    }
}
