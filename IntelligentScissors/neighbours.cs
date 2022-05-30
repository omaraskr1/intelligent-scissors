using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    class neighbours
    {

        public static Vector2D Free_X_Y(int Index, int width)
        {
            Vector2D V2 = new Vector2D();
            V2.X = (int)Index % (int)width;
            V2.Y = ((int)Index / width);

            return V2;
        }
        public static List<edge> Get_neig(int MY_Ind, RGBPixel[,] imgmatrix)
        {

            int Wdth = ImageOperations.GetWidth(imgmatrix);
            var free = Free_X_Y(MY_Ind, Wdth);
            int X_axis = (int)free.X, Y_axis = (int)free.Y;
            var Energy_Weight = ImageOperations.CalculatePixelEnergies(X_axis, Y_axis, imgmatrix);
            int Hgt = ImageOperations.GetHeight(imgmatrix);
            List<edge> neghlist = new List<edge>();



            if (X_axis < Wdth - 1)
            {
                if (Energy_Weight.X == 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis + 1) + Y_axis * Wdth), 10000000000000000));
                else if (Energy_Weight.X != 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis + 1) + Y_axis * Wdth), 1 / (Energy_Weight.X)));
            }
            if (Y_axis < Hgt - 1)
            {
                if (Energy_Weight.Y == 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis) + (Y_axis + 1) * Wdth), 10000000000000000));
                else if (Energy_Weight.Y != 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis) + (Y_axis + 1) * Wdth), 1 / (Energy_Weight.Y)));
            }
            if (X_axis > 0)
            {
                Energy_Weight = ImageOperations.CalculatePixelEnergies(X_axis - 1, Y_axis, imgmatrix);

                if (Energy_Weight.X == 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis - 1) + Y_axis * Wdth), 10000000000000000));
                else if (Energy_Weight.X != 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis - 1) + Y_axis * Wdth), 1 / (Energy_Weight.X)));
            }

            if (Y_axis > 0)
            {
                Energy_Weight = ImageOperations.CalculatePixelEnergies(X_axis, Y_axis - 1, imgmatrix);
                if (Energy_Weight.Y == 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis) + (Y_axis - 1) * Wdth), 10000000000000000));
                else if (Energy_Weight.Y != 0)
                    neghlist.Add(new edge(MY_Ind, ((X_axis) + (Y_axis - 1) * Wdth), 1 / (Energy_Weight.Y)));
            }



            return neghlist;

        }
    }
}
