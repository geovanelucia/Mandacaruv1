using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if (GetComponent<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition))){
            Check();
        }
    }

    void Check(){
        if (GameManager3.currentPiece.GetComponent<PieceScript>().endPosition == transform.position){
            GameManager3.currentPiece.transform.position = transform.position;
            GameManager3.currentPiece.GetComponent<SpriteRenderer>().sortingOrder = 0;
            Destroy(GameManager3.currentPiece.GetComponent<PieceScript>());
            GameManager3.currentPiece = null;
            GameManager3.currentScore++;
            Destroy(gameObject);
        } else
            GameManager3.currentPiece.GetComponent<PieceScript>().cancelPiece = true;
    }
}
