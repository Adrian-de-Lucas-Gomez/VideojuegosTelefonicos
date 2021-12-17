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
        private bool isComlpetedInProvisionalCut = true;
        private bool wasSolvedByHint = false;

        public Flow(int c, Color rc, int boardWidth)
        {
            _color = c;
            _renderColor = rc;
            _boardWidth = boardWidth;
            tiles = new List<TileInfo>();
        }

        //Construcci�n del camino

        public void StartBuildingFlow(Tile tile, int pos)
        {
            if (tile.IsOrigin())
            {
                if (tiles.Count >= 1) //El camino ya est� empezado, hay que limpiarlo todo
                {
                    ClearFlow();
                }
                tiles.Add(new TileInfo(tile, pos));
            }
            else
            {
                SetTransparentBackground(false);
                CloseSmallCircle();
            }
        }

        public void CloseSmallCircle()
        {
            tiles[tiles.Count - 1].tile.EnableSmallCircle(false);
        }

        public void StopBuldingFlow()
        {
            SetTransparentBackground(true);
            if(!tiles[tiles.Count - 1].tile.IsOrigin()) tiles[tiles.Count - 1].tile.EnableSmallCircle(true);
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
            }

            TileInfo auxTile = tiles[tiles.Count - 1];
            if (closed)
            {
                SetClosed(false);
                SetHintMarkerVisibility(false);
                //Buscamos la posicion de corte
                int aux = 0;
                while(auxTile.position != tilePos)
                {
                    auxTile = tiles[aux];
                    aux++;
                }

                //El camino m�s largo es de Origen -> Punto a cortar
                if (aux <= ((tiles.Count - 1) / 2))
                {
                    tiles.Reverse();
                }

                //Si no, el camino m�s largo es de Fin -> Punto a cortar

                auxTile = tiles[tiles.Count - 1];
            }

            int previousTile = auxTile.position;
            while (auxTile.position != tilePos)
            {
                auxTile.tile.ResetData();
                previousTile = auxTile.position;
                tiles.RemoveAt(tiles.Count - 1);
                auxTile = tiles[tiles.Count - 1];
            }
            auxTile.tile.ResetData();
            //auxTile.tile.ClearDirection(DirectionUtils.DirectionBetweenTiles(auxTile.position, previousTile, _boardWidth));
            if(tiles.Count > 1) tiles.RemoveAt(tiles.Count - 1);
            prevTile = tiles[tiles.Count - 1].position;
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
                if(k < ((tiles.Count - 1) / 2) && isComlpetedInProvisionalCut)
                {
                    tiles.Reverse();
                    k = tiles.Count - k - 1;
                }
                SetHintMarkerVisibility(false);
                isComlpetedInProvisionalCut = false;
            }

            //Nos guardamos el resto de tiles en el flujo.
            //Al resto les quitamos los dibujitos.
            for(int l = k; l < tiles.Count; l++)
            {
                if (tiles[l].tile.GetColor() == _color) tiles[l].tile.ResetData();
            }

            provisionalCutPosition = k;

            prevTile = tiles[k - 1].position;
            //return tiles[k - 1].position;
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
                    provisionalCutPosition++;
                    finished = (provisionalCutPosition >= tiles.Count || other.Contains(tiles[provisionalCutPosition].position));
                }
                if (closed && tiles[0].tile.GetColor() == _color)
                {
                    isComlpetedInProvisionalCut = true;
                    SetHintMarkerVisibility(true);
                }
            }
        }

        public void ApplyProvisionalCut()
        {
            if(provisionalCutPosition > 0)
            {
                isComlpetedInProvisionalCut = true;
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

            //Busca por la soluci�n y compara con la lista de tiles hasta que la recorre entera
            //o se encuentra un tile que es diferente.

            //La soluci�n y la lista de tiles siguen el mismo orden
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
            if (closed) SetHintMarkerVisibility(false);
            for(int k = 0; k < tiles.Count; k++)
            {
                tiles[k].tile.HideTransparentBackground();
                tiles[k].tile.ResetData();
            }
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