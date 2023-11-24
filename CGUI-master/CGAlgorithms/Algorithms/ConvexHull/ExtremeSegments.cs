using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            #region Extreme Case
            // Case of 3 or less points
            if (points.Count < 4)
            {
                foreach (Point point in points)
                {
                    outPoints.Add(point);
                }
                return;
            }
            #endregion

            #region General Algorithm
            List<Point> colinearPoints = new List<Point>();
            // Iterating over the list of points to make a line composed of two points
            foreach (Point point1 in points)
            {
                foreach (Point point2 in points)
                {
                    if (point1.Equals(point2))
                    {
                        continue;
                    }

                    Line l = new Line(point1, point2);
                    // Flag used later to validate if it's an extreme segment
                    bool flag = true;
                    int firstSide = 2;
                    int i = 0;
                    // Set that will help with checking if there's more than one side
                    SortedSet<int> sideChecker = new SortedSet<int>();
                    foreach (Point p in points)
                    {
                        // Ignoring the points that formed the line
                        if (p.Equals(point1) | p.Equals(point2))
                        {
                            continue;
                        }
     
                        int side = (int) HelperMethods.CheckTurn(l, p);
                        sideChecker.Add(side);
                        if (i == 0)
                        {
                            i++;
                            firstSide = side;
                        }
                        Console.WriteLine("Side: {0}", side);
                        if (HelperMethods.PointOnSegment(p, point1, point2))
                        {
                            colinearPoints.Add(p);
                        }
                        Console.WriteLine("Sidechecker size: {0}", sideChecker.Count());
                        if (side != firstSide && side != 2)
                        {
                            Console.WriteLine("Not Extreme Segment: ({0}, {1}) & ({2}, {3})", l.Start.X, l.Start.Y, l.End.X, l.End.Y);
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        //outLines.Add(l);
                        outPoints.Add(l.Start);
                        outPoints.Add(l.End);
                        Console.WriteLine("EXTREME SEGMENT: ({0}, {1}) & ({2}, {3})", l.Start.X, l.Start.Y, l.End.X, l.End.Y);
                    }

                }
            }
            #endregion

            #region Removing same and collinear points
            outPoints = outPoints.Distinct().ToList();
            
            foreach (Point p in colinearPoints)
            {
                outPoints.Remove(p);
            }
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
            #endregion

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
