using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    /// <summary>
    /// Se encarga de la gestion del tablero y la ejecucion del nivel (Board)
    /// Instancia los prefabs (Tiles) recibe un Map, configura los Tiles y
    /// los distribuye.
    /// Se encarga del input en el nivel (Board)
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        //TODO: en el start tiene que decirte si eres bobo y no le has puesto valor desde el editor con un debug log
        public GameObject tilePrefab;

        [SerializeField] Transform boardObject;
        [SerializeField] int boardWidth;
        [SerializeField] int boardHeight;
        [SerializeField] Vector2 posIni;

        private logic.Board board;
        private List<Tile> tiles;

        //++++Pruebas
        public Vector2 offset;
        //+++++++++++

        public void Start()
        {
            tiles = new List<Tile>();

            GenerateBoard();
            //tiles[tiley * boardWidth + tilex].SetColor(Color.blue);
        }

        public void Update()
        {
            HandleInput(); //TODO:
        }

        public void GenerateBoard()
        {
            //board = new logic.Board(boardWidth, boardHeight);

            for(int j = 0; j < boardHeight; ++j)
            {
                for(int i = 0; i < boardWidth; ++i)
                {
                    GameObject t = Instantiate(tilePrefab, new Vector2(posIni.x + offset.x/2 + i*offset.x, posIni.y - offset.y/ 2 - j* offset.y),
                        Quaternion.identity, boardObject);
                    tiles.Add(t.GetComponent<Tile>());
                }
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButton(0)) //0: boton izq
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos); //Escala??

                if(IsPosInBoard(worldPos))
                {
                    Vector2 tileInBoard = WorldPosToTile(worldPos);
                    Debug.Log(tileInBoard);
                }
            }
        }

        private bool IsPosInBoard(Vector3 pos)
        {
            if (pos.x >= posIni.x && pos.x < posIni.x + boardWidth * offset.x &&
                pos.y <= posIni.y && pos.y > posIni.y - boardHeight * offset.y)
                return true;

            return false;
        }

        private Vector2 WorldPosToTile(Vector3 pos)
        {
            Vector2 posBoard = new Vector2(Mathf.Abs(pos.x - posIni.x), Mathf.Abs(pos.y - posIni.y));
            int col = (int)(posBoard.x / offset.x);
            int row = (int)(posBoard.y / offset.y);

            return new Vector2(col, row);
        }
    }
}