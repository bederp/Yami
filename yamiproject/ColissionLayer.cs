using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace yamiproject
{
    class ColissionLayer
    {
        int width;
        int height;
        int [,] map;

        public ColissionLayer(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new int[height, width];
        }

        public static ColissionLayer FromFile(string filename)
        {
            ColissionLayer colission;
            List<List<int>> tempcolision = new List<List<int>>();

            using (StreamReader reader = new StreamReader("Content/"+filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim() ;

                    List<int> row = new List<int>();
                    string[] cells = line.Split(' ');

                    foreach (string c in cells)
                    {
                        row.Add(int.Parse(c));
                    }

                    tempcolision.Add(row);
                }
            }

            int width = tempcolision[0].Count;
            int height = tempcolision.Count;

            colission = new ColissionLayer(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colission.SetCellIndex(x, y, tempcolision[y][x]);
                }
            }

            return colission;
        }

        public void SetCellIndex(int x, int y, int value)
        {
            map[y, x] = value;
        }

        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }
    }
}
