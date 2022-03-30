using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KnightTourChallenge
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Width and height of chess board: ");
            int width, height;
            width = height = int.Parse(Console.ReadLine());
            Console.Write("Starting square (format: x,y ; zero-based): ");
            string[] coordinateParts = Console.ReadLine().Split(new char[] { ',' }, 2);
            Coordinate startingSquare = new Coordinate(int.Parse(coordinateParts[0]), int.Parse(coordinateParts[1]));
            Console.Write("Output image? (gif, final, none): ");
            string outputImage = Console.ReadLine();
            string outputImageFilePath = null;
            int imageWidth = -1;
            int imageHeight = -1;
            if (outputImage == "gif" || outputImage == "final")
            {
                Console.Write("Output image file path? ");
                outputImageFilePath = Console.ReadLine();
                if (File.Exists(outputImageFilePath))
                {
                    Console.WriteLine("WARNING! That file already exists, so this application will overwrite it. Quit if you don't want to do this.");
                }
                Console.Write("Output image width? ");
                imageWidth = int.Parse(Console.ReadLine());
                Console.Write("Output image height? ");
                imageHeight = int.Parse(Console.ReadLine());
            }
            KnightBoard board = new KnightBoard(width, height, startingSquare);
            if (!board.TourExists())
            {
                Console.WriteLine("There is no tour for this board.");
                return;
            }
            if (!board.MakeTour())
            {
                Console.WriteLine(string.Join(" ", board.Path.Select(x => x.ToString())));
                Console.WriteLine("I'm stuck :(");
                return;
            }
            Console.WriteLine(string.Join(" ", board.Path.Select(x => x.ToString())));
            if (outputImage == "gif")
            {
                Console.WriteLine("Generating GIF...");
                try
                {
                    BoardDrawing.CreateGif(board.Path, imageWidth, imageHeight, width, height, outputImageFilePath);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("An ArgumentException occured. It looks like this happens when the GIF generation takes too much memory. Sorry!");
                }
                Process.Start(outputImageFilePath);
            }
            else if (outputImage == "final")
            {
                Console.WriteLine("Generating image...");
                BoardDrawing.Draw(board.Path, imageWidth, imageHeight, width, height, outputImageFilePath);
                Process.Start(outputImageFilePath);
            }
        }
    }
}
