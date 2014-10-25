using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelaxMethod
{
    public partial class Form1 : Form
    {
        int IterCount;
        DateTime startTime, endTime;
        Bitmap bm;
        Graphics gr;
        int Lx, Ly;
        double[,] T, TT;
        int N;
        double Tleft, TRight, TUp, TDown;
        Color Col;
        double kx, Err1;
        int dx;
        SolidBrush brush1;
        Boolean priznak;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Lx = pictureBox1.Height;
            Ly = pictureBox1.Height;

            bm = new Bitmap(Lx + 1, Ly + 1);
        }

        private void showpic()
        {
            for (int j = 0; j <= N; j++)
            {
                for (int i = 0; i <= N; i++)
                {
                    if (T[i, j] >= 0)
                    {
                        Col = Color.FromArgb(Convert.ToInt32(255 * (T[i, j]) / 13), 0, 0);
                    }
                    else
                    {
                        Col = Color.FromArgb(0, 0, Convert.ToInt32(-255 * T[i, j] / 13));
                    }
                    brush1.Color = Col;
                    gr.FillRectangle(brush1, j * dx, i * dx, dx, dx);
                }
            }
            pictureBox1.Image = bm;
        }

        //Start
        private void button1_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            priznak = false;
            IterCount = 0;
            if (checkBox1.Checked == false)
            {
                do
                {
                    priznak = true;
                    IterCount += 1;

                    for (int j = 1; j <= N - 1; j++)
                    {
                        for (int i = 1; i <= N - 1; i++)
                        {
                            //релаксация
                            TT[i, j] = 0.25 * (T[i - 1, j] + T[i + 1, j] + T[i, j - 1] + T[i, j + 1]);
                            if (Math.Abs(TT[i, j] - T[i, j]) > Err1) { priznak = false; }
                        }
                    }

                    for (int j = 1; j <= N - 1; j++)
                    {
                        for (int i = 1; i <= N - 1; i++)
                        {
                            T[i, j] = TT[i, j];
                        }
                    }
                } while (priznak == false || IterCount < 10000);

                if (priznak == false) textBoxerror.BackColor = Color.Red;
                else { textBoxerror.BackColor = Color.White; }
            }
            else { timer1.Enabled = true; }

            endTime = DateTime.Now;
            showpic();
            textBoxTime.Text = Convert.ToString(endTime.Minute * 60 + endTime.Second - startTime.Minute * 60 - startTime.Second);
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            textBoxitercount.Text = "";
            Err1 = Convert.ToDouble(textBoxerror.Text);
            N = Convert.ToInt32(textBoxN.Text);
            Tleft = Convert.ToDouble(textBoxLeft.Text);
            TRight = Convert.ToDouble(textBoxRight.Text);
            TDown = Convert.ToDouble(textBoxDown.Text);
            TUp = Convert.ToDouble(textBoxUp.Text);
            T = new double[N + 1, N + 1];
            TT = new double[N + 1, N + 1];

            kx = Lx;
            dx = Convert.ToInt32((kx / (N + 1)));
            pictureBox1.Width = (pictureBox1.Height / dx) * dx;
            pictureBox1.Height = pictureBox1.Width;
            Lx = pictureBox1.DisplayRectangle.Width;
            Ly = pictureBox1.DisplayRectangle.Width;

            brush1 = new SolidBrush(Color.Red);
            gr = Graphics.FromImage(bm);
            pictureBox1.Image = bm;
            IterCount = 0;

            //Граничные условия
            for (int i = 0; i <= N; i++)
            {
                T[0, i] = TUp;
                T[i, 0] = Tleft;
                T[N, i] = TDown;
                T[i, N] = TRight;
            }
            //начальные условия
            for (int j = 1; j <= N - 1; j++)
            {
                for (int i = 1; i <= N - 1; i++)
                {
                    T[i, j] = 8;
                }
            }
            showpic();
        }//инициализация

        private void textBoxitercount_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxerror_TextChanged(object sender, EventArgs e)
        {

        }

        //Stop
        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (priznak == false) textBoxerror.BackColor = Color.Red;
            else { textBoxerror.BackColor = Color.White; }
        }

        //timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            priznak = true;
            IterCount += 1;

            for (int j = 1; j <= N - 1; j++)
            {
                for (int i = 1; i <= N - 1; i++)
                {
                    //релаксация
                    TT[i, j] = 0.25 * (T[i - 1, j] + T[i + 1, j] + T[i, j - 1] + T[i, j + 1]);
                    if (Math.Abs(TT[i, j] - T[i, j]) > Err1) { priznak = false; }
                }
            }
            showpic();
            textBoxitercount.Text = Convert.ToString(IterCount);
            if (priznak == false) textBoxerror.BackColor = Color.Red;
            else { textBoxerror.BackColor = Color.White; }

            for (int j = 1; j <= N - 1; j++)
            {
                for (int i = 1; i <= N - 1; i++)
                {
                    T[i, j] = TT[i, j];
                }
            }
        }

        private void textBoxN_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
