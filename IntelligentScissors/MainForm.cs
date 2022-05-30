using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        bool mdown_done = false;
        int frequancy = 57;
        List<Point> point_of_anchor=new List<Point>();


        int cu_src = -1;
            int main = -1;
        limit slimit;
        RGBPixel[,] segment_SQ;
        List<int> rootlist;
        Point[] currentpth;
        List<Point> m_slct=new List<Point>();
        int before_pos_of_mouse;
        Point size_of_anchor = new Point(5, 5);
        Pen main_selection_ofpen = new Pen(Brushes.Orange, 1); // MAIN SELECTION PEN
        Pen current_path_ofpen = new Pen(Brushes.Aqua, 1);  
        float[] pattern = { (float)1, (float)0.000000000001 };
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] matrix_of_img;

        void init()
        {
            m_slct = new List<Point>();
            point_of_anchor = new List<Point>();
           
        }
        void renew()
        {

            
            point_of_anchor.Clear();
            currentpth = null;
            cu_src = -1;
            m_slct.Clear();
            main = -1;
            before_pos_of_mouse = -1;
            

            mdown_done = false;
        }
       
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                matrix_of_img = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(matrix_of_img, pictureBox1);
				renew();
			}
            txtWidth.Text = ImageOperations.GetWidth(matrix_of_img).ToString();
            txtHeight.Text = ImageOperations.GetHeight(matrix_of_img).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            matrix_of_img = ImageOperations.GaussianFilter1D(matrix_of_img, maskSize, sigma);
            ImageOperations.DisplayImage(matrix_of_img, pictureBox2);
        }

		private void pictureBox1_DoubleClick(object sender, EventArgs e)
		{

		}

		private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
		{
            if (pictureBox1.Image != null)
            {
                var node_that_clicked = functions.converter_2d_1d(e.X, e.Y, ImageOperations.GetWidth(matrix_of_img));

                if (cu_src != node_that_clicked)
                {
                    if (cu_src == -1) 
                    {
                        main = node_that_clicked;
                    }
                    else if(cu_src!=-1)
                    {
                        functions.Insettolist<Point>(m_slct, currentpth);
                    }
                    slimit = new limit();
                    int n_src;
                    point_of_anchor.Add(e.Location);
                    cu_src = node_that_clicked;
                    int x;
                    int y;
                    x = ImageOperations.GetWidth(matrix_of_img) - 1;
                    y = ImageOperations.GetHeight(matrix_of_img) - 1;
                    slimit = shortest_path_functions.boundrysquare(cu_src,x,y);
                    
                    segment_SQ = functions.repeat(slimit, matrix_of_img);


                    x = ImageOperations.GetWidth(matrix_of_img);
                     y = ImageOperations.GetWidth(segment_SQ);
                    n_src = functions.pixel_handling(slimit, cu_src, x,y);
                    rootlist = shortest_path_functions.Dijkstra(n_src, segment_SQ);
                    anchor.renew();
                }
            }
        }
        public static class drawer
        {


            public static void dash_line(Graphics graphics, Pen pen, Point[] points, float[] values)
            {
                pen.DashPattern = values;
                graphics.DrawCurve(pen, points);
            }


            public static void dash_line(Graphics graphics, Pen pen, Point point1, Point point2, float[] values)
            {
                Point[] points = new Point[2];
                points[0] = point1;
                points[1] = point2;

                dash_line(graphics, pen, points, values);

            }
            
        }

      

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
            mdown_done = true;
		}
        float cfloat = 0.0f;
        float time_wait = .02f;

        public void modify(MouseEventArgs e)
        {
            var picture = pictureBox1.CreateGraphics();
            if (cfloat > time_wait * 2)
            {
                if (matrix_of_img != null)
                {
                    int p = ImageOperations.GetWidth(matrix_of_img);

                    if (before_pos_of_mouse != functions.converter_2d_1d(e.X, e.Y, p))
                    {
                        if (cu_src != -1)
                        {
                            before_pos_of_mouse = functions.converter_2d_1d(e.X, e.Y, p);
                            int v = ImageOperations.GetWidth(matrix_of_img);
                            if (functions.inbox(functions.converter_2d_1d(e.X, e.Y, p), slimit, v))
                            {
                                List<Point> segmentpath = new List<Point>();
                                int x;
                                int y;
                                int z;
                                x = ImageOperations.GetWidth(matrix_of_img);
                                y = ImageOperations.GetWidth(segment_SQ);
                                int Segment_mouse = functions.pixel_handling(slimit, functions.converter_2d_1d(e.X, e.Y, p), x, y);
                                z = ImageOperations.GetWidth(segment_SQ);
                                segmentpath = shortest_path_functions.shortestpath(rootlist, Segment_mouse, z);
                                List<Point> Curpath = functions.pixel_handling(slimit, segmentpath);
                                currentpth = Curpath.ToArray();

                                if (mdown_done)
                                {
                                    List<Point> pthcool = anchor.pathofanchor();
                                    double fq = (double)frequancy / 1000;
                                    anchor.Update(Curpath, fq);

                                    if (pthcool.Count > 0)
                                    {
                                        slimit = new limit();
                                        int o = ImageOperations.GetWidth(matrix_of_img);
                                        Point anchoor = pthcool[pthcool.Count - 1];
                                        point_of_anchor.Add(anchoor);
                                        functions.Insettolist<Point>(m_slct, pthcool);
                                        cu_src = functions.converter_2d_1d(anchoor.X, anchoor.Y, o);
                                        slimit = shortest_path_functions.boundrysquare(cu_src, o - 1, ImageOperations.GetHeight(matrix_of_img) - 1);
                                        segment_SQ = functions.repeat(slimit, matrix_of_img);
                                        int n_src = functions.pixel_handling(slimit, cu_src, o, ImageOperations.GetWidth(segment_SQ));
                                        rootlist = shortest_path_functions.Dijkstra(n_src, segment_SQ);
                                        anchor.renew();
                                    }
                                }
                            }
                            else
                                currentpth = null;
                        }
                    }
                }
                cfloat = 0.0f;
            }
            if (cfloat > time_wait)
            {
                pictureBox1.Refresh();
                picture.Dispose();
            }
            cfloat += .019f;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (matrix_of_img != null)
            {
                var picture = e.Graphics;


                int i = 0;
                while (i < point_of_anchor.Count)
                {
                    int x = point_of_anchor[i].X - size_of_anchor.X / 2;
                    int y = point_of_anchor[i].Y - size_of_anchor.Y / 2;
                    Point loc = new Point(x, y);
                    Size size = new Size(size_of_anchor);
                    picture.FillEllipse(Brushes.Yellow, new Rectangle(loc, size));
                    i++;
                }
                if (currentpth != null)
                {
                    if (currentpth.Length > 10)
                    {
                        drawer.dash_line(picture, current_path_ofpen, currentpth, pattern);
                    }
                }

                if (m_slct != null && m_slct.Count > 5)
                {
                    drawer.dash_line(e.Graphics, main_selection_ofpen, m_slct.ToArray(), pattern);
                }
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            modify(e);
            txtBox_x_axis.Text = e.X.ToString();
            txtbox_y_axis.Text = e.Y.ToString();
            if (pictureBox1.Image != null)
            {
                int wdth = ImageOperations.GetWidth(matrix_of_img);
                textBox1.Text = functions.converter_2d_1d(e.X, e.Y, wdth).ToString();
            }
        }

		private void pictureBox2_Click(object sender, EventArgs e)
		{

		}

		private void button1_MouseUp(object sender, MouseEventArgs e)
		{
            mdown_done = false;
        }

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
            mdown_done = false;
        }

		private void label7_Click(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}

		private void txtbox_y_axis_TextChanged(object sender, EventArgs e)
		{

		}

		private void label9_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
            if (cu_src != main)
            {
                RGBPixel[,] img_take;
                int wdth = ImageOperations.GetWidth(matrix_of_img);
                if (functions.inbox(main, slimit, wdth))
                {
                    int wdth_matrix = ImageOperations.GetWidth(matrix_of_img);
                    int wdthseg = ImageOperations.GetWidth(segment_SQ);
                    int Segment_mouse;
                    List<Point> segmentpath = new List<Point>();
                    int wdth_seg2 = ImageOperations.GetWidth(segment_SQ);
                    Segment_mouse = functions.pixel_handling(slimit, main, wdth_matrix, wdthseg);
                    
                    
                    segmentpath = shortest_path_functions.shortestpath(rootlist, Segment_mouse, wdth_seg2);
                    currentpth = functions.pixel_handling(slimit, segmentpath).ToArray();
                }
                else if (!functions.inbox(main, slimit, wdth))
                {
                    currentpth = shortest_path_functions.makeshortpath(cu_src, main, matrix_of_img).ToArray();
                }
                functions.Insettolist<Point>(m_slct, currentpth);


               
                img_take = fill.Fill(m_slct, matrix_of_img);
              
                ImageOperations.DisplayImage(img_take, pictureBox2);
              }
        }
	}
}