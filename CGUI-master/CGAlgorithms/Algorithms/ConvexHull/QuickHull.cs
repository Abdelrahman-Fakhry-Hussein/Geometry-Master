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
        public static float DistancefrompointToLine(Point point, Line line)
        {
            float x1 = (float)line.Start.X, y1 = (float)line.Start.Y;
            float x2 = (float)line.End.X, y2 = (float)line.End.Y;
            float x = (float)point.X, y = (float)point.Y;

            float distance = Math.Abs((x2 - x1) * (y1 - y) - (x1 - x) * (y2 - y1))
                            / (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            return distance;
        }

        public static Point FindMaxDistancePointfromline(List<Point> points, Line line)
        {
            Point maxPoint = null;
            float maxDistance = float.MinValue;

            foreach (Point point in points)
            {
                float distance = DistancefrompointToLine(point, line);

                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    maxPoint = point;
                }
            }

            return maxPoint;
        }

        public List<Point> Get_points(List<Point> points, Point left, Point right)
        {

            List<Point> result = new List<Point>();
            List<Point> tempss = new List<Point>();
            Line L = new Line((Point)left, (Point)right);
            for (int i = 0;i< points.Count;i++)
            {
                if (CGUtilities.HelperMethods.CheckTurn(L, points[i]) == Enums.TurnType.Left)
                {
                    Console.WriteLine("(" + points[i].X + "," + points[i].Y + ")#");
                    tempss.Add(points[i]);
                    
                }
            }
           
            
            if (tempss.Count() != 0)
            {
                Point NW_Max = FindMaxDistancePointfromline(tempss, L);
                Console.WriteLine("(" + NW_Max.X + "," + NW_Max.Y + ")+");
                result.Add(NW_Max);
                result = result.Concat(Get_points(points, left, NW_Max)).ToList();
                result = result.Concat(Get_points(points, NW_Max, right)).ToList();
            }
            return result;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count() <= 3)
            {
                outPoints = new List<Point>();
                foreach (Point pasaa in points.Distinct().ToList())
                {
                   // Console.WriteLine("(" + pasaa.X + "," + pasaa.Y + ")");
                    outPoints.Add(pasaa);
                }
               
            }
            else
            {

                double minx = double.MaxValue, miny = double.MaxValue, maxx = double.MinValue, maxy = double.MinValue;
                Point min_x = null, min_y = null, max_x = null, max_y = null;
                for (int i = 0; i < points.Count; i++)
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
                Console.WriteLine("(" + min_x.X + "," + min_x.Y + ").");
                Console.WriteLine("(" + max_x.X + "," + max_x.Y + ").");
                Console.WriteLine("(" + min_y.X + "," + min_y.Y + ").");
                Console.WriteLine("(" + max_y.X + "," + max_y.Y + ").");
                List<Point> p = points.Select(pa => new Point(pa.X, pa.Y)).ToList();
                Console.WriteLine("1");
                p.Remove(min_y);
                Console.WriteLine("2");
                p.Remove(max_y);
                Console.WriteLine("3");
                p.Remove(min_x);
                Console.WriteLine("4");
                p.Remove(max_x);
                Console.WriteLine("5");
                List<Point> NW_points = new List<Point>(), NE_points = new List<Point>(), SE_points = new List<Point>(), SW_points = new List<Point>();
                List<Point> removerd = new List<Point>();
                for (int i = 0; i < p.Count; i++)
                {
                    bool temp1 = false, temp2 = false, temp3 = false, temp4 = false;
                    Enums.TurnType s = CGUtilities.HelperMethods.CheckTurn(NE, p[i]);
                    Console.WriteLine(s.ToString());
                    Enums.TurnType s2 = CGUtilities.HelperMethods.CheckTurn(NW, p[i]);
                    Console.WriteLine(s2.ToString());
                    Enums.TurnType s3 = CGUtilities.HelperMethods.CheckTurn(SE, p[i]);
                    Console.WriteLine(s3.ToString());
                    Enums.TurnType s4 = CGUtilities.HelperMethods.CheckTurn(SW, p[i]);
                    Console.WriteLine(s4.ToString());
                   
                    if (CGUtilities.HelperMethods.CheckTurn(NE, p[i]) == Enums.TurnType.Left)
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
                        removerd.Add(p[i]);
                    }
                }
                Console.WriteLine("NE_points -> " + NE_points.Count());
                Console.WriteLine("NW_points -> " + NW_points.Count());
                Console.WriteLine("SE_points -> " + SE_points.Count());
                Console.WriteLine("SW_points -> " + SW_points.Count());
                Console.WriteLine("p -> " + p.Count());
                List<Point> outputss = new List<Point>();
                outputss.Add(min_y); outputss.Add(max_y); outputss.Add(min_x); outputss.Add(max_x);
              

                if (NE_points.Count() != 0)
                {

                    Point NE_Max = FindMaxDistancePointfromline(NE_points, NE);
                    outputss.Add(NE_Max);
                    Console.WriteLine("(" + NE_Max.X + "," + NE_Max.Y + ").");
                    outputss = outputss.Concat(Get_points(NE_points, max_y, NE_Max)).ToList();
                    outputss = outputss.Concat(Get_points(NE_points, NE_Max, max_x)).ToList();
                }
               
                if (NW_points.Count() != 0)
                {
                    Point NW_Max = FindMaxDistancePointfromline(NW_points, NW);
                    outputss.Add(NW_Max);
                    outputss = outputss.Concat(Get_points(NW_points, min_x, NW_Max)).ToList();
                    outputss = outputss.Concat(Get_points(NW_points, NW_Max, max_y)).ToList();
                }
               
                if (SE_points.Count() != 0)
                {
                    Point SE_Max = FindMaxDistancePointfromline(SE_points, SE);
                    outputss.Add(SE_Max);
                    Console.WriteLine("(" + SE_Max.X + "," + SE_Max.Y + ").");
                    outputss = outputss.Concat(Get_points(SE_points, max_x, SE_Max)).ToList();
                    outputss = outputss.Concat(Get_points(SE_points, SE_Max, min_y)).ToList();
                }

                if (SW_points.Count() != 0)
                {

                    Point SW_Max = FindMaxDistancePointfromline(SW_points, SW);
                    outputss.Add(SW_Max);

                    outputss = outputss.Concat(Get_points(SW_points, min_y , SW_Max)).ToList();
                    outputss = outputss.Concat(Get_points(SW_points, SW_Max, min_x)).ToList();
                }

                outPoints = outputss.Distinct().ToList();
                foreach (Point paaa in outPoints)
                {
                    Console.WriteLine("(" + paaa.X + "," + paaa.Y + ")");
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
