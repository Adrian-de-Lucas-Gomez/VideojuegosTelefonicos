using System.Collections;
using System.Collections.Generic;

namespace flow
{
    public class Flow
    {
        private List<Tile> tiles;
        private List<Tile> solution;
        private Tile originA, originB;
        private Tile startingTile;
        private int color = 0;

        bool closed = false;

        public Flow(int c)
        {
            color = c;
            tiles = new List<Tile>();
            solution = new List<Tile>();
        }

        //Construcción del camino

        public void startBuildingFlow(Tile tile)
        {
            startingTile = tile;
            tiles.Add(startingTile);
        }

        public void stopBuldingFlow()
        {

        }

        public void addToFlow(Tile newTile, Direction dir)
        {
            tiles[tiles.Count - 1].SetDirection(dir);
            tiles.Add(newTile);
            newTile.SetDirection(DirectionUtils.GetOppositeDirection(dir));
            if (!newTile.IsActive()) newTile.SetColor(color);
        }

        public void constructSolution(Tile tile)
        {
            solution.Add(tile);
        }

        public void closeFlow()
        {
            closed = true;
        }

        public void clearFlow()
        {
            tiles.Clear();
        }

        //Getters

        public Tile getStartingTile()
        {
            return startingTile;
        }

        public bool isClosed()
        {
            return closed;
        }

        //Setters

        public void setOrigins(Tile origA, Tile origB)
        {
            originA = origA;
            originA.SetAsOrigin(color);
            constructSolution(origA);
            originB = origB;
            originB.SetAsOrigin(color);
            constructSolution(origB);
        }
    }
}