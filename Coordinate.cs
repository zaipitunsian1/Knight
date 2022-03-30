using System;

namespace KnightTourChallenge
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Coordinate))
            {
                return false;
            }
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            return other != null && this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return new { X = X, Y = Y }.GetHashCode();
        }
    }
}
