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
    }
}