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
            TileInfo searchTile = tiles[tiles.Count - 1];
            searchTile.tile.ResetData();
            while(searchTile.position != tilePos)
            {
                searchTile.tile.ResetData();
                tiles.RemoveAt(tiles.Count - 1);
                searchTile = tiles[tiles.Count - 1];
            }
            searchTile.tile.ResetData();
            tiles.RemoveAt(tiles.Count - 1);

            return tiles[tiles.Count - 1].position;
        }

        public void clearFlow()
        {
            tiles.Clear();
        }

        //Getters

        public Tile getStartingTile()
        {
            return startingTile.tile;
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