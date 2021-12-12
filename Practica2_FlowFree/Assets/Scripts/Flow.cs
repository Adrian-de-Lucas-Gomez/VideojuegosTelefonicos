using System.Collections;
using System.Collections.Generic;
using UnityEngine; //SOLO PARA DEBUGEAR PORFA QUITAME CUANDO ACABEMOS ESTO

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

        private List<TileInfo> tiles;
        private List<TileInfo> solution;
        private TileInfo originAInfo, originBInfo;
        private TileInfo startingTile;
        private int _color = 0;

        bool closed = false;

        public Flow(int c)
        {
            _color = c;
            tiles = new List<TileInfo>();
            solution = new List<TileInfo>();
        }

        //Construcción del camino

        public void startBuildingFlow(Tile tile, int pos)
        {
            closed = false;
            if(tiles.Count > 0) //El camino ya está empezado, hay que limpiarlo todo
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

        public void stopBuldingFlow()
        {

        }

        public void addToFlow(Tile newTile, int pos, Direction dir)
        {
            tiles[tiles.Count - 1].tile.SetDirection(dir);
            tiles.Add(new TileInfo(newTile, pos));
            newTile.SetDirection(DirectionUtils.GetOppositeDirection(dir));
            if (!newTile.IsActive()) newTile.SetColor(_color);
        }

        public void constructSolution(Tile tile, int pos)
        {
            solution.Add(new TileInfo(tile, pos));
        }

        private void constructSolution(TileInfo tileInfo)
        {
            solution.Add(tileInfo);
        }

        public void closeFlow()
        {
            closed = true;
        }

        public int cutFlow(int tilePos)
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

                //El camino más largo es de Origen -> Punto a cortar
                if (aux < (tiles.Count - 1 )/ 2)
                {
                    startingTile = tiles[tiles.Count - 1];
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
            tiles.RemoveAt(tiles.Count - 1);

            return tiles[tiles.Count - 1].position;
            
        }

        public void clearFlow()
        {
            tiles.Clear();
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
            constructSolution(originAInfo);
            originBInfo.tile = origB;
            originBInfo.position = posOrigB;
            originBInfo.tile.SetAsOrigin(_color);
            constructSolution(originBInfo);
        }
    }
}