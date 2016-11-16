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
    private bool mouseOverCastZone;
    private Vector3 boardPlanePointUnderMouse;

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
        SetMouseContext();
//        if (pieceUnderMouse != null) {
//            IDamageable target = pieceUnderMouse.GetComponent<IDamageable>();
//            if (target != null) {
//                Debug.Log("target " + target);
//            }
//        }
        HandleInput();
        DragCard();
//        MouseOverCastZone();
    }

    private void HandleInput() {

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
//            if (mouseOverDropZone) {
//                Board.Instance.dropZoneMeshRenderer.material.color = Color.white;
//            }
//            else {
//                Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
//            }

            if (draggedCard is SpellCard && ((SpellCard)draggedCard).CanUseTarget()) {
//                LineRenderer lr = draggedCard.gameObject.AddComponent<LineRenderer>();
//                Debug.Log("targeting spell cast");
//                Debug.DrawLine(CardHand.Instance.transform.position, boardPlanePointUnderMouse, Color.magenta);
                CardHand.Instance.SetLineTarget(boardPlanePointUnderMouse);
                draggedCard.SetVisible(!mouseOverCastZone);
            }

            if (DragCancelled()) {
                StopDrag();
            }
            else if (DragCompleted()) {
                if (mouseOverCastZone || targetUnderMouse != null) {
                    PlayDraggedCard();
                }
                else {
                    StopDrag();
                }
            }
//            Board.Instance.dropZoneMeshRenderer.material.color = Board.Instance.boardMeshRenderer.material.color;
        }
    }

    private bool DragCancelled() {
        return (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1));
    }

    private bool DragCompleted() {
        return !Input.GetMouseButton(0) ;  
    }

    private void DragCard() {
        if (draggedCard != null) {
            draggedCard.transform.position = Vector3.Lerp(draggedCard.transform.position, boardPlanePointUnderMouse, lerpSpeed);
        }
    }

    private void PlayDraggedCard() {
        if (draggedCard is SpellCard) {
            SpellCard spellCard = draggedCard as SpellCard;
            if (spellCard.CanUseTarget() && pieceUnderMouse != null && targetUnderMouse != null) {
                if (spellCard.Play(targetUnderMouse)) {
                    DiscardDraggedCard();
                }
//                else {
//                    StopDrag();
//                }
            }
            else if (!spellCard.RequiresTarget()) {    
                if (spellCard.Play()) {
                    DiscardDraggedCard();
                }
//                else {
//                    StopDrag();
//                }
            }
//            else {
//                StopDrag();
//            }
        }
        StopDrag();
    }

    private void StartDrag(Card card) {
        int cardHandIndex;
        card.hand.PopCard(card, out cardHandIndex);
        draggedCard = card;
        draggedCardHandIndex = cardHandIndex;
//        card.transform.localScale = Card.mouseoverScaleFactor * Vector3.one;
        card.transform.rotation = Quaternion.identity;
        draggedCard.GetComponent<Collider>().enabled = false;
        CardHand.Instance.SetLineVisible(true);
    }

    private void StopDrag() {
        if (draggedCard != null) {
            draggedCard.GetComponent<Collider>().enabled = true;
            CardHand.Instance.AddCard(draggedCard, draggedCardHandIndex);
            draggedCard.SetVisible(true);
            draggedCard = null;
        }
        draggedCardHandIndex = -1;
        CardHand.Instance.SetLineVisible(false);
    }

    private void DiscardDraggedCard() {
        Destroy(draggedCard.gameObject);
        draggedCard = null;
        draggedCardHandIndex = -1;
    }

    private void CheckPieceUnderMouse() {
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

    private void CheckBoardPointUnderMouse() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Board.Instance.boardCollider.Raycast(mouseRay, out hitInfo, 1000f);
        boardPlanePointUnderMouse = hitInfo.point;
    }

    private bool MouseOverDropZone() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        return Board.Instance.dropZoneCollider.Raycast(mouseRay, out hitInfo, 1000f);
    }

    private void CheckMouseOverCastZone() {
        float mouseYRelative = Input.mousePosition.y / Screen.height;
        mouseOverCastZone = (mouseYRelative >= 0.33f);
    }

    private void SetMouseContext() {
        CheckMouseOverCastZone();
        CheckPieceUnderMouse();
        CheckBoardPointUnderMouse();
    }
}
