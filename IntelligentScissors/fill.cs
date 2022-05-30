using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public struct limit
    {
        public int maxx;
        public int minx;
        public int maxy;
        public int miny;

    }
    class fill
    {
        private static RGBPixel[,] simg;

        public static bool maax(int i, int j)
        {
            if (i > j)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int minus(int x, int y)
        {
            return x - y;
        }
        
        //make bounries of rectangle
        private static limit get_limit(List<Point> spt)
        {

            limit bd;
            bd.maxx = -1000000000;
            bd.maxy = -1000000000;
            bd.minx = 1000000000;
            bd.miny = 1000000000;

            int i = 0;
            while (maax(spt.Count, i))
            {
                int tmpx = spt[i].X;
                int tmpy = spt[i].Y;
                if (maax(tmpy, bd.maxy)) bd.maxy = tmpy;
                if (maax(bd.miny, tmpy)) bd.miny = tmpy;
                if (maax(tmpx, bd.maxx)) bd.maxx = tmpx;
                if (maax(bd.minx, tmpx)) bd.minx = tmpx;
                i++;
            }
            return bd;

        }

        //clarify selected object from rectangle by make white color for unselected object 
        private static void dfs(Vector2D s_point)
        {
            Stack<Vector2D> dfs_heab = new Stack<Vector2D>();
            dfs_heab.Push(s_point);
            while (maax(dfs_heab.Count, 0))
            {
                Vector2D current = dfs_heab.Pop();
                if (functions.Vaild_Pixel((int)current.X, (int)current.Y, simg))
                {
                    if (!simg[(int)current.Y, (int)current.X].block)
                    {
                        simg[(int)current.Y, (int)current.X].green = 240;
                        simg[(int)current.Y, (int)current.X].red = 240;
                        simg[(int)current.Y, (int)current.X].blue = 240;
                        simg[(int)current.Y, (int)current.X].block = true;

                        Vector2D vec1 = new Vector2D();
                        vec1.X = current.X;
                        vec1.Y = current.Y + 1;
                        Vector2D vec2 = new Vector2D();
                        vec2.X = current.X;
                        vec2.Y = current.Y - 1;
                        Vector2D vec3 = new Vector2D();
                        vec3.X = current.X + 1;
                        vec3.Y = current.Y;
                        Vector2D vec4 = new Vector2D();
                        vec4.X = current.X - 1;
                        vec4.Y = current.Y;

                        dfs_heab.Push(vec1);
                        dfs_heab.Push(vec2);
                        dfs_heab.Push(vec3);
                        dfs_heab.Push(vec4);
                    }
                }

            }

        }

        //make sure pt in border rectangle or not
        private static void blk_limit(List<Point> spt, limit bd)
        {

            int i = 0;
            while (maax(spt.Count, i))
            {
                int X = minus(spt[i].X, bd.minx);
                int Y = minus(spt[i].Y, bd.miny);
                simg[Y, X].block = true;
                i++;
            }


        }


        public static RGBPixel[,] Fill(List<Point> spt, RGBPixel[,] img)
		{
            limit bd = get_limit(spt); 
            simg = functions.repeat(bd, img);//make small rectangle from original image  
			blk_limit(spt, bd);                     
			ffill(ImageOperations.GetWidth(simg) - 1, ImageOperations.GetHeight(simg) - 1);
			return simg;
		}

        

        
       // if pt not visited send it to dfs 
        private static void ffill(int Width, int Height)
        {
            
            
            int i = 0;
            while (i <= Width){
               
                if (!simg[0, i].block)
                { 
                    Vector2D vec2 = new Vector2D();
                    vec2.Y = 0; 
                    vec2.X = i;
                    dfs(vec2);
                       
                } 
                
                if (!simg[Height, i].block)
                {
                    Vector2D vec2 = new Vector2D();
                    vec2.X = i;
                    vec2.Y = Height;
                    dfs(vec2);
                        
                }
                    i++;
            }

            int k= 0;
            while (k <= Height)
            {
                if (!simg[k, 0].block)
                {
                    Vector2D vec2 = new Vector2D();
                    vec2.X = 0;
                    vec2.Y = k;
                    dfs(vec2);
                }
                if (!simg[k, Width].block)
                {
                    Vector2D vec2 = new Vector2D();
                    vec2.X = Width;
                    vec2.Y = k;
                    dfs(vec2);
                }
                k++;
            }
            
        }


    }
}
