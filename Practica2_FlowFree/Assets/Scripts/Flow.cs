using System.Collections;
using System.Collections.Generic;
using UnityEngine; //SOLO PARA DEBUGEAR PORFA QUITAME CUANDO ACABEMOS TODO ESTO :)

namespace flow
{
    public class Flow
    {
        private struct TileInfo
        {
            public Tile tile;
            public int position;
            public TileInfo(Tile t, int pos) { tile = t; position = pos; }
        }

        private int provisionalCutPosition = -1;

        private List<TileInfo> tiles;
        private TileInfo originAInfo, originBInfo;
        private int _color = 0;
        private int _boardWidth;
        private Color _renderColor;

        private bool closed = false;
        private bool closedInProvisionalCut = false;
        private bool wasSolvedByHint = false;

        private int initialPosition = 0;
        private bool hasChanged = false;

        public Flow(int c, Color rc, int boardWidth)
        {
            _color = c;
            _renderColor = rc;
            _boardWidth = boardWidth;
            tiles = new List<TileInfo>();
        }

        //Construcción del camino

        public void StartBuildingFlow(Tile tile, int pos)
        {
            if (tiles.Count == 0) tiles.Add(new TileInfo(tile, pos));
            initialPosition = pos;
            hasChanged = false;
            CloseSmallCircle();
        }

        public void CloseSmallCircle()
        {
            tiles[tiles.Count - 1].tile.EnableSmallCircle(false);
        }

        public void StopBuldingFlow()
        {
            SetTransparentBackground(true);
            hasChanged = (initialPosition != tiles[tiles.Count - 1].position);
            if(!tiles[tiles.Count - 1].tile.IsOrigin()) tiles[tiles.Count - 1].tile.EnableSmallCircle(true);
        }

        public bool ChangedInMove()
        {
            return hasChanged;
        }

        public void AddToFlow(Tile newTile, int pos, Direction dir)
        {
            tiles[tiles.Count - 1].tile.SetDirection(dir);
            tiles.Add(new TileInfo(newTile, pos));
            newTile.SetDirection(DirectionUtils.GetOppositeDirection(dir));
            newTile.SetColor(_color);
            newTile.SetTempColor(_renderColor);
        }

        public void SetClosed(bool state)
        {
            closed = state;
            closedInProvisionalCut = state;
        }

        public void SetHintMarkerVisibility(bool visibility)
        {
            if (wasSolvedByHint)
            {
                tiles[0].tile.setHintMarker(visibility);
                tiles[tiles.Count - 1].tile.setHintMarker(visibility);
            }
        }

        public void CutFlow(int tilePos, out int prevTile)
        {
            if(tilePos == tiles[0].position)
            {
                for (int k = 0; k < tiles.Count; k++)
                {
                    tiles[k].tile.ResetData();
                }
                tiles.RemoveRange(1, tiles.Count - 1);
                prevTile = tiles[0].position;
            }

            else
            {
                TileInfo auxTile = tiles[tiles.Count - 1];
                if (closed)
                {
                    SetClosed(false);
                    SetHintMarkerVisibility(false);
                    //Buscamos la posicion de corte
                    int aux = 0;
                    while (auxTile.position != tilePos)
                    {
                        auxTile = tiles[aux];
                        aux++;
                    }

                    //El camino más largo es de Origen -> Punto a cortar
                    if (aux <= ((tiles.Count - 1) / 2))
                    {
                        tiles.Reverse();
                    }

                    //Si no, el camino más largo es de Fin -> Punto a cortar

                    auxTile = tiles[tiles.Count - 1];
                }

                while (auxTile.position != tilePos)
                {
                    auxTile.tile.ResetData();
                    tiles.RemoveAt(tiles.Count - 1);
                    auxTile = tiles[tiles.Count - 1];
                }
                auxTile.tile.ResetData();
                if (tiles.Count > 1)
                {
                    tiles.RemoveAt(tiles.Count - 1);
                }
                prevTile = tiles[tiles.Count - 1].position;
            }
        }

        public void ProvisionalCut(int position, out int prevTile)
        {
            TileInfo searchTile = tiles[0];
            int k = 0;
            //Buscamos el tile que corta el flujo
            while (searchTile.position != position)
            {
                k++;
                searchTile = tiles[k];
            }

            if (closed)
            {
                if(k < ((tiles.Count - 1) / 2) && closedInProvisionalCut)
                {
                    tiles.Reverse();
                    k = tiles.Count - k - 1;
                }
                SetHintMarkerVisibility(false);
                closedInProvisionalCut = false;
            }
            //Nos guardamos el resto de tiles en el flujo.
            //Al resto les quitamos los dibujitos.
            for(int l = k; l < tiles.Count; l++)
            {
                if (tiles[l].tile.GetColor() == _color) tiles[l].tile.ResetData();
            }
            provisionalCutPosition = k;
            prevTile = tiles[k - 1].position;
        }

