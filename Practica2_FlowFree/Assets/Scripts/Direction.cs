using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    struct DirectionUtils
    {
        static public Direction GetOppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return Direction.None;
            }
        }

        static public Direction DirectionBetweenTiles(int pos1, int pos2, int boardWidth)
        {
            if (pos1 + 1 == pos2 && pos1 / boardWidth == pos2 / boardWidth) return Direction.Right;

            else if (pos1 - 1 == pos2 && pos1 / boardWidth == pos2 / boardWidth) return Direction.Left;

            else if (pos1 - boardWidth == pos2) return Direction.Up;

            else if (pos1 + boardWidth == pos2) return Direction.Down;

            return Direction.None;
        }
    }
}