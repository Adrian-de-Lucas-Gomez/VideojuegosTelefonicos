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
        public int offsetX;
        public int offsetY;
        public int tilex, tiley;
        //+++++++++++

        public void Start()
        {
            tiles = new List<Tile>();

            GenerateBoard();
            tiles[tiley * boardWidth + tilex].SetColor(Color.blue);
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
    }
}