        public void RecalculateCut(Flow other, int position)
        {
            if (provisionalCutPosition > -1 && provisionalCutPosition < tiles.Count)
            {
                bool finished = other.Contains(tiles[provisionalCutPosition].position);
                while (!finished)
                {
                    Direction dir = DirectionUtils.DirectionBetweenTiles(tiles[provisionalCutPosition - 1].position, tiles[provisionalCutPosition].position, _boardWidth);
                    tiles[provisionalCutPosition - 1].tile.SetDirection(dir);
                    tiles[provisionalCutPosition].tile.SetDirection(DirectionUtils.GetOppositeDirection(dir));
                    tiles[provisionalCutPosition].tile.SetColor(_color);
                    tiles[provisionalCutPosition].tile.SetTempColor(_renderColor);
                    tiles[provisionalCutPosition].tile.ShowTransparentBackground(_renderColor);
                    provisionalCutPosition++;
                    finished = (provisionalCutPosition >= tiles.Count || other.Contains(tiles[provisionalCutPosition].position));
                }
                if (closed && tiles[0].tile.GetColor() == _color)
                {
                    closedInProvisionalCut = true;
                    SetHintMarkerVisibility(true);
                }
            }
        }

        public void ApplyProvisionalCut()
        {
            if(provisionalCutPosition > 0)
            {
                closedInProvisionalCut = true;
                if (provisionalCutPosition < tiles.Count)
                {
                    SetClosed(false);
                    for (int k = provisionalCutPosition; k < tiles.Count; k++) tiles[k].tile.HideTransparentBackground();
                    tiles.RemoveRange(provisionalCutPosition, tiles.Count - provisionalCutPosition);
                    if (tiles.Count == 1) tiles[0].tile.HideTransparentBackground();
                }
                provisionalCutPosition = -1;
            }
        }

        public bool IsSolved(List<int> solution)
        {
            if (!closed) return false;
            bool isSolved = true;

            //Busca por la solución y compara con la lista de tiles hasta que la recorre entera
            //o se encuentra un tile que es diferente.

            //La solución y la lista de tiles siguen el mismo orden
            if (solution[0] == tiles[0].position) 
            {
                for (int k = 0; k < tiles.Count - 1 && isSolved; k++)
                {
                    isSolved = solution[k] == tiles[k].position;
                }
            }

            //Hay que recorrerla en sentido contrario
            else if (solution[0] == tiles[tiles.Count - 1].position)
            {
                int l = 0;
                for (int k = tiles.Count - 1; k > 0 && isSolved; k--, l++)
                {
                    isSolved = solution[k] == tiles[l].position;
                }
            }

            return isSolved;
        }

        public int GetFilledTiles()
        {
            if(provisionalCutPosition > -1) //Si está siendo cortado por otro flow pero no se ha aplicado aún
            {
                if (closedInProvisionalCut) return tiles.Count - 2; //No cuentan los extremos
                else return provisionalCutPosition - 1;
            }
            if (closed) return tiles.Count - 2;
            if (tiles.Count > 1) return tiles.Count - 1;
            return 0;
        }

        public void SetAsSolvedByHint()
        {
            wasSolvedByHint = true;
        }

        public void SetTransparentBackground(bool enable)
        {
            if (enable)
            {
                for (int k = 0; k < tiles.Count; k++)
                {
                    tiles[k].tile.ShowTransparentBackground(_renderColor);
                }
                if (tiles.Count == 1) tiles[0].tile.HideTransparentBackground();
            }
            else for (int k = 0; k < tiles.Count; k++) tiles[k].tile.HideTransparentBackground();
        }

        public void ClearFlow()
        {
            for(int k = 0; k < tiles.Count; k++)
            {
                tiles[k].tile.HideTransparentBackground();
                tiles[k].tile.ResetData();
            }
            int tilesChanged = tiles.Count - 1;
            tiles.Clear();
        }

        public bool Contains(int position)
        {
            for(int k = 0; k < tiles.Count; k++)
            {
                if (tiles[k].position == position) return true;
            }
            return false;
        }

        public void PrintTiles() //DEBUG ONLY PORFA BORRAME AL TERMINAR
        {
            string msg = _color + ": ";
            for (int k = 0; k < tiles.Count; k++)
            {
                msg += tiles[k].position + ", ";
            }
            Debug.Log(msg);
        }

        //Getters

        public bool isClosed()
        {
            return closed;
        }

        //Setters

        public void setOrigins(Tile origA, int posOrigA, Tile origB, int posOrigB)
        {
            originAInfo.tile = origA;
            originAInfo.position = posOrigA;
            originAInfo.tile.SetAsOrigin(_color);
            originBInfo.tile = origB;
            originBInfo.position = posOrigB;
            originBInfo.tile.SetAsOrigin(_color);
        }
    }
}