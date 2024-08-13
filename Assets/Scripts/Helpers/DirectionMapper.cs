namespace Helpers
{
    public static class DirectionMapper
    {
        public static int GetDirection(Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Left)
            {
                return -1;
            }
            else if (direction == Direction.Right || direction == Direction.Down)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }
}