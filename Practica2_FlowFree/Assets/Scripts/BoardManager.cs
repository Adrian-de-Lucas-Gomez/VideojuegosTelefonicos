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
        [SerializeField] GameObject tilePrefab;
        [SerializeField] Transform boardObject;
        [SerializeField] Vector2 posIni;

        //Para nivel
        private string level = "5,0,1,5;18,17,12;21,16,11,6;3,4,9;0,1,2,7,8,13,14,19,24,23,22;20,15,10,5"; //TODO:
        private int levelNumber;
        private int nFlows;
        private int boardWidth;
        private int boardHeight;
        private List<List<int>> solution;
        private List<Flow> flows;
        private List<Tile> tiles;

        //++++Pruebas TODO:
        public Vector2 offset;

        //+++++++++++

        //Input
        private int currentTile = 0;
        private int currentFlow = 0;
        private bool isBuildingFlow = false;


        public void Start()
        {
#if UNITY_EDITOR
            if (tilePrefab == null || boardObject == null || posIni == null || offset == null)
            {
                Debug.LogError("BoardManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif

            tiles = new List<Tile>();
            solution = new List<List<int>>();
            flows = new List<Flow>();

            GenerateBoard();
        }

        public void Update()
        {
            HandleInput(); //TODO:
        }

        public void GenerateBoard()
        {
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
                solution.Add(new List<int>());

                for (int j = 0; j < auxInfo.Length; ++j)
                    solution[i].Add(int.Parse(auxInfo[j]));
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

            //Set-up de los caminos
            for(int i = 0; i < solution.Count; i++)
            {
                flows.Add(new Flow(i));

                //Asignamos el origen y el final de los caminos
                flows[i].setOrigins(tiles[solution[i][0]], tiles[solution[i][solution[i].Count - 1]]);
                
                //Le indicamos la solución
                for(int j = 1; j < solution[i].Count - 1; j++)
                {
                    flows[i].constructSolution(tiles[solution[i][j]]);
                }
            }
        }

        private void HandleInput()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos); //Escala??

            if (IsPosInBoard(worldPos))
            {
                int newTile = WorldPosToTile(worldPos);
                int newFlow = tiles[newTile].GetColor();

                //0: boton izq, 1: boton der
                if (Input.GetMouseButtonDown(0)) //Si el usuario acaba de pulsar el botón izquierdo 
                {
                    if (tiles[newTile].IsActive())
                    {
                        currentTile = newTile;
                        currentFlow = tiles[currentTile].GetColor();

                        //3 tipos de casos
                        flows[currentFlow].startBuildingFlow(tiles[currentTile]);
                        isBuildingFlow = true;
                    }
                }

                else if (Input.GetMouseButtonUp(0)) //Si el usuario acaba de liberar el botón izquierdo 
                {
                    currentTile = int.MaxValue;
                    currentFlow = int.MaxValue;
                    isBuildingFlow = false;
                    //lol
                }

                else if (Input.GetMouseButton(0) && isBuildingFlow) //Si el usuario está pulsando el botón izquierdo
                {
                    if(newTile != currentTile && !flows[currentFlow].isClosed()) //Nos hemos movido de casilla
                    {
                        Direction dir = DirectionFromTile(currentTile, newTile);

                        if(dir != Direction.None) //Si el movimiento ha sido válido (El movimiento en diagonal es demasiado poderoso)
                        {
                            if (tiles[newTile].IsActive()) //La casilla forma parte de un flujo
                            {
                                if (newFlow != currentFlow) //Nos hemos cruzado con otro flujo
                                {
                                    if (!tiles[newTile].IsOrigin())
                                    {
                                        //Cortar el otro flujo y avanzar en esa dirección
                                        currentTile = newTile;
                                    }
                                    else
                                    {
                                        //No hacer nada
                                    }
                                }

                                else //Nos hemos cruzado con el flujo que estamos construyendo
                                {
                                    if (tiles[newTile].IsOrigin()) //Nos tenemos que asegurar que NO es el origen del que estamos construyendo
                                    {
                                        if(flows[currentFlow].getStartingTile() != tiles[currentTile])
                                        {
                                            flows[currentFlow].addToFlow(tiles[newTile], DirectionFromTile(currentTile, newTile));
                                            flows[currentFlow].closeFlow();
                                        }
                                    }

                                    else
                                    {
                                        //Corta el flujo a ver quien tiene huevos de hacer eso :_)
                                    }

                                    currentTile = newTile;
                                }
                            }

                            else //La casilla está vacía
                            {
                                flows[currentFlow].addToFlow(tiles[newTile], DirectionFromTile(currentTile, newTile));
                                currentTile = newTile;
                            }
                        }
                    }
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

        private int WorldPosToTile(Vector3 pos)
        {
            Vector2 posBoard = new Vector2(Mathf.Abs(pos.x - posIni.x), Mathf.Abs(pos.y - posIni.y));
            int col = (int)(posBoard.x / offset.x);
            int row = (int)(posBoard.y / offset.y);

            return (row * boardWidth + col);
        }

        private Direction DirectionFromTile(int origin, int dest)
        {
            if (origin + 1 == dest && origin / boardWidth == dest / boardWidth) return Direction.Right;

            else if (origin - 1 == dest && origin / boardWidth == dest / boardWidth) return Direction.Left;

            else if (origin - boardWidth == dest) return Direction.Up;

            else if (origin + boardWidth == dest) return Direction.Down;

            return Direction.None;
        }
    }
}