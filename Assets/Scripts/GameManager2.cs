using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour {

    public Puzzle puzzlePrefab;
    public string FolderName;
    //public GameObject FullPicture;
    //Moving puzzle
    public LayerMask collisionMask;

    private List<Puzzle> puzzleList = new List<Puzzle>();
    private Vector3[] auxStartPosition = new Vector3[15];
      
    private Vector2 startPosition = new Vector2(-1.85f, 1.77f);
    private Vector2 offset = new Vector2(1.01f, 0.71f);

    private bool checarEnd = false;
       
    //collision
    Ray ray_up, ray_down, ray_left, ray_right;
    RaycastHit hit;
    private BoxCollider collider;
    private Vector3 collider_size;
    private Vector3 collider_center;


	// Use this for initialization
	void Start () {
        checarEnd = false;
        SpawnPuzzle(14);
        SetStartPosition();
        ApplyMaterial();
    }
	
	// Update is called once per frame
	void Update () {
        MovePuzzle();
        if (checarEnd) {
            if (CheckEnd(14))
            {
                EndGame();
            }
        }
    }

    private void SpawnPuzzle(int number)
    {
        for(int i=0; i<=number; i++)
        {
            puzzleList.Add(Instantiate(puzzlePrefab, new Vector3(0.47f, 0.252f, 0.0f), new Quaternion(0.0f, 0.0f, 180.0f, 0.0f)) as Puzzle);
        }
    }

    private void SetStartPosition()
    {
        // first line
        puzzleList[0].transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);
        auxStartPosition[0] = puzzleList[0].transform.position;
        puzzleList[1].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y, 0.0f);
        auxStartPosition[1] = puzzleList[1].transform.position;
        puzzleList[2].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y, 0.0f);
        auxStartPosition[2] = puzzleList[2].transform.position;


        // second line
        puzzleList[3].transform.position = new Vector3(startPosition.x, startPosition.y - offset.y, 0.0f);
        auxStartPosition[3] = puzzleList[3].transform.position;
        puzzleList[4].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - offset.y, 0.0f);
        auxStartPosition[4] = puzzleList[4].transform.position;
        puzzleList[5].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - offset.y, 0.0f);
        auxStartPosition[5] = puzzleList[5].transform.position;
        puzzleList[6].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - offset.y, 0.0f);
        auxStartPosition[6] = puzzleList[6].transform.position;


        // third line
        puzzleList[7].transform.position = new Vector3(startPosition.x, startPosition.y - (2 * offset.y), 0.0f);
        auxStartPosition[7] = puzzleList[7].transform.position;
        puzzleList[8].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - (2 * offset.y), 0.0f);
        auxStartPosition[8] = puzzleList[8].transform.position;
        puzzleList[9].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - (2 * offset.y), 0.0f);
        auxStartPosition[9] = puzzleList[9].transform.position;
        puzzleList[10].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - (2 * offset.y), 0.0f);
        auxStartPosition[10] = puzzleList[10].transform.position;


        // fourth line
        puzzleList[11].transform.position = new Vector3(startPosition.x, startPosition.y - (3 * offset.y), 0.0f);
        auxStartPosition[11] = puzzleList[11].transform.position;
        puzzleList[12].transform.position = new Vector3(startPosition.x + offset.x, startPosition.y - (3 * offset.y), 0.0f);
        auxStartPosition[12] = puzzleList[12].transform.position;
        puzzleList[13].transform.position = new Vector3(startPosition.x + (2 * offset.x), startPosition.y - (3 * offset.y), 0.0f);
        auxStartPosition[13] = puzzleList[13].transform.position;
        puzzleList[14].transform.position = new Vector3(startPosition.x + (3 * offset.x), startPosition.y - (3 * offset.y), 0.0f);
        auxStartPosition[14] = puzzleList[14].transform.position;

        //shufle
        shufle();
    }

    private void shufle()
    {
        Vector3 aux;
        aux = puzzleList[5].transform.position;
        puzzleList[5].transform.position = puzzleList[2].transform.position;
        puzzleList[2].transform.position = puzzleList[6].transform.position;
        puzzleList[6].transform.position = aux;
        checarEnd = true;
    }

    public bool CheckEnd( int number)
    {
        bool retorno = true ;

        for (int i=0; i < number ; i++)
        {
            if(puzzleList[i].transform.position != auxStartPosition[i])
            {
                retorno = false;
            }
        }
        return retorno;
    }

    void MovePuzzle()
    {
        foreach (Puzzle puzzle in puzzleList)
        {
            puzzle.move_amount = offset;

            if(puzzle.clicked)
            {
                // ray up and down
                collider = puzzle.GetComponent<BoxCollider>();
                collider_size = collider.size;
                collider_center = collider.center;

                float move_amount = offset.x;
                float direction = Mathf.Sign(move_amount);

                //set rays
                float x = (puzzle.transform.position.x + collider_center.x - collider_size.x / 4) + collider_size.x / 4;
                float y_up = puzzle.transform.position.y + collider_center.y + collider_size.y / 4 * direction;
                float y_down = puzzle.transform.position.y + collider_center.y + collider_size.y / 4 * -direction;
                                
                ray_up = new Ray(new Vector2(x, y_up), new Vector2(0, direction));
                ray_down = new Ray(new Vector2(x, y_down), new Vector2(0, -direction));

                // draw rays
                Debug.DrawRay(ray_up.origin, ray_up.direction);
                Debug.DrawRay(ray_down.origin, ray_down.direction);

                // left and right ray
                float y = (puzzle.transform.position.y + collider_center.y - collider_size.y / 4) + collider_size.y / 4;
                float x_right = puzzle.transform.position.x + collider_center.x + collider_size.x / 4 * direction;
                float x_left = puzzle.transform.position.x + collider_center.x + collider_size.x / 4 * -direction;

                ray_left = new Ray(new Vector2(x_left, y), new Vector2(-direction, 0f));
                ray_right = new Ray(new Vector2(x_right, y), new Vector2(direction, 0f));

                // desenha rays
                Debug.DrawRay(ray_left.origin, ray_left.direction);
                Debug.DrawRay(ray_right.origin, ray_right.direction);


                //avalia colisão para Cima
                if ((Physics.Raycast(ray_up, out hit, 0.5f, collisionMask) == false) && puzzle.moved == false && (puzzle.transform.position.y < startPosition.y))
                {
                    Debug.Log("Movimento para cima permitido");
                    puzzle.go_up = true;
                }
                else
                {
                    Debug.Log("== CIMA NÃO - START ==");
                    Debug.Log("Physics.Raycast(ray_up, out hit, 1.0f, collisionMask): " + Physics.Raycast(ray_up, out hit, 1.0f, collisionMask));
                    Debug.Log("puzzle.moved: " + puzzle.moved);
                    Debug.Log("puzzle.transform.position.y: " + puzzle.transform.position.y);
                    Debug.Log("startPosition.y: " + startPosition.y);
                    Debug.Log("== CIMA NÃO - END ==");
                }
                

                //avalia colisão para Baixo
                if ((Physics.Raycast(ray_down, out hit, 0.5f, collisionMask) == false) && puzzle.moved == false && (puzzle.transform.position.y > (startPosition.y - 3 * offset.y)))
                {
                    Debug.Log("Movimento para baixo permitido");
                    puzzle.go_down = true;
                }
                   

                //avalia colisão Esquerda
                if ((Physics.Raycast(ray_left, out hit, 1.0f, collisionMask) == false) && puzzle.moved == false && puzzle.transform.position.x > (startPosition.x))
                {
                    Debug.Log("Movimento para esquerda permitido");
                    puzzle.go_left = true;
                }
                        
                //avalia colisão Direita
                if ((Physics.Raycast(ray_right, out hit, 1.0f, collisionMask) == false) && puzzle.moved == false && (puzzle.transform.position.x < (startPosition.x + 3 * offset.x)))
                {
                    Debug.Log("Movimento para direita permitido");
                    puzzle.go_right = true;
                }
            }
                    
                
            
        }
    }

    void ApplyMaterial()
    {
        string filePath;
        for(int i=1; i <= puzzleList.Count; i++)
        {
            if (i > 3)
            {
                filePath = FolderName + "/Cube" + (i + 1);

            }
            else
            {
                filePath = FolderName + "/Cube" + i;
            }

            Texture2D mat = Resources.Load(filePath, typeof(Texture2D)) as Texture2D;
            puzzleList[i - 1].GetComponent<Renderer>().material.mainTexture = mat;
        }

        //filePath = FolderName + "/pic";
        //Texture2D mat1 = Resources.Load(filePath, typeof(Texture2D)) as Texture2D;
        //FullPicture.GetComponent<Renderer>().material.mainTexture = mat1;
    }

   public void EndGame()
    {
        /////// TO DO
        ////// MOSTRAR FINAL
        //Debug.Log("FIM DO JOGO");
        Application.LoadLevel("FimDeJogo");
    }

}
