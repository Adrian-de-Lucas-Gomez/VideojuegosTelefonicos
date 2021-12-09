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

        [SerializeField] Vector2 posIni;

        //Para nivel
        //private string level = "5,0,1,5;18,17,12;21,16,11,6;3,4,9;0,1,2,7,8,13,14,19,24,23,22;20,15,10,5";
        private int levelNumber;
        private int nFlows;
        private int boardWidth;
        private int boardHeight;
        List<List<int>> flows;
        
        //private logic.Board board;
        private List<Tile> tiles;

        //++++Pruebas
        public Vector2 offset;
        //+++++++++++

        public void Start()
        {

        }

        public void Update()
        {
            HandleInput(); //TODO:
        }

        public void GenerateBoard(string level, Color[] skin)
        {
            tiles = new List<Tile>();
            flows = new List<List<int>>();

            //TODO: pasar a leerNivel
            string[] levelInfo = level.Split(';');
            string[] auxInfo;

            //Procesamos la cabecera del nivel
            auxInfo = levelInfo[0].Split(',');
            levelNumber = int.Parse(auxInfo[2]);
            nFlows = int.Parse(auxInfo[3]);

            if (auxInfo[0].Contains(":")) { //Ancho y alto
                string[] levelSize = auxInfo[0].Split(':');
                boardWidth = int.Parse(levelSize[0]);
                boardHeight = int.Parse(levelSize[1]);
            }
            else {
                boardWidth = boardHeight = int.Parse(auxInfo[0]);
            }

            //Nivel
            for(int i = 0; i < nFlows; ++i) {
                auxInfo = levelInfo[i+1].Split(',');
                flows.Add(new List<int>());

                for (int j = 0; j < auxInfo.Length; ++j)
                    flows[i].Add(int.Parse(auxInfo[j]));
            }


            //Generar nivel
            for (int j = 0; j < boardHeight; ++j)
            {
                for (int i = 0; i < boardWidth; ++i)
                {
                    GameObject t = Instantiate(tilePrefab, new Vector2(posIni.x + offset.x / 2 + i * offset.x, posIni.y - offset.y / 2 - j * offset.y),
                        Quaternion.identity, boardObject);

                    tiles.Add(t.GetComponent<Tile>());
                }
            }

            //Asignar los extremos de los flujos
            foreach (var flow in flows) {
                tiles[flow[0]].SetAsOrigin();
                tiles[flow[flow.Count - 1]].SetAsOrigin();
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