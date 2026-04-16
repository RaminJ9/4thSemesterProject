namespace Core.Utils
{
    public class Cursor
    {
        private int max;
        public int Position { get; private set; }
        public Cursor(int maxIndex, int start =  0) 
        {
            max = maxIndex;
        }

        public int Move(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                if (Position == max)
                {
                    Position = 0;
                    continue;
                }

                Position++;
            }

            return Position;
        }
    }
}
