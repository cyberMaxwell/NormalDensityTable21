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

        private void FormLoad(object sender, EventArgs e)
        {
            //chart1.ChartAreas[0].
            chart1.Series[1].MarkerSize = 7;

            chart1.Series[2].Color = Color.Gray;
            chart1.Series[2].BorderWidth = 1;
            chart1.Series[2].BorderDashStyle = ChartDashStyle.Dot;

        }

        private void onCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            double x;
            bool isDouble = false;
            if (dataGridView.Rows[e.RowIndex].Cells[0].Value != null)
                isDouble = double.TryParse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), out _);
            else
                dataGridView.Rows[e.RowIndex].Cells[1].Value = null;




            if (!isDouble)
            {

                dataGridView.Rows[e.RowIndex].Cells[0].Value = null;
            }
            else
            {
                dataGridView.Rows[e.RowIndex].Cells[1].Value = f(Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value));

                x = Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value);




                chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
                chart1.Series[1].Color = Color.Red;
                chart1.Series[1].Points.Add(new DataPoint(x, f(x)));
                chart1.Series[2].Points.Add(new DataPoint(x, f(x)));
                chart1.Series[2].Points.Add(new DataPoint(0, f(x)));

            }

        }


        private void plot(object sender, PaintEventArgs e)
        {
            chart1.Series[0].Points.Clear();

            double a = -3, b = 4, h = 0.1, x, y;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 2;
            

            chart1.ChartAreas[0].AxisX.Minimum = -2;
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

        private void Button_click(object sender, EventArgs e)
        {

        }

        private void RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();

        }
    }
}
