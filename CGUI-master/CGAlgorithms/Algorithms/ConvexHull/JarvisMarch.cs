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

                List<Point> sorted = points.OrderBy(e => e.X).ToList();
                if (sorted.Count > 0)
                {
                    Point p = sorted[0];

                    outPoints.Add(p);

                    int num1 = 0;

                    for (int i = 1; i < sorted.Count(); i++)
                    {

                        Point p2 = sorted[i];

                        bool temp = true;
                        for (int j = 0; j < sorted.Count(); j++)
                        {
                            if (j == num1 || j == i)
                                continue;
                            else
                            {
                                if (CGUtilities.HelperMethods.CheckTurn(new Line(p, p2), sorted[j]) == Enums.TurnType.Left)
                                {
                                    p2 = sorted[j];
                                    temp = false;

                                    break;

                                }

                            }
                        }
                        if (temp)
                        {
                            outPoints.Add(p2);
                            Console.WriteLine("(" + p2.X + "," + p2.Y + ")");


                            p = p2;
                        }

                    }
                }
            }
           
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
