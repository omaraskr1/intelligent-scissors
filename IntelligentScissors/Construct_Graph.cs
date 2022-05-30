using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public class edge
    {
        public double weight;
        public int From, To;
        public edge(int From, int To, double weight)
        {
            this.To = To;
            this.From = From;
            this.weight = weight;
        }
    }

    class Construct_Graph
    {

        public static Dictionary<int, List<edge>> Graph(RGBPixel[,] ImageMatrix)
        {
            int Hgh = ImageOperations.GetHeight(ImageMatrix);
            int Wdth = ImageOperations.GetWidth(ImageMatrix);
            Dictionary<int, List<edge>> adj = new Dictionary<int, List<edge>>();
            int i = 0;
            while( i < Hgh)
            {
                int j = 0;
                while (j < Wdth)
                {
                    
                    List<edge> neig =neighbours.Get_neig((j + i * Wdth), ImageMatrix);
                    adj[(j + i * Wdth)]= neig;
                    j++;
                }
                i++;
            }
            return adj;
        }
       
    }

}
