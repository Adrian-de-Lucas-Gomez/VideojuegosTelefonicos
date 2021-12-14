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
        //private string level = "5,0,1,5;18,17,12;21,16,11,6;3,4,9;0,1,2,7,8,13,14,19,24,23,22;20,15,10,5";
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
        private int previousFlow = int.MaxValue;
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
        }

        public void Update()
        {
            HandleInput(); //TODO:
        }

        public void ReadLevel(string level, ref List<int> emptyTiles, ref List<(int, int)> walls) //TODO: dberia estar aqui? o en un script bobo "board"??
        {
            string[] levelInfo = level.Split(';');
            string[] auxInfo;

            //Procesamos la cabecera del nivel
            auxInfo = levelInfo[0].Split(',');
            levelNumber = int.Parse(auxInfo[2]);
            nFlows = int.Parse(auxInfo[3]);

            if (auxInfo[0].Contains(":")) //Ancho y alto
            { 
                string[] levelSize = auxInfo[0].Split(':');
                boardWidth = int.Parse(levelSize[0]);
                boardHeight = int.Parse(levelSize[1]);
            }
            else
            {
                boardWidth = boardHeight = int.Parse(auxInfo[0]);
            }

            //auxInfo[4] son los puentes: No se procesan

            if (auxInfo.Length >= 6) //Celdas huecas (0:6:12:4:17)
            {
                string[] listEmpty = auxInfo[5].Split(':');

                for (int i = 0; i < listEmpty.Length; ++i)
                    emptyTiles.Add(int.Parse(listEmpty[i].Split('_')[0]));

                if (auxInfo.Length >= 7) //Celdas separadas por muros (0|6:12|4:17|3)
                {
                    string[] listWalls = auxInfo[6].Split(':'); 

                    for (int i = 0; i < listWalls.Length; ++i) //(0|6)
                    {
                        string[] aux = listWalls[i].Split('|');

                        walls.Add((int.Parse(aux[0]), int.Parse(aux[1])));
                    }
                }
            }

            //Flujos nivel
            for(int i = 0; i < nFlows; ++i)
            {
                auxInfo = levelInfo[i+1].Split(',');
                solution.Add(new List<int>());

                for (int j = 0; j < auxInfo.Length; ++j)
                    solution[i].Add(int.Parse(auxInfo[j]));
            }
        }


        public void GenerateBoard(string level, Color[] skin)
        {
            tiles = new List<Tile>();
            flows = new List<Flow>();
            solution = new List<List<int>>();

            List<int> emptyTiles = new List<int>();
            List<(int, int)> walls = new List<(int, int)>();

            //Lectura del nivel
            ReadLevel(level, ref emptyTiles, ref walls);

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
                flows.Add(new Flow(i, skin[i], boardWidth));

                //Asignamos el origen y el final de los caminos
                flows[i].setOrigins(tiles[solution[i][0]], solution[i][0], tiles[solution[i][solution[i].Count - 1]], solution[i][solution[i].Count - 1]);

                tiles[solution[i][0]].SetTempColor(skin[i]);
                tiles[solution[i][solution[i].Count - 1]].SetTempColor(skin[i]);

                //Le indicamos la solucion
                for (int j = 1; j < solution[i].Count - 1; j++)
                {
                    flows[i].ConstructSolution(tiles[solution[i][j]], solution[i][j]);
                }
            }

            //Asignar casillas vacias
            for (int i = 0; i < emptyTiles.Count; i++)
            {
                tiles[emptyTiles[i]].SetEmpty();
            }

            //Asignar muros
            for (int i = 0; i < walls.Count; i++)
            {
                int tile1 = walls[i].Item1;
                int tile2 = walls[i].Item2;
                Direction aux = DirectionUtils.DirectionBetweenTiles(tile1, tile2, boardWidth);

                tiles[tile1].SetWall(aux, true);
                tiles[tile2].SetWall(DirectionUtils.GetOppositeDirection(aux), true);
            }
        }


        private void HandleInput()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos); //Escala??

            if (Input.GetMouseButtonUp(0)) //Si el usuario acaba de liberar el boton izquierdo 
            {
                if (previousFlow != int.MaxValue)
                {
                    flows[previousFlow].CloseSmallCircle();
                }

                previousFlow = currentFlow;

                if (currentFlow != int.MaxValue)
                {
                    flows[currentFlow].StopBuldingFlow();
                    for (int k = 0; k < flows.Count; k++) if (k != currentFlow) flows[k].ApplyProvisionalCut();
                    currentTile = int.MaxValue;
                    currentFlow = int.MaxValue;
                    isBuildingFlow = false;
                }
            }

            if (IsPosInBoard(worldPos))
            {
                int newTile = WorldPosToTile(worldPos);
                int newFlow = tiles[newTile].GetColor();

                //0: boton izq, 1: boton der
                if (Input.GetMouseButtonDown(0)) //Si el usuario acaba de pulsar el boton izquierdo 
                {
                    if (tiles[newTile].IsActive())
                    {
                        currentTile = newTile;
                        currentFlow = tiles[currentTile].GetColor();

                        //Partimos de un origen. Empezamos a construir a partir del mismo
                        if(tiles[newTile].IsOrigin()) flows[currentFlow].StartBuildingFlow(tiles[currentTile], currentTile);
                        //Partimos de un camino a medio construir. Lo cortamos hasta ese punto.
                        else
                        {
                            int previousPos = flows[currentFlow].CutFlow(newTile);
                            flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(previousPos, newTile, boardWidth));
                        }
                        isBuildingFlow = true;
                    }
                }

                else if (Input.GetMouseButton(0) && isBuildingFlow) //Si el usuario esta pulsando el boton izquierdo
                {
                    if(newTile != currentTile && !flows[currentFlow].isClosed()) //Nos hemos movido de casilla
                    {
                        Direction dir = DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth);

                        if(dir != Direction.None && tiles[newTile].CanBeAccesed(dir)) //Si el movimiento ha sido valido (El movimiento en diagonal es demasiado poderoso)
                        {
                            if (tiles[newTile].IsActive()) //La casilla forma parte de un flujo
                            {
                                if (newFlow != currentFlow) //Nos hemos cruzado con otro flujo
                                {
                                    if (!tiles[newTile].IsOrigin())
                                    {
                                        //Cortar el otro flujo y avanzar en esa direccion
                                        int previousTile = flows[newFlow].ProvisionalCut(newTile);
                                        tiles[previousTile].ClearDirection(DirectionUtils.DirectionBetweenTiles(previousTile, newTile, boardWidth));
                                        flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth));
                                        currentTile = newTile;
                                    }
                                }

                                else //Nos hemos cruzado con el flujo que estamos construyendo
                                {
                                    if (tiles[newTile].IsOrigin()) //Nos tenemos que asegurar que NO es el origen del que estamos construyendo
                                    {
                                        if(!flows[currentFlow].Contains(newTile))
                                        {
                                            flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth));
                                            flows[currentFlow].CloseFlow();
                                        }
                                        else
                                        {
                                            flows[currentFlow].CutFlow(newTile);
                                            for (int k = 0; k < flows.Count; k++) if (k != currentFlow) flows[k].RecalculateCut(flows[currentFlow], newTile);
                                            //flows[currentFlow].addToFlow(tiles[newTile], newTile, DirectionFromTile(lastTileInFlow, newTile));
                                            currentTile = newTile;
                                        }
                                    }

                                    else
                                    {
                                        int lastTileInFlow = flows[currentFlow].CutFlow(newTile);
                                        flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(lastTileInFlow, newTile, boardWidth));
                                        for (int k = 0; k < flows.Count; k++) if (k != currentFlow) flows[k].RecalculateCut(flows[currentFlow], newTile);
                                        currentTile = newTile;
                                    }
                                }
                            }

                            else //La casilla esta vacia
                            {
                                flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth));
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
    }
}