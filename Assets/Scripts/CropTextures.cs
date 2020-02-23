using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropTextures : MonoBehaviour {

    public enum Options {
        Grid2X2 = 2,
        Grid3X3 = 3,
        Grid4X4 = 4,
        Grid5X5 = 5,
        Grid6X6 = 6,
        Grid7X7 = 7,
        Grid8X8 = 8,
        Grid9X9 = 9
    };

    public Options GridType;
    public Texture2D sourceTexture; 
    public GameObject piecePrefab, gridPrefab;

    private int amountPieces;
    private List<Vector2> positions = new List<Vector2>();
    private List<Vector2> sortedPositions = new List<Vector2>();
    private Vector2 position, distancePieces, resolutionPieces;

    void StartComponents(){
        amountPieces = (int)GridType;
        resolutionPieces = new Vector2(sourceTexture.width  / amountPieces,
                                        sourceTexture.height / amountPieces);
        GameManager3.currentScore = 0;
        GameManager3.scoreTotal = amountPieces * amountPieces;
    }

    Texture2D CropTexture (int row, int line) {
        var resolutionX = Mathf.RoundToInt(resolutionPieces.x);
        var resolutionY = Mathf.RoundToInt(resolutionPieces.y);
        Color[] pixels = sourceTexture.GetPixels(row*resolutionX, line*resolutionY,
                                                 resolutionX, resolutionY);

        Texture2D tex = new Texture2D(resolutionX, resolutionY);
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    void CreatePositions() {
        distancePieces = new Vector2(resolutionPieces.x / 100.0f, resolutionPieces.y / 100.0f);
        for (int x = 0; x < amountPieces; x++) {
            for (int y = 0; y < amountPieces; y++) {
                positions.Add(new Vector2(x * distancePieces.x, y * distancePieces.y));
            }
        }
    }

    Vector2 RandomPosition() {
        var sorted = false;
        var pos = Vector2.zero;

        while (!sorted) {
            pos = positions[Random.Range(0, positions.Count)];
            sorted = !sortedPositions.Contains(pos);
            if (sorted) {
                sortedPositions.Add(pos);
            }
        }
        pos = new Vector2((pos.x - 1.95f), pos.y);
        return pos;
    }

    void CreatePiece() {
        var start = amountPieces - 1;
        for (int i = start; i >= 0; i--) {
            for (int j = 0; j < amountPieces; j++) {
                var texture = CropTexture(j, i);
                position = RandomPosition();
                var quad = Instantiate (piecePrefab, position, Quaternion.identity) as GameObject;
                quad.GetComponent<SpriteRenderer>().sprite =
                    Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                quad.GetComponent<BoxCollider2D>().size = 
                    new Vector2(distancePieces.x, distancePieces.y);
                quad.GetComponent<PieceScript>().startPosition = position;
                CreateGrid(j, i, quad);
            }
        }
    }

    void CreateGrid(int j, int i, GameObject quad) {
        var grid = Instantiate(gridPrefab,
                            new Vector2((j * distancePieces.x), i * distancePieces.y - 2.7f),
                            Quaternion.identity) as GameObject;
        var newScale = new Vector2(resolutionPieces.x / 150f, resolutionPieces.y / 150f);
        grid.transform.localScale = new Vector3(newScale.x, newScale.y, 0);
        quad.GetComponent<PieceScript>().endPosition = grid.transform.position;
    }

    // Start is called before the first frame update
    void Start(){
        StartComponents();
        CreatePositions();
        CreatePiece();
    }

    // Update is called once per frame
    void Update(){
        
    }
}
