using ClassLibrary.PointsModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GeoModel.Utils
{
    internal class FileReader
    {
        private readonly char[] separator;
        private readonly NumberFormatInfo numberFormat;

        public FileReader()
        {
            numberFormat = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };
            separator = new[] { ' ', ',', ';', '\t' };
        }

        public List<GeoTreeNode> Read(string filename)
        {
            var slices = new List<GeoTreeNode>();

            using (var streamReader = File.OpenText(filename))
            {
                //Read first line - string names of coords (e.g. x, y, z1, rho1, z2, rho2, ...)
                streamReader.ReadLine();

                //Read second line - 2 integer values: num of triangles, num of slices
                var line = streamReader.ReadLine();
                var words = line?.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                //num of lines = num of triangles*3
                var numOfLinesInSlice = int.Parse(words?[0] ?? "") * 3;
                var numOfSlices = int.Parse(words?[1] ?? "");

                //parse slices
                for (var i = 0; i < numOfSlices; i++)
                {
                    slices.Add(FillSlice(numOfLinesInSlice, streamReader));
                }
            }
            return slices;
        }

        public GeoTreeNode ReadVzPoints(string filename)
        {
            var vzPoints = new GeoTreeNode
            {
                Name = filename
            };
            using (var streamReader = File.OpenText(filename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var words = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length != 2) break;
                    var args = new float[words.Length];
                    for (var i = 0; i < words.Length; i++)
                    {
                        args[i] = float.Parse(words[i], numberFormat);
                    }
                    var point = new GeoPoint(args[0], args[1], 0, 0);
                    vzPoints.Add(point);
                }

            }
            return vzPoints;
        }

        private GeoTreeNode FillSlice(int numOfLinesInSlice, StreamReader streamReader)
        {
            //Read slice name
            var line = streamReader?.ReadLine()?.Trim();
            var ps = new GeoTreeNode
            {
                Name = line
            };

            for (var k = 0; k < numOfLinesInSlice; k++)
            {
                line = streamReader?.ReadLine();
                var words = line?.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                var args = new float[words?.Length ?? 0];
                for (var i = 0; i < words?.Length; i++)
                {
                    args[i] = float.Parse(words[i], numberFormat);
                }
                AddLine(args, ref ps);
            }
            return ps;
        }

        private void AddLine(float[] args, ref GeoTreeNode node)
        {
            if (args.Length < 4) throw new ArgumentOutOfRangeException(nameof(args));

            var x = args[0];
            var y = args[1];
            var zCounter = 0;

            var l = args.Length - args.Length % 2;
            for (var i = 2; i < l; i += 2)
            {
                while (node.Items.Count <= zCounter)
                {
                    var geoTreeNode = new GeoTreeNode { Name = $"Z[{zCounter}]" };
                    node.Add(geoTreeNode);
                }
                IGeoTreeItem treeItem = new GeoPoint(x, y, args[i], args[i + 1]);
                (node.Items[zCounter] as IGeoTreeNode)?.Add(treeItem);
                zCounter++;
            }
            if (args.Length % 2 == 1)
            {
                while (node.Items.Count <= zCounter)
                {
                    var geoTreeNode = new GeoTreeNode { Name = $"Z[{zCounter}]" };
                    node.Add(geoTreeNode);
                }
                (node.Items[zCounter] as GeoTreeNode)?.Add(new GeoPoint(x, y, float.PositiveInfinity, args.Last()));
            }
        }
    }
}
