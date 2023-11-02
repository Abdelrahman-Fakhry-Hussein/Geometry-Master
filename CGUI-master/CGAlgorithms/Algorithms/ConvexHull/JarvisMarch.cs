using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //if counterclockwise we update the first with the last and discard the second 

            // if all in the same clockwise we keep it
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
                Console.WriteLine("*");
                List<Point> sorted = points.OrderBy(e => e.X).ToList();
                Point p = sorted[0];
                Console.WriteLine("(" + p.X + "," + p.Y + ").");
                int ind = 1;
                outPoints.Add(p);
                do
                {

                    ind = (ind + 1) % points.Count();

                    
                    for (int j = 0; j < points.Count(); j++)
                    {

                    
                        if (CGUtilities.HelperMethods.CheckTurn(new Line((Point)p, (Point)points[ind]), points[j]) == Enums.TurnType.Left)
                        {
                            ind = j;

                        }
                        else if (CGUtilities.HelperMethods.CheckTurn(new Line((Point)p, (Point)points[ind]), points[j]) == Enums.TurnType.Colinear)
                        {

                            if (CGUtilities.HelperMethods.PointOnSegment(points[j], p, points[ind]) == false)
                            {
                                ind = j;
                            }
                           

                        }



                    }

                    
                    if (points[ind].X == sorted[0].X && points[ind].Y == sorted[0].Y)
                        break;
                    outPoints.Add(points[ind]);
                    p = points[ind];
                } while (true);


                foreach(Point pasaa in outPoints)
                    Console.WriteLine("(" + pasaa.X + "," + pasaa.Y + ")");





            }
          
           
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
