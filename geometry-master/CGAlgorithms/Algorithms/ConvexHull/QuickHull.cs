using CGUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public static double DistanceToLine(Point point, Line line)
        {
            double x1 = line.Start.X, y1 = line.Start.Y;
            double x2 = line.End.X, y2 = line.End.Y;
            double x = point.X, y = point.Y;

            double distance = Math.Abs((x2 - x1) * (y1 - y) - (x1 - x) * (y2 - y1))
                            / Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return distance;
        }

        public static Point FindMaxDistancePoint(List<Point> points, Line line)
        {
            Point maxPoint = null;
            double maxDistance = double.MinValue;

            foreach (Point point in points)
            {
                double distance = DistanceToLine(point, line);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxPoint = point;
                }
            }

            return maxPoint;
        }
        public  List<Point> Get_points(List<Point> points, Point left, Point right)
        {
            List<Point> result = new List<Point>();
            List<Point> tempss = new List<Point>();
            Line L = new Line((Point)left, (Point)right);
            for (int i = 0;i< points.Count;i++)
            {
                if (CGUtilities.HelperMethods.CheckTurn(L, points[i]) == Enums.TurnType.Left)
                {
                    tempss.Add(points[i]);
                    
                }
            }
            Point NW_Max = FindMaxDistancePoint(tempss, L);
            result.Add(NW_Max);

            result = result.Concat(Get_points(points, left, NW_Max)).ToList();
            result = result.Concat(Get_points(points, NW_Max, right)).ToList();
            return result;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            double minx = double.MaxValue, miny = double.MaxValue, maxx = double.MinValue, maxy = double.MinValue;
            Point min_x = null, min_y = null, max_x = null, max_y = null;
            for(int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minx)
                {
                    minx = points[i].X;
                    min_x = points[i];
                }
                if (points[i].Y < miny)
                {
                    miny = points[i].Y;
                    min_y = points[i];
                }
                if (points[i].X > maxx) { maxx = points[i].X; max_x = points[i]; }
                if (points[i].Y > maxy) { maxy = points[i].Y; max_y = points[i]; }
            }
            Line NW = new Line((Point)min_x, (Point)max_y);
            Line NE = new Line((Point)max_y, (Point)max_x);
            Line SE = new Line((Point)max_x, (Point)min_y);
            Line SW = new Line((Point)min_y, (Point)min_x);
            List<Point> p = points.Select(pa => new Point(pa.X, pa.Y)).ToList();
            p.Remove(min_y); p.Remove(max_y);
            p.Remove(min_x);p.Remove(max_x);
            List<Point> NW_points = new List<Point>(), NE_points = new List<Point>(), SE_points = new List<Point>(), SW_points = new List<Point>();
            for (int i = 0;i < p.Count;i++)
            {
                bool  temp1 = false, temp2 = false, temp3 = false, temp4 = false;
                
                if(CGUtilities.HelperMethods.CheckTurn(NE, p[i]) == Enums.TurnType.Left)
                {
                    NE_points.Add(p[i]);
                }
                else if (CGUtilities.HelperMethods.CheckTurn(NW, p[i]) == Enums.TurnType.Left)
                {
                    NW_points.Add(p[i]);
                }

                else if (CGUtilities.HelperMethods.CheckTurn(SE, p[i]) == Enums.TurnType.Left)
                {
                    SE_points.Add(p[i]);
                }

                else if (CGUtilities.HelperMethods.CheckTurn(SW, p[i]) == Enums.TurnType.Left)
                {
                    SW_points.Add(p[i]);
                }
                else
                {
                    p.Remove(p[i]);
                }
            }
            List<Point> outputss = new List<Point>();
            outputss.Add(min_y); outputss.Add(max_y);   outputss.Add(min_x);    outputss.Add(max_x);
            Point NE_Max = FindMaxDistancePoint(NE_points, NE);
            if(NE_Max != null)
            {
                outputss.Add(NE_Max);
                outputss = outputss.Concat( Get_points(p, max_y, NE_Max)).ToList();
                outputss = outputss.Concat(Get_points(p, NE_Max, max_x)).ToList();
            }
            Point NW_Max = FindMaxDistancePoint(NW_points, NW);
            if (NW_Max != null)
            {
                outputss.Add(NW_Max);
                outputss = outputss.Concat(Get_points(p, min_x, NW_Max)).ToList();
                outputss = outputss.Concat(Get_points(p, NW_Max, min_y)).ToList();
            }
            Point SE_Max = FindMaxDistancePoint(SE_points, SE);
            if (SE_Max != null)
            {
                outputss.Add(SE_Max);
                outputss = outputss.Concat(Get_points(p, min_x, SE_Max)).ToList();
                outputss = outputss.Concat(Get_points(p, SE_Max, min_y)).ToList();
            }
           
            Point SW_Max =  FindMaxDistancePoint(SW_points, SW);
            if (SW_Max != null)
            {
                outputss.Add(SW_Max);
                outputss = outputss.Concat(Get_points(p, min_y, SW_Max)).ToList();
                outputss = outputss.Concat(Get_points(p, SW_Max, min_x)).ToList();
            }

            outPoints = outputss.Distinct().ToList();
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
