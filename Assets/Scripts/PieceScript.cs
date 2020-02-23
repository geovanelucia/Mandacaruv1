using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour {

    [HideInInspector]
    public Vector3 startPosition, endPosition;
    [HideInInspector]
    public bool canMove = false, cancelPiece = false;

    private SpriteRenderer sprite;
    private float timeToLerp = 20;

    private void OnMouseOver(){
        if (Input.GetMouseButtonDown(0) && !cancelPiece && GameManager3.currentPiece == null) {
            GameManager3.currentPiece = gameObject;
            canMove = true;
        }
        if (Input.GetMouseButtonDown(1) && !cancelPiece && canMove){
            cancelPiece = true;
        }
    }

    void CancelPiece(){
        GameManager3.currentPiece = null;
        transform.position = Vector2.MoveTowards(
            transform.position, startPosition, Time.deltaTime * timeToLerp);
        canMove = false;
        if (transform.position == startPosition){
            sprite.sortingOrder = 0;
            cancelPiece = false;
        }
    }

    // Start is called before the first frame update
    void Start(){
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if (canMove){
            sprite.sortingOrder = 1;
            Vector3 mouseP = Input.mousePosition;
            mouseP.z = transform.position.z - Camera.main.transform.position.z;
            transform.position = Camera.main.ScreenToWorldPoint(mouseP);
        }
        if (cancelPiece){
            CancelPiece();
        }
    }
}
