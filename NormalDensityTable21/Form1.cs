using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NormalDensityTable21
{

    public partial class Form1 : Form
    {

        public static double f(double x)
        {
            return x * x * x - 3 * x * x - x + 3;
        }

        public Form1()
        {
            InitializeComponent();
            dataGridView.Columns.Add("X", "x");
            dataGridView.Columns.Add("Y", "f(x)");

        }


        private void onCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            double x, y;
            bool isDouble = double.TryParse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), out _);
            if (!isDouble)
            {

                dataGridView.Rows[e.RowIndex].Cells[0].Value = DBNull.Value;
            }
            else
            {
                dataGridView.Rows[e.RowIndex].Cells[1].Value = f(Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value));

                x = Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[1].Value);
                y = f(x);


                
                chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[1].Color = Color.Red;
                chart1.Series[1].Points.Add(new DataPoint(x, y));
             
            }

        }

        private void OnPlotClick(object sender, EventArgs e)
        {
           

        }

        private void on(object sender, EventArgs e)
        {

        }

        private void plot(object sender, PaintEventArgs e)
        {


            double a = -10, b = 10, h = 0.1, x, y;

            chart1.Series[0].Points.Clear();


            chart1.ChartAreas[0].AxisX.Minimum = -3;
            chart1.ChartAreas[0].AxisX.Maximum = 4;
            chart1.ChartAreas[0].AxisY.Minimum = -10;
            chart1.ChartAreas[0].AxisY.Maximum = 10;



            x = a;
            while (x <= b)
            {

                chart1.Series[0].Points.AddXY(x, f(x));


                x += h;
            }

        }

        private void tick(object sender, EventArgs e)
        {

        }

       
    }
}
