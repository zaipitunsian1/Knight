using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KnightTourChallenge
{
    class KnightBoard
    {
        BitArray visited;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Coordinate KnightPosition { get; private set; }
        public List<Coordinate> Path { get; private set; }
        int[][] directions = new int[][]
        {
                new int[] { 1, 2 },
                new int[] { 2, 1 },
                new int[] { -1, -2 },
                new int[] { -2, -1 },
                new int[] { 2, -1 },
                new int[] { -2, 1 },
                new int[] { 1, -2 },
                new int[] { -1, 2 }
        };

        public KnightBoard(int width, int height, Coordinate knightPos)
        {
            visited = new BitArray(width * height);
            Width = width;
            Height = height;
            visited[ArrayPositionFromCoordinate(knightPos)] = true;
            Path = new List<Coordinate>();
            Path.Add(knightPos);
            KnightPosition = knightPos;
        }

        int ArrayPositionFromCoordinate(Coordinate pos)
        {
            if (pos.X >= Width || pos.Y >= Height || pos.X < 0 || pos.Y < 0) throw new ArgumentException();

            return (pos.Y * Height) + pos.X;
        }

        public bool TourExists()
        {
            int m = Math.Min(Width, Height);
            int n = Math.Max(Width, Height);

            if (m >= 5) return true; // a tour exists, and it's possibly an open one

            // Otherwise, check that there is a closed tour.
            if (m % 2 == 1 && n % 2 == 1)
                return false;
            if (m == 1 || m == 2 || m == 4)
                return false;
            if (m == 3 && (n == 4 || n == 6 || n == 8))
                return false;
            // if any of the three conditions is true, a closed tour is impossible.

            return true;
        }

        List<Coordinate> GetValidDestinations(Coordinate origin)
        {
            List<Coordinate> result = new List<Coordinate>();
            foreach (int[] dir in directions)
            {
                int newX = origin.X + dir[0];
                int newY = origin.Y + dir[1];
                if (newX < 0 || newY < 0 || newX >= Width || newY >= Height)
                {
                    continue;
                }
                Coordinate newCo = new Coordinate(newX, newY);
                if (visited[ArrayPositionFromCoordinate(newCo)] || newCo.Equals(origin))
                {
                    continue;
                }
                result.Add(newCo);
            }
            return result;
        }

        Coordinate FarthestFromCenter(List<Coordinate> options)
        {
            double centerX = (Width - 1) / 2.0;
            double centerY = (Height - 1) / 2.0;
            Dictionary<Coordinate, double> coordinatesWithDistanceSquared = options.ToDictionary(x => x, c => Math.Pow(centerX - c.X, 2) + Math.Pow(centerY - c.Y, 2));
            return coordinatesWithDistanceSquared.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        public bool MakeTour()
        {
            while (Path.Count < Width * Height)
            {
                Dictionary<Coordinate, int> weightedCoordinates = new Dictionary<Coordinate, int>();

                Coordinate chosen = null;

                foreach (Coordinate co in GetValidDestinations(KnightPosition))
                {
                    int optionsFromNewDestination = GetValidDestinations(co).Count;
                    weightedCoordinates.Add(co, optionsFromNewDestination);
                }

                if (weightedCoordinates.Count == 0)
                {
                    return false;
                }

                int min = weightedCoordinates.Min(x => x.Value);
                List<Coordinate> allMin = weightedCoordinates.Where(x => x.Value == min).Select(x => x.Key).ToList();
                if (allMin.Count == 1)
                {
                    chosen = allMin[0];
                }
                else
                {
                    chosen = FarthestFromCenter(allMin);
                }
                Path.Add(chosen);
                visited[ArrayPositionFromCoordinate(chosen)] = true;
                KnightPosition = chosen;
            }
            return true;
        }
    }
}
