using CGUtilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort((a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));
            outPoints = ConvexHullDC(points);
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

        private List<Point> ConvexHullDC(List<Point> points)
        {
            int n = points.Count;
            if (n == 1)
                return points;

            List<Point> L = new List<Point>();
            List<Point> R = new List<Point>();

            for (int i = 0; i < points.Count / 2; i++) L.Add(points[i]);
            for (int i = points.Count / 2; i < points.Count; i++) R.Add(points[i]);

            List<Point> leftHull = ConvexHullDC(L);
            List<Point> rightHull = ConvexHullDC(R);
            return Merge(leftHull, rightHull);
        }

        private int GetMin(List<Point> points)
        {
            int index = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[index].X > points[i].X)
                    index = i;
                else if (points[i].X == points[index].X)
                {
                    if (points[index].Y > points[i].Y)
                        index = i;
                }
            }
            return index;
        }

        private int GetMax(List<Point> points)
        {
            int index = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[index].X < points[i].X)
                    index = i;
                else if (points[i].X == points[index].X)
                {
                    if (points[index].Y < points[i].Y)
                        index = i;
                }
            }
            return index;
        }

        private Tuple<int, int> UpperTang(List<Point> Left, List<Point> Right,int maxleft,int minright)
        {
            int countleft = Left.Count;
            int countright = Right.Count;
            int nextleft = (maxleft + 1) % countleft;
            int prevright = (countright + minright - 1) % countright;
            bool found;
            do
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(Right[minright], Left[maxleft]), Left[nextleft]) == Enums.TurnType.Right)
                {
                    maxleft = nextleft;
                    nextleft = (maxleft + 1) % countleft;
                    found = false;
                }

                if (found && (HelperMethods.CheckTurn(new Line(Right[minright], Left[maxleft]), Left[nextleft]) == Enums.TurnType.Colinear))
                    maxleft = nextleft;
                nextleft = (maxleft + 1) % countleft;

                while (HelperMethods.CheckTurn(new Line(Left[maxleft], Right[minright]), Right[prevright]) == Enums.TurnType.Left)
                {
                    minright = prevright;
                    prevright = (countright + minright - 1) % countright;
                    found = false;
                }

                if (found && (HelperMethods.CheckTurn(new Line(Left[maxleft], Right[minright]), Right[prevright]) == Enums.TurnType.Colinear))
                    minright = prevright;
                prevright = (countright + minright - 1) % countright;


            } while (found == false);

            return new Tuple<int, int>(maxleft, minright);
        }

        private Tuple<int, int> LowerTang(List<Point> Left, List<Point> Right, int maxleft, int minright)
        {
            bool found;
            int countleft = Left.Count;
            int countright = Right.Count;
            int prevleft = (countleft + maxleft - 1) % countleft;
            int nextright = (minright + 1) % countright;

            do
            {
                found = true;
                while (HelperMethods.CheckTurn(new Line(Right[minright], Left[maxleft]), Left[prevleft]) == Enums.TurnType.Left)
                {
                    maxleft = prevleft;
                    prevleft = (countleft + maxleft - 1) % countleft;
                    found = false;
                }

                if (found && (HelperMethods.CheckTurn(new Line(Right[minright], Left[maxleft]), Left[prevleft]) == Enums.TurnType.Colinear))
                    maxleft = prevleft;
                prevleft = (countleft + maxleft - 1) % countleft;

                while (HelperMethods.CheckTurn(new Line(Left[maxleft], Right[minright]), Right[nextright]) == Enums.TurnType.Right)
                {
                    minright = nextright;
                    nextright = (minright + 1) % countright;
                    found = false;
                }

                if (found && (HelperMethods.CheckTurn(new Line(Left[maxleft], Right[minright]), Right[nextright]) == Enums.TurnType.Colinear))
                    minright = nextright;
                nextright = (minright + 1) % countright;

            } while (!found);

            return new Tuple<int, int>(maxleft, minright);
        }

        private List<Point> Merge(List<Point> Left, List<Point> Right)
        {
            List<Point> Ans = new List<Point>();
            int maxleft = GetMax(Left);
            int minright = GetMin(Right);
            Tuple<int, int> upper = UpperTang(Left, Right, maxleft, minright);
            Tuple<int, int> lower = LowerTang(Left, Right, maxleft, minright);
            
            int ind = upper.Item1;
            if (!Ans.Contains(Left[ind]))
                Ans.Add(Left[ind]);
            while (ind != lower.Item1)
            {
                ind = (ind + 1) % Left.Count;
                if (!Ans.Contains(Left[ind]))
                    Ans.Add(Left[ind]);
            }

            ind = lower.Item2;
            if (!Ans.Contains(Right[ind]))
                Ans.Add(Right[ind]);
            while (ind != upper.Item2)
            {
                ind = (ind + 1) % Right.Count;
                if (!Ans.Contains(Right[ind]))
                    Ans.Add(Right[ind]);
            }
            return Ans;
        }
    }
}
