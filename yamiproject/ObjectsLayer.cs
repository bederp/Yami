using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace yamiproject
{
    class ObjectsLayer
    {
        public int width;
        public int height;
        int [,] map;

        public ObjectsLayer(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new int[height, width];
        }

        public static ObjectsLayer FromFile(string filename)
        {
            ObjectsLayer obj;
            List<List<int>> tempobj = new List<List<int>>();

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

                    tempobj.Add(row);
                }
            }

            int width = tempobj[0].Count;
            int height = tempobj.Count;

            obj = new ObjectsLayer(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    obj.SetCellIndex(x, y, tempobj[y][x]);
                }
            }

            return obj;
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
