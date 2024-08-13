namespace Models
{
    public class UIModel
    {
        public int CurrentMoveCount { get; private set; }

        public void SetCurrentMoveCount(int count)
        {
            CurrentMoveCount = count;
        }
    }
}
