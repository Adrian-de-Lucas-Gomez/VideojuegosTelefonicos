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
        [SerializeField] SpriteRenderer pointerIndicatorSprite;

        //Generacion del nivel
        private int levelNumber;
        private int nFlows;
        private bool isSurroundedByWalls = false;
        private int boardWidth;
        private int boardHeight;
        private List<List<int>> solution;
        private List<Flow> flows;
        private List<Tile> tiles;
        private PointerIndicator pointerIndicator;
        private Color[] colors;


        //Logica
        private Vector2 posIni;
        private Vector2 tileSize;
        private int currentTile = 0;
        private int currentFlow = int.MaxValue;
        private int previousFlow = int.MaxValue;
        private bool isBuildingFlow = false;

        private int numFillableTiles = 0;
        private int numFilledTiles = 0;
        private int numMoves = 0;
        private bool finished = false;


        void Start()
        {
#if UNITY_EDITOR
            if (tilePrefab == null || boardObject == null || pointerIndicatorSprite == null)
            {
                Debug.LogError("BoardManager: Alguna variable no tiene valor asociado desde el editor.");
                return;
            }
#endif
        }

        public void Update()
        {
            if (!finished)
            {
                HandleInput();
                GetPercentageFilled();
                CheckFlows();
            }
        }

        private void CheckFlows()
        {
            int i = 0;
            bool closed = true;

            while (i < flows.Count && closed)
            {
                closed = flows[i].GetClosed();
                i++;
            }

            if (closed && !isBuildingFlow)
            {
                bool correct = true;
                int flow = 0;

                while (flow < flows.Count && correct)
                {
                    correct = flows[flow].IsSolved(solution[flow]);
                    flow++;
                }

                if (correct)
                {
                    finished = true;
                    numFilledTiles = numFillableTiles;
                    for (int k = 0; k < flows.Count; k++) flows[k].PlayEndingAnimation();
                    //Llamada al onLevelFinished del GameManager para que guarde y avise al Level para que ponga la interfaz correspondiente
                    GameManager.GetInstance().OnLevelFinished(numMoves); //Son los moves usados
                }
            }
        }

        public void ReadLevel(string level, ref List<int> emptyTiles, ref List<(int, int)> walls)
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
                if(levelSize[1].Contains("+")) {
                    boardHeight = int.Parse(levelSize[1].Split('+')[0]);
                    isSurroundedByWalls = levelSize[1].Split('+')[1] == "B";
                }
                else boardHeight = int.Parse(levelSize[1]);
            }
            else
            {
                if(auxInfo[0].Contains("+"))
                {
                    boardWidth = boardHeight = int.Parse(auxInfo[0].Split('+')[0]);
                    isSurroundedByWalls = auxInfo[0].Split('+')[1] == "B";
                }
                boardWidth = boardHeight = int.Parse(auxInfo[0]);
            }

            //Calcular la posicion desde la que se instanciaran las casillas
            posIni = new Vector2(-(float)boardWidth / 2f, (float)boardHeight / 2f);

            numFillableTiles = boardWidth * boardHeight - nFlows;

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

        public int GetTotalFlows()
        {
            return nFlows;
        }
        public int GetNumFlows()
        {
            int flowsDone = 0;
            for (int i = 0; i < flows.Count; i++)
            {
                if (flows[i].GetClosed())
                {
                    flowsDone++;
                }
            }
            return flowsDone;
        }
        public int GetNumMoves()
        {
            return numMoves;
        }
        public float GetPercentage()
        {
            float aux = numFilledTiles;
            return (aux / numFillableTiles) * 100;
        }

        public void ResetLevel()
        {
            currentTile = 0;
            currentFlow = int.MaxValue;
            previousFlow = int.MaxValue;
            isBuildingFlow = false;

            numFilledTiles = 0;
            numMoves = 0;
            finished = false;

            for (int k = 0; k < flows.Count; k++) flows[k].ClearFlow();
        }

        private void Setup()
        {
            currentTile = 0;
            currentFlow = int.MaxValue;
            previousFlow = int.MaxValue;
            isBuildingFlow = false;

            numFillableTiles = 0;
            numFilledTiles = 0;
            numMoves = 0;
            finished = false;

            if(tiles != null)
            {
                foreach (Transform child in boardObject.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            tiles = new List<Tile>();
            flows = new List<Flow>();
            solution = new List<List<int>>();

            pointerIndicator = new PointerIndicator();
            pointerIndicator.SetSprite(pointerIndicatorSprite);
        }

        public (int, int) GenerateBoard(string level, Color[] skin, float boardScale)
        {
            Setup();
            colors = skin;
            //Lectura del nivel
            List<int> emptyTiles = new List<int>();
            List<(int, int)> walls = new List<(int, int)>();
            ReadLevel(level, ref emptyTiles, ref walls);
            //El tamanho de un tile en pantalla
            tileSize = Vector2.one;
            //TODO: no funciona el escalado
            //tileSize = new Vector2((int)boardScale / boardWidth, (int)boardScale / boardHeight);

            //Generar nivel
            for (int j = 0; j < boardHeight; ++j)
            {
                for (int i = 0; i < boardWidth; ++i)
                {
                    GameObject t = Instantiate(tilePrefab, new Vector2(posIni.x + tileSize.x / 2 + i * tileSize.x, posIni.y - tileSize.y / 2 - j * tileSize.y),
                        Quaternion.identity, boardObject);
                    t.transform.localScale = tileSize; //Escalamos el tile
                    
                    Tile newTile = t.GetComponent<Tile>();
                    newTile.Initialize();
                    tiles.Add(t.GetComponent<Tile>());
                }
            }

            //Set-up de los caminos
            for(int i = 0; i < solution.Count; i++)
            {
                flows.Add(new Flow(i, colors[i], boardWidth));

                //Asignamos el origen y el final de los caminos
                flows[i].SetOrigins(tiles[solution[i][0]], solution[i][0], tiles[solution[i][solution[i].Count - 1]], solution[i][solution[i].Count - 1]);

                tiles[solution[i][0]].SetRenderColor(colors[i]);
                tiles[solution[i][solution[i].Count - 1]].SetRenderColor(colors[i]);
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
            if (isSurroundedByWalls) SurrondBoardByWalls();
            SetWallsOnEmptyTiles(emptyTiles);

            return (boardWidth, boardHeight);
        }


        private void HandleInput()
        {
            Vector3 worldPos = PlayerInput.GetPointerPosition();
            pointerIndicator.SetPosition(worldPos);

            if (PlayerInput.JustUnpressed()) //Si el usuario acaba de liberar el boton izquierdo 
            {
                if (currentFlow != int.MaxValue)
                {
                    for (int k = 0; k < flows.Count; k++) if (k != currentFlow) flows[k].ApplyProvisionalCut();
                    flows[currentFlow].StopBuldingFlow();
                    if (flows[currentFlow].ChangedInMove() && currentFlow != previousFlow)
                    {
                        numMoves++;
                        previousFlow = currentFlow; //Solo se cambia el flow anterior cuando en el actual se ha producido un cambio.
                    }
                }

                pointerIndicator.Hide();
                currentTile = int.MaxValue;
                currentFlow = int.MaxValue;
                isBuildingFlow = false;
            }

            if (IsPosInBoard(worldPos))
            {
                int newTile = WorldPosToTile(worldPos);
                int newFlow = tiles[newTile].GetColor();

                if (PlayerInput.JustPressed()) //Si el usuario acaba de pulsar el boton izquierdo 
                {
                    if (tiles[newTile].IsActive())
                    {
                        currentTile = newTile;
                        currentFlow = tiles[currentTile].GetColor();
                        //Partimos de un punto del flow. Cortamos hasta ese punto.
                        isBuildingFlow = true;
                        flows[currentFlow].SetTransparentBackground(false);
                        tiles[GetOppositeOrigin(currentFlow, currentTile)].PlayBigCircleAnimation();
                        if (!tiles[newTile].IsOrigin()) //Si NO es origen
                        {
                            int prevPos;
                            flows[currentFlow].CutFlow(newTile, out prevPos);
                            flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(prevPos, newTile, boardWidth));
                        }
                        else
                        {
                            flows[currentFlow].ClearFlow();
                        }
                        flows[currentFlow].StartBuildingFlow(tiles[currentTile], currentTile);
                        pointerIndicator.Show(colors[currentFlow]);
                        pointerIndicator.SetDarkened(false);
                    }
                }

                else if (PlayerInput.IsPressed() && isBuildingFlow) //Si el usuario esta pulsando el boton izquierdo
                {
                    if (tiles[newTile].GetColor() == currentFlow || !tiles[newTile].IsActive()) pointerIndicator.SetDarkened(false);
                    else pointerIndicator.SetDarkened(true);

                    if (newTile != currentTile && flows[currentFlow].AdmitsMove(newTile)) //Nos hemos movido de casilla y el flow actual permite el movimiento
                    {
                        Direction dir = DirectionUtils.DirectionBetweenTiles(newTile, currentTile, boardWidth);

                        if (dir != Direction.None && tiles[newTile].CanBeAccesed(dir)) //Si el movimiento ha sido valido (El movimiento en diagonal es demasiado poderoso)
                        {
                            if (tiles[newTile].IsActive()) //La casilla forma parte de un flujo
                            {
                                int prevPos;
                                if (newFlow != currentFlow) //Nos hemos cruzado con otro flujo
                                {
                                    if (!tiles[newTile].IsOrigin())
                                    {
                                        //Cortar el otro flujo y avanzar en esa direccion
                                        flows[newFlow].ProvisionalCut(newTile, out prevPos);
                                        tiles[prevPos].ClearDirection(DirectionUtils.DirectionBetweenTiles(prevPos, newTile, boardWidth));
                                        flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth));
                                        currentTile = newTile;
                                    }
                                }

                                else //Nos hemos cruzado con el flujo que estamos construyendo
                                {
                                    if (tiles[newTile].IsOrigin()) //Nos tenemos que asegurar que NO es el origen del que estamos construyendo
                                    {
                                        if (!flows[currentFlow].Contains(newTile))
                                        {
                                            flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(currentTile, newTile, boardWidth));
                                            //flows[currentFlow].SetClosed(true);
                                            flows[currentFlow].SetHintMarkerVisibility(true);
                                            currentTile = newTile;
                                        }
                                        else
                                        {
                                            flows[currentFlow].CutFlow(newTile, out prevPos);
                                            for (int k = 0; k < flows.Count; k++) if (k != currentFlow) flows[k].RecalculateCut(flows[currentFlow], newTile);
                                            //flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionFromTile(prevPos, newTile));
                                            currentTile = newTile;
                                        }
                                    }
                                    else
                                    {
                                        flows[currentFlow].CutFlow(newTile, out prevPos);
                                        flows[currentFlow].AddToFlow(tiles[newTile], newTile, DirectionUtils.DirectionBetweenTiles(prevPos, newTile, boardWidth));
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
            else
            {
                pointerIndicator.SetDarkened(true);
            }
        }

        private void GetPercentageFilled()
        {
            numFilledTiles = 0;
            for(int k = 0; k < flows.Count; k++)
            {
                numFilledTiles += flows[k].GetFilledTiles();
            }
        }

        private void ApplyHint(int flowToChange)
        {
            flows[flowToChange].ClearFlow();
            List<int> flowSolution = solution[flowToChange];

            int previousTile = flowSolution[0];
            int currentTile;

            flows[flowToChange].ClearFlow();
            flows[flowToChange].SetTransparentBackground(false);
            flows[flowToChange].StartBuildingFlow(tiles[previousTile], flowSolution[0]);

            int occupyingFlow;
            for (int k = 1; k < flowSolution.Count; k++)
            {
                currentTile = flowSolution[k];

                if (tiles[currentTile].IsActive())
                {
                    occupyingFlow = tiles[currentTile].GetColor();
                    if (occupyingFlow != flowToChange)
                    {
                        flows[occupyingFlow].SetTransparentBackground(false);
                        int prevPos;
                        flows[occupyingFlow].CutFlow(currentTile, out prevPos);
                        tiles[prevPos].ClearDirection(DirectionUtils.DirectionBetweenTiles(prevPos, currentTile, boardWidth));
                        flows[occupyingFlow].SetTransparentBackground(true);
                    }
                }

                flows[flowToChange].AddToFlow(tiles[flowSolution[k]], flowSolution[k], DirectionUtils.DirectionBetweenTiles(previousTile, currentTile, boardWidth));
                previousTile = currentTile;
            }
            flows[flowToChange].SetAsSolvedByHint();
            flows[flowToChange].SetTransparentBackground(true);
            flows[flowToChange].SetHintMarkerVisibility(true);
            flows[flowToChange].PlayEndingAnimation();
        }

        private int GetOppositeOrigin(int flow, int tile)
        {
            List<int> sol = solution[flow];
            if (sol[0] == tile) return sol[sol.Count - 1];
            else return sol[0];
        }

        private bool IsPosInBoard(Vector3 pos)
        {
            if (pos.x >= posIni.x && pos.x < posIni.x + boardWidth * tileSize.x &&
                pos.y <= posIni.y && pos.y > posIni.y - boardHeight * tileSize.y)
                return true;

            return false;
        }

        private void SetWallsOnEmptyTiles(List<int> emptyTiles)
        {
            int auxTile;
            for(int k = 0; k < emptyTiles.Count; k++)
            {
                for(int l = 0; l < (int)Direction.None; l++) { 
                    if(NavigateTile(emptyTiles[k], out auxTile, (Direction)l))
                    {
                        if (!tiles[auxTile].IsEmpty()) tiles[emptyTiles[k]].SetWall((Direction)l, true);
                    }
                }
            }
        }

        private void SurrondBoardByWalls()
        {
            for(int k = 0; k < boardWidth; k++)
            {
                if(!tiles[k].IsEmpty()) tiles[k].SetWall(Direction.Up, true);
                if(!tiles[k + boardWidth * (boardHeight - 1)].IsEmpty()) tiles[k + boardWidth * (boardHeight - 1)].SetWall(Direction.Down, true);
            }
            for (int k = 0; k < boardHeight; k++)
            {
                if(!tiles[k * boardWidth].IsEmpty()) tiles[k * boardWidth].SetWall(Direction.Left, true);
                if(!tiles[(k * boardWidth) + boardWidth - 1].IsEmpty()) tiles[(k * boardWidth) + boardWidth - 1].SetWall(Direction.Right, true);
            }
        }

        private int WorldPosToTile(Vector3 pos)
        {
            Vector2 posBoard = new Vector2(Mathf.Abs(pos.x - posIni.x), Mathf.Abs(pos.y - posIni.y));
            int col = (int)(posBoard.x / tileSize.x);
            int row = (int)(posBoard.y / tileSize.y);

            return (row * boardWidth + col);
        }

        private bool NavigateTile(int tile, out int destination, Direction dir)
        {
            destination = 0;
            if (dir == Direction.Up) destination = tile - boardWidth;
            else if (dir == Direction.Down) destination = tile + boardWidth;
            else if (dir == Direction.Left)
            {
                destination = tile - 1;
                return (tile % boardWidth > 0);
            }
            else if (dir == Direction.Right)
            {
                destination = tile + 1;
                return (tile % boardWidth != boardWidth - 1);
            }
            return (tile >= 0 && tile < (boardWidth * boardHeight));
        }

        public Vector2 GetBoardSize()
        {
            return new Vector2(boardWidth, boardHeight);
        }

        public void UseHint()
        {
            int k = 0;
            while (k < flows.Count && flows[k].IsSolved(solution[k]))
            {
                k++;
            }

            if (k < flows.Count)
            {
                ApplyHint(k);
            }
        }
    }
}