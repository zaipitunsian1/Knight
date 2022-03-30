using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace KnightTourChallenge
{
    class BoardDrawing
    {
        public static void Draw(List<Coordinate> path, int width, int height, int boardWidth, int boardHeight, string file)
        {
            using (Bitmap boardBitmap = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(boardBitmap))
                {
                    g.Clear(Color.White);

                    float lineWidth = 1f;

                    int verticalLineCount = boardWidth - 1;
                    float verticalLineDistance = (width - 2 * lineWidth) / boardWidth;

                    int horizontalLineCount = boardHeight - 1;
                    float horizontalLineDistance = (height - 2 * lineWidth) / boardHeight;

                    g.DrawRectangle(new Pen(Color.Black, 1), 0, 0, width - lineWidth, height - lineWidth);

                    for (int i = 1; i <= verticalLineCount; i++)
                    {
                        g.DrawLine(new Pen(Color.Black, lineWidth), new PointF(i * verticalLineDistance, 0), new PointF(i * verticalLineDistance, height - lineWidth));
                    }

                    for (int i = 1; i <= horizontalLineCount; i++)
                    {
                        g.DrawLine(new Pen(Color.Black, lineWidth), new PointF(0, i * horizontalLineDistance), new PointF(width - lineWidth, i * horizontalLineDistance));
                    }

                    PointF[] linePoints = new PointF[path.Count];
                    for (int i = 0; i < linePoints.Length; i++)
                    {
                        float x = verticalLineDistance * path[i].X + verticalLineDistance / 2;
                        float y = horizontalLineDistance * path[i].Y + horizontalLineDistance / 2;
                        linePoints[i] = new PointF(x, y);

                        if (i == 0)
                        {
                            float ellipseWidth = verticalLineDistance / 3;
                            float ellipseHeight = horizontalLineDistance / 3;
                            g.FillEllipse(Brushes.Blue, x - (ellipseWidth / 2), y - (ellipseHeight / 2), ellipseWidth, ellipseHeight);
                        }
                    }
                    if (linePoints.Length >= 2)
                    {
                        g.DrawLines(new Pen(Color.Blue, 2), linePoints);
                    }
                }
                boardBitmap.Save(file);
            }
        }

        public static void CreateGif(List<Coordinate> path, int width, int height, int boardWidth, int boardHeight, string file)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "KnightsTour", "temp-" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                for (int i = 0; i < path.Count; i++)
                {
                    string filename = Path.Combine(tempDir, i + ".gif");
                    Draw(path.Take(i + 1).ToList(), width, height, boardWidth, boardHeight, filename);
                    collection.Add(filename);
                    collection[i].AnimationDelay = i != path.Count - 1 ? 50 : 300; // 3 seconds if last frame, otherwise 0.5
                }

                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 256;
                collection.Quantize(settings);

                collection.Optimize();
                collection.Write(file);
            }

            Directory.Delete(tempDir, true);
        }
    }
}
