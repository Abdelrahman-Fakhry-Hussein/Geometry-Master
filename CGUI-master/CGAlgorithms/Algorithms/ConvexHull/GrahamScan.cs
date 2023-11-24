using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        private static double RetrieveAngle(Point p1, Point p2)
        {
            double deltaY = p1.Y - p2.Y;
            double deltaX = p1.X - p2.X;
            double angleRad = Math.Atan2(deltaY, deltaX);
            double angleDegrees = angleRad * (180.0 / Math.PI);
            return angleDegrees;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 4)
            {
                foreach (Point point in points)
                {
                    outPoints.Add(point);
                }
                return;
            }
            // Finding lowest point by Y-Coordinate or X-Coordinate if there are equal Y-Coordinate points
            Point lowestPoint = points[0];
            foreach (Point point in points)
            {
                if (point.Y < lowestPoint.Y || (point.Y == lowestPoint.Y && point.X < lowestPoint.X))
                {
                    lowestPoint = point;
                }
            }

            // Remove lowest point from the list and push it to the stack
            points.Remove(lowestPoint);
            Stack<Point> convexHullStack = new Stack<Point>();
            convexHullStack.Push(lowestPoint);

            // Sorting by angle
            points.Sort((point1, point2) => {
                // Comparing angles with respect to the lowest point
                double angle1 = RetrieveAngle(point1, lowestPoint);
                double angle2 = RetrieveAngle(point2, lowestPoint);
                if (angle1 < angle2)
                    return -1;
                else if (angle1 > angle2)
                    return 1;
                // Comparing distances from lowestPoint in case of equal angles
                double dist1 = Math.Pow(point1.X - lowestPoint.X, 2) + Math.Pow(point1.Y - lowestPoint.Y, 2);
                double dist2 = Math.Pow(point2.X - lowestPoint.X, 2) + Math.Pow(point2.Y - lowestPoint.Y, 2);
                return dist1.CompareTo(dist2);
            });


            // Push first point in the stack and remove it from list
            convexHullStack.Push(points[0]);
            Point p1 = lowestPoint;
            Point p2 = points[0];
            Line l = new Line(p1, p2);
            points.RemoveAt(0);
            
            // Looping over the points list and checking their direction to push or pop from the stack 
            foreach(Point point in points)
            {
                while (convexHullStack.Count > 1 && HelperMethods.CheckTurn(l, point) != Enums.TurnType.Left)
                {
                    // Changing start and end points of the line
                    convexHullStack.Pop();
                    Point temp = convexHullStack.Pop();
                    if (convexHullStack.Count != 0)
                    {
                        p1 = convexHullStack.Peek();
                    }
                    convexHullStack.Push(temp);
                    p2 = temp;
                    l.Start = p1;
                    l.End = p2;
                }
                convexHullStack.Push(point);
                p1 = p2;
                p2 = point;
                l.Start = p1;
                l.End = p2;

            }

            // Adding points to the list from the stack
            while (convexHullStack.Count > 0)
            {
                outPoints.Add(convexHullStack.Pop());
            }

        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
