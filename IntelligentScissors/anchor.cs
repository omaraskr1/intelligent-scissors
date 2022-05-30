using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
	public static class anchor
	{
        public static List<double> cool_Time;
        public static List<Point> Cut_wire;
        public static List<int> new_rope_color;


        public static List<Point> pathofanchor()
        {
            int freeze = 0;
            List<Point> free_path = new List<Point>();
            for (int i = 0; i < Cut_wire.Count; i++)
            {
                if (cool_Time[i] > 1) 
                {
                    if(new_rope_color[i] >= 10)
                        freeze = i;
                }
            }
            int l = 0;
            while (l < freeze)
            {
                free_path.Add(Cut_wire[l]);
                l++;
            }
            return free_path;
        }
       
        // update wire to pathwire to get cooled path
        public static void Update(List<Point> Path, double Ctime)
        {
            int wire_size = Cut_wire.Count; 
            int path_length = Path.Count;


            int i = 0;
            int j = 0;
            while (j < wire_size && i < path_length)
            {
                if (Path[i] != Cut_wire[j])
                {

                    Cut_wire[j] = Path[i];
                    cool_Time[j] = 0;
                    new_rope_color[j] = 0;//number of drawen wire
                }
                else if(Path[i] == Cut_wire[j])
                {
                    cool_Time[j] += Ctime;
                    new_rope_color[j] += 1;
                }
                i++; j++;
            }

            for (int z = 0; z < wire_size; z++)
            {
                cool_Time[z] = 0;
                new_rope_color[z] = 0;
                Cut_wire[z] = new Point(-1, -1);

            }
            
            for (int k=0;k< path_length;k++)
            {
                cool_Time.Add(0);
                new_rope_color.Add(0);
                Cut_wire.Add(Path[k]);
            }

            
           
        }
        public static void renew()
        {
            new_rope_color = new List<int>();

            Cut_wire = new List<Point>();

            cool_Time = new List<double>();

        }


    }
}
