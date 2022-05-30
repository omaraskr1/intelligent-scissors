using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
	class functions
	{
       
        public static Vector2D make_2d(int Index, int width)
        {
            Vector2D v2 = new Vector2D();
            v2.X = ((int)Index % (int)width);
            v2.Y = (int)Index / width;
            return v2;
        }
        // bool if point in selected rectangle or not
        public static bool inbox(int point, limit bondry, int Width)
        {

            Vector2D points = make_2d(point, Width);
            bool x2 = (points.Y >= bondry.miny);
            bool x4 = points.Y < bondry.maxy;
            bool x = (points.X >= bondry.minx);
            bool x3 = points.X < bondry.maxx;
            bool res = x && x2 && x3 && x4;
            return res;
        }
        //get crosponding pixel in small rectangle 
        private static Point pixel_handling(limit bondry, Point P)
        {
            P.X += bondry.minx;
            P.Y += bondry.miny;
            return P;
        }
        //sure if pixel in boundry of image
        public static bool Vaild_Pixel(int X, int Y, RGBPixel[,] ImageMatrix)
        {
            bool x = ( X < ImageOperations.GetWidth(ImageMatrix));
            bool x3 = X >= 0;
            bool x2 = ( Y < ImageOperations.GetHeight(ImageMatrix));
            bool x4 = Y >= 0;

            bool res = x && x2 && x3 && x4;
            return res;
        }
        //get crosponding pixel in small rectangle 
        public static List<Point> pixel_handling(limit bondry, List<Point> pth)
        {
            int i = 0;
            while (i < pth.Count)
            {
                pth[i] = pixel_handling(bondry,pth[i]);
                i++;
            }
            return pth;
        }
        //get crosponding pixel in small rectangle 
        public static int pixel_handling(limit bondry, int nodes, int width, int s_width)
        {
            Vector2D axis = make_2d(nodes, width);
            axis.X = axis.X - bondry.minx;
            axis.Y = axis.Y - bondry.miny;
            int newnode = converter_2d_1d((int)axis.X, (int)axis.Y, s_width);
            return newnode;
        }

        
        //copy from large image to small image
        public static RGBPixel[,] repeat(limit bondry, RGBPixel[,] ImageMatrix)
        {

            int Height = bondry.maxy - bondry.miny;
            int r = 0;
            int Width = bondry.maxx - bondry.minx; 
          
            RGBPixel[,] img = new RGBPixel[Height + 1, Width + 1];
           
            while (r <= Height)
            {
                for (int z = 0; z <= Width; z++)
                {
                    img[r, z] = ImageMatrix[bondry.miny + r, bondry.minx + z];
                }
                r++;
            }
            return img;
        }

        public static List<T> Insettolist<T>(List<T> dst, List<T> src)
        {
            List<T> tmp = dst;
            if (src == null)
            {
                
                throw new ArgumentNullException();
            }

            if (dst == null  )
            {

                throw new ArgumentNullException();
            }

            int i = 0;
            while (i < src.Count())
            {
                tmp.Add(src[i]);
                i++;
            }
            return tmp;

        }
        
        public static int converter_2d_1d(int X, int Y, int width)
        {
            return (X) + (Y * width);
        }

        public static List<T> Insettolist<T>(List<T> dst, T[] src)
        {
            List<T> tp = dst;
            if (src == null)
            {
                return null;
                throw new ArgumentNullException();
            }
            
            
            
            if (dst == null )
            {
                return null;
                throw new ArgumentNullException();

            }
            int i = 0;
            while ( i < src.Length)
            {
                tp.Add(src[i]);
                i++;
            }
           
            return tp;

        }



    }
}
