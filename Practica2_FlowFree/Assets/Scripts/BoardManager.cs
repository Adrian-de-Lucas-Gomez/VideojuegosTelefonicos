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
        [SerializeField] int posIniX;
        [SerializeField] int posIniY;

        private logic.Board board;
        private List<Tile> tiles;

        //++++Pruebas
        public float offsetX;
        public float offsetY;
        public int tilex, tiley;
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
                    GameObject t = Instantiate(tilePrefab, new Vector2(posIniX + i*offsetX, posIniY - j*offsetY), Quaternion.identity, boardObject);
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
                    Debug.Log("warro");
                }
            }
        }

        private bool IsPosInBoard(Vector3 pos)
        {
            if (pos.x >= posIniX - offsetX/2 && pos.x < posIniX - offsetX/2 + boardWidth * offsetX &&
                pos.y <= posIniY + offsetY/2 && pos.y > posIniY + offsetY/2 - boardHeight * offsetY)
                return true;

            return false;
        }
    }
}