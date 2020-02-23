using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager3 : MonoBehaviour{
    public Text text;
    public static GameObject currentPiece;
    public static int currentScore, scoreTotal;

    void Start(){
        
    }

    void Update(){
        if (currentScore == scoreTotal){
            text.gameObject.SetActive(true);
        }
    }
}
