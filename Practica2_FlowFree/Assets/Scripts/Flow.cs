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
        private List<TileInfo> solution;
        private TileInfo originAInfo, originBInfo;
        private TileInfo startingTile;
        private int _color = 0;
        private int _boardWidth;
        private Color _renderColor;

        bool closed = false;

        public Flow(int c, Color rc, int boardWidth)
        {
            _color = c;
            _renderColor = rc;
            _boardWidth = boardWidth;
            tiles = new List<TileInfo>();
            solution = new List<TileInfo>();
        }

        //Construcci�n del camino

        public void StartBuildingFlow(Tile tile, int pos)
        {
            closed = false;
            if(tiles.Count > 0) //El camino ya est� empezado, hay que limpiarlo todo
            {
                for (int k = 0; k < tiles.Count; k++)
                {
                    tiles[k].tile.ResetData();
                }
                tiles.Clear();
            }
            startingTile.tile = tile;
            startingTile.position = pos;
            tiles.Add(startingTile);
        }

        public void CloseSmallCircle()
        {
            tiles[tiles.Count - 1].tile.EnableSmallCircle(false);
        }

        public void StopBuldingFlow()
        {
            if(!tiles[tiles.Count - 1].tile.IsOrigin()) tiles[tiles.Count - 1].tile.EnableSmallCircle(true);
        }

        public void AddToFlow(Tile newTile, int pos, Direction dir)
        {
            tiles[tiles.Count - 1].tile.SetDirection(dir);
            tiles.Add(new TileInfo(newTile, pos));
            newTile.SetDirection(DirectionUtils.GetOppositeDirection(dir));
            //Aqu� comprobaba antes si no estaba vac�o. Si peta por cualquier cosa probad lo que quit�.
            newTile.SetColor(_color);
            newTile.SetTempColor(_renderColor);
        }

        public void ConstructSolution(Tile tile, int pos)
        {
            solution.Add(new TileInfo(tile, pos));
        }

        private void ConstructSolution(TileInfo tileInfo)
        {
            solution.Add(tileInfo);
        }

        public void CloseFlow()
        {
            closed = true;
        }

        public int CutFlow(int tilePos)
        {
            if(tilePos == startingTile.position)
            {
                for (int k = 0; k < tiles.Count; k++)
                {
                    tiles[k].tile.ResetData();
                }
                tiles.Clear();
                tiles.Add(startingTile);

                return startingTile.position;
            }

            TileInfo auxTile = tiles[tiles.Count - 1];
            if (closed)
            {
                closed = false;
                int aux = 0;
                while(auxTile.position != tilePos)
                {
                    auxTile = tiles[aux];
                    aux++;
                }

                //El camino m�s largo es de Origen -> Punto a cortar
                if (aux < (tiles.Count - 1 )/ 2)
                {
                    startingTile = tiles[tiles.Count - 1];
                    tiles.Reverse();
                }
                //Si no, el camino m�s largo es de Fin -> Punto a cortar

                auxTile = tiles[tiles.Count - 1];
            }
            
            while (auxTile.position != tilePos)
            {
                auxTile.tile.ResetData();
                tiles.RemoveAt(tiles.Count - 1);
                auxTile = tiles[tiles.Count - 1];
            }
            auxTile.tile.ResetData();
            tiles.RemoveAt(tiles.Count - 1);

            return tiles[tiles.Count - 1].position;
            
        }

        public int ProvisionalCut(int position)
        {
            TileInfo searchTile = tiles[0];
            int k = 0;
            //Buscamos el tile que corta el flujo
            while (searchTile.position != position)
            {
                k++;
                searchTile = tiles[k];
            }
            //Nos guardamos el resto de tiles en el flujo.
            //Al resto les quitamos los dibujitos.
            for(int l = k; l < tiles.Count; l++)
            {
                if (tiles[l].tile.GetColor() == _color) tiles[l].tile.ResetData();
            }

            provisionalCutPosition = k;

            return tiles[k - 1].position;
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
            }
        }

        public void ApplyProvisionalCut()
        {
            if(provisionalCutPosition > 0)
            {
                if(provisionalCutPosition < tiles.Count) tiles.RemoveRange(provisionalCutPosition, tiles.Count - provisionalCutPosition);
                provisionalCutPosition = -1;
            }
        }

        public void ClearFlow()
        {
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

        private void PrintTiles() //DEBUG ONLY
        {
            string msg = _color + ": ";
            for (int k = 0; k < tiles.Count; k++)
            {
                msg += tiles[k].position + ", ";
            }
            Debug.Log(msg);
        }

        //Getters

        public int getStartingTile()
        {
            return startingTile.position;
        }

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
            ConstructSolution(originAInfo);
            originBInfo.tile = origB;
            originBInfo.position = posOrigB;
            originBInfo.tile.SetAsOrigin(_color);
            ConstructSolution(originBInfo);
        }
    }
}