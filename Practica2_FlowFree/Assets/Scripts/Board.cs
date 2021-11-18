using System.Collections;
using System.Collections.Generic;

namespace flow.logic
{
    public class Board
    {
        private int w = 3, h = 3, nColors= 2;

        private List<List<Tile>> map;

        public Board() {
            map = new List<List<Tile>>();
            map.Add(new List<Tile>()); map[0].Add(new Tile(0)); map[0].Add(new Tile(0)); map[0].Add(new Tile(2));
            map.Add(new List<Tile>()); map[1].Add(new Tile(1)); map[1].Add(new Tile(1)); map[1].Add(new Tile(0));
            map.Add(new List<Tile>()); map[2].Add(new Tile(2)); map[2].Add(new Tile(0)); map[2].Add(new Tile(0));
        }

        private class Tile
        {
            public Tile(int c)
            {
                color = c;
                isOrigin = (c != 0);
            }

            public int color = 0;
            public bool isOrigin = false;
        }
    }
}
