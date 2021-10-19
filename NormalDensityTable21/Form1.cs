using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

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
            //chart1.Series[1].Points.Clear();


            dataGridView.Columns[1].ReadOnly = true;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Crossing = 0;
            chart1.ChartAreas[0].AxisY.Crossing = 0;
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



            BuildPlot();
        }


        private void RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();

        }



        private void CreateMenu(object sender, EventArgs e)//нажатие на "создать"
        {
            dataGridView.Rows.Clear();
        }

        private void SaveDataMenu(object sender, EventArgs e)//сохранить данные DataGridView
        {
            SaveFileDialog save = new SaveFileDialog();

            List<double> row = new List<double>();



            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {

                for (int j = 0; j < 2; j++)
                {
                    row.Add(Convert.ToDouble(dataGridView.Rows[i].Cells[j].Value));
                }
            }


            string saveStr = "";

            int newLine = 0;


            foreach (var a in row)
            {
                if (newLine == 1)
                {
                    saveStr += a.ToString() + "\n";
                    newLine = 0;
                }
                else
                {
                    saveStr += a.ToString() + ";";
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

            for (int i = 0; i < saveStr.Length; i++)
            {
                if (saveStr.Contains("0;0"))
                {
                    int indexZero = saveStr.IndexOf("0;0");
                    saveStr = saveStr.Remove(indexZero, 3);
                }
            }

            save.RestoreDirectory = true;
            save.DefaultExt = "txt";
            save.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save.FileName, saveStr);
            }
        }

        private void OnOpenDataClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            List<string> l = new List<string>();

            List<string> odd = new List<string>();
            List<string> even = new List<string>();


            string output = "";

            if (open.ShowDialog() == DialogResult.OK)
            {
                output = File.ReadAllText(open.FileName);
            }

            l.AddRange(output.Split(';', ' ', '\n'));

            for (int i = 0; i < l.Count; i++)
            {
                l.Remove("");

            }




            for (int i = 0; i < l.Count; i++)
            {
                if (i % 2 == 0)
                {
                    even.Add(l[i]);
                }
                else
                {
                    odd.Add(l[i]);
                }
            }


            for (int i = 0; i < even.Count; i++)
            {
                dataGridView.Rows.Add();


                dataGridView.Rows[i].Cells[0].Value = even[i];



            }
            for (int i = 0; i < odd.Count; i++)
            {

                dataGridView.Rows[i].Cells[1].Value = odd[i];


            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                Calculatefunc(i);
                PaintDots(i);
                dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();

            }
        }




        private void вычислятьНемедленноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CalculateImmediately.Checked = !CalculateImmediately.Checked;

            if (CalculateImmediately.Checked)
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    Plot(i);
                }

            }

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

        private void BuildPlot()
        {
            //chart1.Series[0].Points.Clear();


            double a = -3, b = 4, h = 0.1, x;

            x = a;
            while (x <= b)
            {
                chart1.Series[0].Points.AddXY(x, f(x));
                x += h;
            }
        }

        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //BuildPlot();
            if (CalculateImmediately.Checked)
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    Plot(i);
                }
                // Plot(e.RowIndex);

            }
        }



        private void OnRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RepaintDots();
        }

        private void RepaintDots()
        {

            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {

                PaintDots(i);
            }
        }



        private void Plot(int rowIndex)//построение точек и перпендикуляров на графике и вычисление значений функции в DataGridView
        {
            Calculatefunc(rowIndex);

            PaintDots(rowIndex);

        }

        private void Calculatefunc(int rowIndex)//вычисление значений функции в DataGridView
        {
            bool isDouble = false;
            if (dataGridView.Rows[rowIndex].Cells[0].Value != null)
                isDouble = double.TryParse(dataGridView.Rows[rowIndex].Cells[0].Value.ToString(), out _);
            else
            {
                dataGridView.Rows[rowIndex].Cells[0].Value = null;
                dataGridView.Rows[rowIndex].Cells[1].Value = null;

                RepaintDots();
            }

            if (!isDouble)
            {
                dataGridView.Rows[rowIndex].Cells[0].Value = null;
                dataGridView.Rows[rowIndex].Cells[1].Value = null;

            }
            else
            {
                if (Convert.ToDouble(dataGridView.Rows[rowIndex].Cells[0].Value) > 391)
                {
                    dataGridView.Rows[rowIndex].Cells[0].Value = null;
                    return;
                }


                dataGridView.Rows[rowIndex].Cells[1].Value = f(Convert.ToDouble(dataGridView.Rows[rowIndex].Cells[0].Value));

            }
        }

        private void CalculateFuncButton_Click(object sender, EventArgs e)//f(x) на панели инструментов
        {
            //BuildPlot();

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                Plot(i);
            }
        }

        private void PaintDots(int rowIndex)//рисует точки на графике
        {
            double x = 0;
            {//костыль, но бага то нет
                chart1.Series[2].Points.Add(new DataPoint(0, 200));
                chart1.Series[2].Points.Add(new DataPoint(200, 0));
            }

            bool isDouble = false;
            if (dataGridView.Rows[rowIndex].Cells[0].Value != null)
                isDouble = double.TryParse(dataGridView.Rows[rowIndex].Cells[0].Value.ToString(), out _);
            if (isDouble)
            {
                x = Convert.ToDouble(dataGridView.Rows[rowIndex].Cells[0].Value);
            }
            else return;

            bool isEmpty = false;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (Convert.ToDouble(dataGridView.Rows[rowIndex].Cells[0].Value) == 0 && Convert.ToDouble(dataGridView.Rows[rowIndex].Cells[1].Value) == 0)
                {
                    isEmpty = true;
                }
            }

            if (x == 0)
            {
                chart1.Series[1].Points.Add(new DataPoint(200, 200));

            }

            if (isEmpty)
            {
                return;
            }
            else
            {

                chart1.Series[1].Points.Add(new DataPoint(x, f(x)));

                chart1.Series[2].Points.Add(new DataPoint(0, f(x)));
                chart1.Series[2].Points.Add(new DataPoint(x, 0));
            }

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CopyTheChart(object sender, EventArgs e)//копирование графика в меню
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart1.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);

            }
        }

        private void SaveTheChart(object sender, EventArgs e)//сохранение графика в меню
        {

            SaveFileDialog save = new SaveFileDialog();
            save.RestoreDirectory = true;
            save.DefaultExt = "png";
            save.Filter = "Png files (*.png)|*.png|All files (*.*)|*.*";
            save.FilterIndex = 1;


            if (save.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(save.FileName, ChartImageFormat.Png);
            }
        }
    }
}
