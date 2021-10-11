using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            
            chart1.Series[1].Points.Clear();

            dataGridView.Columns[1].ReadOnly = true;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Crossing = 0;
            chart1.ChartAreas[0].AxisX.MajorTickMark.TickMarkStyle = TickMarkStyle.AcrossAxis;
            chart1.ChartAreas[0].AxisY.MajorTickMark.TickMarkStyle = TickMarkStyle.AcrossAxis;

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 2;

            chart1.ChartAreas[0].AxisX.Minimum = -2;
            chart1.ChartAreas[0].AxisX.Maximum = 4;
            chart1.ChartAreas[0].AxisY.Minimum = -10;
            chart1.ChartAreas[0].AxisY.Maximum = 10;

            chart1.Series[1].MarkerSize = 7;
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[1].Color = Color.Red;

            chart1.Series[2].Color = Color.Gray;
            chart1.Series[2].BorderWidth = 1;
            chart1.Series[2].ChartType = SeriesChartType.StepLine;
            chart1.Series[2].BorderDashStyle = ChartDashStyle.Dot;


        }

        private void onCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double x;
            bool isDouble = false;

            if (dataGridView.Rows[e.RowIndex].Cells[0].Value != null)
                isDouble = double.TryParse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString(), out _);

            if (!isDouble)
                dataGridView.Rows[e.RowIndex].Cells[0].Value = null;
            else
            {

                if (Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value) > 391)
                {
                    dataGridView.Rows[e.RowIndex].Cells[0].Value = null;
                    return;
                }

                dataGridView.Rows[e.RowIndex].Cells[1].Value = f(Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value));

                x = Convert.ToDouble(dataGridView.Rows[e.RowIndex].Cells[0].Value);

                chart1.Series[1].Points.Add(new DataPoint(x, f(x)));

                chart1.Series[2].Points.Add(new DataPoint(x, f(x)));
                chart1.Series[2].Points.Add(new DataPoint(0, f(x)));
                chart1.Series[2].Points.Add(new DataPoint(x, -10));
            }
        }

        private void plot(object sender, PaintEventArgs e)
        {

            chart1.Series[0].Points.Clear();


            double a = -3, b = 4, h = 0.1, x;


            x = a;
            while (x <= b)
            {
                chart1.Series[0].Points.AddXY(x, f(x));
                x += h;
            }
            menuStrip1.Update();
        }

        private void RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();
        }

        private void всплывающиеПодсказкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            всплывающиеПодсказкиToolStripMenuItem.Checked = !всплывающиеПодсказкиToolStripMenuItem.Checked;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void CreateMenu(object sender, EventArgs e)//нажатие на "создать"
        {
            dataGridView.Rows.Clear();
        }

        private void SaveDataMenu(object sender, EventArgs e)//сохранить данные DataGridView
        {
            SaveFileDialog save = new SaveFileDialog();

            double[,] masToSave = new double[dataGridView.Rows.Count, 2];

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                
                for (int j = 0; j < 2; j++)
                {
                    if (dataGridView.Rows[i].Cells[0].Value == null && dataGridView.Rows[i].Cells[1].Value == null)
                        continue;
                    else
                        masToSave[i, j] = Convert.ToDouble(dataGridView.Rows[i].Cells[j].Value);

                }
            }

            string saveStr = "";

            int newLine = 0;

            foreach (double a in masToSave)
            {
                if (newLine == 1)
                {
                    saveStr += a + "\n";
                    newLine = 0;
                }
                else
                {
                    saveStr += a + ";";
                    newLine++;
                }

            }
            if (saveStr.Length > 5)
            {
                
                saveStr = saveStr.Substring(0, saveStr.Length - 5);
                saveStr.Trim();

            }
            else
            {
                saveStr = "";
            }
            precisionLabel.Text = saveStr;

           // save.Filter = "Text File | *.txt";
           // save.FilterIndex = 2;
           //
           // if (save.ShowDialog() == DialogResult.OK)
           // {
           //     File.WriteAllText(save.FileName, saveStr);
           //    
           // }
            
            
        }
    }
}
