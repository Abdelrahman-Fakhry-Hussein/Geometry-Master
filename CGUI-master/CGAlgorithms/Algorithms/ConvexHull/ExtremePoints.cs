using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            #region Extreme Case
            // Case of 3 or less points
            if (points.Count() < 4)
            {
                foreach(Point p in points)
                {
                    outPoints.Add(p);
                }
                return;
            }
            #endregion
            #region General Algorithm
            // Iterating over each point P
            // Iterating over each other point - a, b, c - to form a triangle to check if the point P is an extreme point or not 
            foreach (Point p in points)
            {
                bool flag = false;
                foreach (Point a in points)
                {
                    if (flag)
                        break;
                    if (a.Equals(p))
                        continue;
                    foreach (Point b in points)
                    {
                        if (flag)
                            break;
                        if (b.Equals(a) || b.Equals(p))
                            continue;
                        foreach (Point c in points)
                        {
                            if (flag)
                                break;
                            if (c.Equals(a) || c.Equals(b) || c.Equals(p))
                                continue;
                            if (HelperMethods.PointInTriangle(p, a, b, c) != Enums.PointInPolygon.Outside)
                            {
                                flag = true;
                            }
                        }
                    }
                }
                if (!flag)
                {
                    outPoints.Add(p);
                    Console.WriteLine("Point added: ({0}, {1})", p.X, p.Y);
                }
            }
            #endregion
            #region Removing same points
            // Removing identical points
            for (int i = 0; i < outPoints.Count; i++)
            {
                for (int j = i + 1; j < outPoints.Count; j++)
                {
                    if (outPoints[i].X == outPoints[j].X && outPoints[i].Y == outPoints[j].Y)
                    {
                        outPoints.RemoveAt(j);
                    }
                }
            }
        }
        #endregion
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
