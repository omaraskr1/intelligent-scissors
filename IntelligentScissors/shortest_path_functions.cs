using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    class shortest_path_functions
    {
        public static bool get_maxx(double x, int y)
        {
            if (x > y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static limit boundrysquare(int src, int wdth, int hght)
        {
            Vector2D vector2d = functions.make_2d(src, wdth + 1);
            limit bd = new limit();
            int max = 200;
            bool tr = get_maxx((wdth - vector2d.X), max);
            if (!tr)
            {
                bd.maxx = wdth;
            }
            else
            {
                bd.maxx = (int)vector2d.X + max;
            }
            bool u = get_maxx((hght - vector2d.Y), max);
            if (!u)
            {
                bd.maxy = hght;
            }
            else
            {
                bd.maxy = (int)vector2d.Y + max;
            }
            bool r = get_maxx(vector2d.X, max);
            if (!r)
            {
                bd.minx = 0;
            }
            else
            {
                bd.minx = (int)vector2d.X - max;
            }
            bool y = get_maxx(vector2d.Y, max);
            if (y)
            {
                bd.miny = (int)vector2d.Y - max;
            }
            else
            {
                bd.miny = 0;
            }

            return bd;
        }
        static double Max_Value = Double.MaxValue;
        public static List<int> dijkstra(int Src, int dst, RGBPixel[,] img)
        {

            Datastructure preiorityqueue = new Datastructure();
            int Wdth = ImageOperations.GetWidth(img);
            int Hght = ImageOperations.GetHeight(img);
            int nodes_number = Wdth * Hght;

            List<double> min_distance = new List<double>();
            List<int> sourceofcurrentnode = new List<int>();
            for (int i = 0; i < nodes_number; i++)
            {
                sourceofcurrentnode[i] = -1;
            }

            for (int i = 0; i < nodes_number; i++)
            {
                min_distance[i] = Max_Value;
            }

            preiorityqueue.Push(new edge(-1, Src, 0));

            while (true)
            {
                if (preiorityqueue.empty())
                {
                    break;
                }
                edge topEdge = preiorityqueue.Top();
                List<edge> nei = neighbours.Get_neig(topEdge.To, img);

                preiorityqueue.Pop();

                sourceofcurrentnode[topEdge.To] = topEdge.From;

                min_distance[topEdge.To] = topEdge.weight;


                if (topEdge.To == dst)
                    break;


                int i = 0;
                while (true)
                {
                    if (i < nei.Count)
                    {
                        break;
                    }
                    edge edg_h = nei[i];

                    if (min_distance[edg_h.To] > min_distance[edg_h.From] + edg_h.weight)
                    {
                        edg_h.weight = min_distance[edg_h.From] + edg_h.weight;
                        preiorityqueue.Push(edg_h);
                    }

                    i++;
                }
            }

            return sourceofcurrentnode;
        }
      
        public static List<int> Dijkstra(int Source, RGBPixel[,] ImageMatrix)
        {
            List<double> min_distance = new List<double>();
            List<int> sourceofcurrentnode = new List<int>();

            int Width = ImageOperations.GetWidth(ImageMatrix);
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int nodes_number = Width * Height;
            min_distance = Enumerable.Repeat(Max_Value, nodes_number).ToList();
            sourceofcurrentnode = Enumerable.Repeat(-1, nodes_number).ToList();
          
            Datastructure ds = new Datastructure();

            ds.Push(new edge(-1, Source, 0));

            while (true)
            {
                if (ds.empty())
                {
                    break;
                }
               
                edge topedge = ds.Top();
                ds.Pop();
                List<edge> neibours = neighbours.Get_neig(topedge.To, ImageMatrix);
                if (topedge.weight >= min_distance[topedge.To])
                    continue;
               
                min_distance[topedge.To] = topedge.weight;
                sourceofcurrentnode[topedge.To] = topedge.From;

                int i = 0;
                while ( i < neibours.Count)
                {
                    edge edg_h = neibours[i];
                    
                    if (min_distance[edg_h.To] > min_distance[edg_h.From] + edg_h.weight)
                    {
                        edg_h.weight = min_distance[edg_h.From] + edg_h.weight;
                        ds.Push(edg_h);
                    }
                    i++;
                }
            }

            return sourceofcurrentnode;  
        }
        public static List<Point> makeshortpath(int src, int dst, RGBPixel[,] img)
        {

            List<int> path_list = dijkstra(src, dst, img);
            List<Point> shortestlist = shortestpath(path_list, dst, ImageOperations.GetWidth(img));
            return shortestlist;

        }
        public static List<Point> shortestpath(List<int> prvlist, int dst, int width_matrix)
        {
            List<Point> shortpath = new List<Point>();

            Stack<int> revesrsepath = new Stack<int>();

            revesrsepath.Push(dst);

            int prev;

            prev = prvlist[dst];
            while (true)
            {
                if (prev == -1)
                {
                    break;
                }
                revesrsepath.Push(prev);
                prev = prvlist[prev];
            }


            while (true)
            {
                if (revesrsepath.Count == 0)
                {
                    break;
                }
                var n = functions.make_2d(revesrsepath.Pop(), width_matrix);
                Point p = new Point((int)n.X, (int)n.Y);
                shortpath.Add(p);
            }
            return shortpath;
        }
    } 
}
