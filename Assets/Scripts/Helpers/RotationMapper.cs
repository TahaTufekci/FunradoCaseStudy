using UnityEngine;

public static class RotationMapper
{
    public static Quaternion GetRotation(Direction direction)
    {
        if (direction == Direction.Up)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        else if (direction == Direction.Right)
        {
            return Quaternion.Euler(0, 270, 0);
        }
        else if ( direction == Direction.Down)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Direction.Left)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else
        {
            return Quaternion.Euler(0, 180, 0);
        }

    }
}
