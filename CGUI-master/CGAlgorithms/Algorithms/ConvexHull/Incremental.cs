using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            outPoints = new List<Point>();
            if (points.Count() <= 3)
            {

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
                outPoints.Add(min_x); outPoints.Add(max_y); outPoints.Add(max_x); outPoints.Add(min_y);
                foreach(Point p in points)
                {
                    int min_range = -1; int max_range = 0;
                    bool temp = false;
                    List<Point> remove = new List<Point>();
                    for(int i = 0; i < outPoints.Count() -1;i++)
                    {
                        if (CGUtilities.HelperMethods.CheckTurn(new Line((Point)outPoints[i], (Point)outPoints[i+1]), p) == Enums.TurnType.Left)
                        {
                            if(min_range == -1)
                                min_range = i;
                            max_range = i+1;
                            temp = true;
                        }
                        else if (CGUtilities.HelperMethods.CheckTurn(new Line((Point)outPoints[i], (Point)outPoints[i + 1]), p) == Enums.TurnType.Colinear)
                        {

                            if (CGUtilities.HelperMethods.PointOnSegment( p,(Point)outPoints[i], (Point)outPoints[i + 1]) == false)
                            {
                                Console.WriteLine("(" + outPoints[i].X + "," + outPoints[i].Y + ").");
                                remove.Add(outPoints[i ]);
                               
                            }


                        }
                    }

                    if (CGUtilities.HelperMethods.CheckTurn(new Line((Point)outPoints[outPoints.Count()-1], (Point)outPoints[0]), p) == Enums.TurnType.Left)
                    {
                        Console.WriteLine("(" + p.X + "," + p.Y + ")#");
                        outPoints.Add(p);
                    }

                    if (temp) 
                    {
                       for(int j = max_range - 1 ; j > min_range; j--)
                       {
                          

                            Point ps = outPoints[j];
                            outPoints.Remove(ps);
                       }
                       foreach(Point psss in remove)
                        {
                           outPoints.Remove(psss);
                        }
                        outPoints.Insert(min_range + 1, p);
                    }
                }
                HashSet<Point> outss = new HashSet<Point>();
                foreach (Point pasaa in outPoints)
                {
                    outss.Add(pasaa);
                    
                    
                }
               
                outPoints = outss.ToList();
               
             }
            


        }
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
