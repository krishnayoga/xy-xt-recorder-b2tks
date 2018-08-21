/*
 * Dibuat Oleh:
 * Ida Bagus Krishna Yoga Utama <email: hello@krishna.my.id >
 * Arbariyanto Mahmud Wicaksono <email: arbariyantom@gmail.com>
 * 
 * Teknik Elektro 2015
 * Departemen Teknik Elektro
 * Universitas Indonesia
 * 
 * Dibuat pada Agustus 2018
 * Untuk BPPT B2TKS Divisi SBPI
*/

/*
 * Configuration waveformAICtrl1:
 * Channel Count = 3;
 * Frequency (Convert Clock Rate) = 8000;
 * Section Length = 32;
 * 
 * Konfigurasi untuk sampling rate 10Hz (10 data per detik)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.DataVisualization;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using excel = Microsoft.Office.Interop.Excel;

namespace AI_StreamingAI
{
    public partial class XYReport : Form
    {
        int i, j;
        double[] dataX1;
        double[] dataX2;
        double[] dataY;
        double jam;
        int jumlah_data;
        int max_x_chart;
        int min_x_chart;
        int max_y_chart;
        int min_y_chart;
        

        DateTime datee = new DateTime();

        public XYReport()
        {
            InitializeComponent();
        }

        private void XYReport_Load(object sender, EventArgs e)
        {
            //init_chart();
        }

        private void fileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = "Open File";
                open.Filter = "CSV Files (*.csv)|*.csv";
                open.ShowDialog();
                File.Text = open.FileName.ToString();

                load_judul();
            }
            catch
            {
                MessageBox.Show("error!");
            }
            
            //Date.Text = DateTime.Now.ToShortDateString();
            //Waktu.Text = DateTime.Now.ToLongTimeString();

            //load_judul();
            
        }

        private void load_judul()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            TitleMain.Text = Convert.ToString(res.Cells[1, 2].Value);
            ConsumerMain.Text = Convert.ToString(res.Cells[2, 2].Value);
            SenseMain.Text = Convert.ToString(res.Cells[3, 2].Value);

            MaxX1.Text = Convert.ToString(res.Cells[13, 2].Value);
            MinX1.Text = Convert.ToString(res.Cells[14, 2].Value);
            MaxX2.Text = Convert.ToString(res.Cells[15, 2].Value);
            minX2.Text = Convert.ToString(res.Cells[16, 2].Value);
            MaxY.Text = Convert.ToString(res.Cells[11, 2].Value);
            MinY.Text = Convert.ToString(res.Cells[12, 2].Value);
            
            datee = Convert.ToDateTime(res.Cells[4, 2].Value);
            Date.Text = datee.ToString("dd/MM/yyyy");

            jam = double.Parse(Convert.ToString(res.Cells[5, 2].Value));
            DateTime jam_text = DateTime.FromOADate(jam);
            Waktu.Text = jam_text.ToLongTimeString();
            

            jumlah_data = Convert.ToInt32(res.Cells[10, 4].Value); ;
            /*
            chartXY.Series[0].Points.AddXY(1, 2);
            chartXY.Series[0].Points.AddXY(2, 3);
            chartXY.Series[0].Points.AddXY(3, 4);
            chartXY.Series[0].Points.AddXY(4, 5);
            chartXY.Series[0].Points.AddXY(5, 6);
            */
            book.Close();
            ex.Quit();


        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dataX1 = new double[jumlah_data];
            dataX2 = new double[jumlah_data];
            dataY = new double[jumlah_data];

            
            load_data();
            
            /*for (i = 0; i < jumlah_data; i++)
            {
                Console.WriteLine("data ke: "+i+" dataX1: " + dataX1[i] + " dataX2: " + dataX2[i] + " dataY: " + dataY[i]);
            }*/

            init_chart();
            plot_chart();

            plot_data();


            //res.Columns.AutoFit();
            //book.SaveAs(File.Text);


            //button_start.Enabled = false;
            //button_pause.Enabled = false;
            //startRecordToolStripMenuItem.Enabled = true;
        }

        private void load_data()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            for (i = 18; i < (jumlah_data+18); i++)
            {
                dataX1[i - 18] = Convert.ToDouble(res.Cells[i, 2].Value);
                dataX2[i - 18] = Convert.ToDouble(res.Cells[i, 3].Value);
                dataY[i - 18] = Convert.ToDouble(res.Cells[i, 4].Value);
            }

            book.Close();
            ex.Quit();
        }

        private void init_chart()
        {
            chartXY.Series.Clear();
            chartXY.Series.Add("Series 1");
            chartXY.Series.Add("Series 2");
            chartXY.Series[0].ChartType = SeriesChartType.Line;
            chartXY.Series[1].ChartType = SeriesChartType.Line;

            chartXY.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartXY.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            chartXY.Series[0].Color = Color.Blue;
            chartXY.Series[1].Color = Color.Red;
        }
        
        private void plot_chart()
        {
            chartXY.ChartAreas[0].AxisX.Crossing = 0;
            chartXY.ChartAreas[0].AxisY.Crossing = 0;

            max_x_chart = Convert.ToInt32(comboBox_MaxX.Text);
            min_x_chart = Convert.ToInt32(comboBox_MinX.Text);
            max_y_chart = Convert.ToInt32(comboBox_MaxY.Text);
            min_y_chart = Convert.ToInt32(comboBox_MinY.Text);

            Console.WriteLine(max_x_chart + "    " + min_x_chart + "   " + max_y_chart + "   " + min_y_chart);

            //this.chartXY.Titles.Add("pt. B2TKS - BPPT");
            
            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = min_x_chart;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = max_x_chart/10;
            chartXY.ChartAreas[0].AxisY.Interval = max_y_chart/10;

            //chartXY.ChartAreas[0].AxisX.Title = SensorX1.Text + " (" + UnitX1.Text + ")";
            //chartXY.ChartAreas[0].AxisY.Title = SensorY.Text + " (" + UnitY.Text + ")";
            
        }

        private void plot_data()
        {
            for (i = 0; i < jumlah_data; i++)
            {
                chartXY.Series[0].Points.AddXY(dataX1[i], dataY[i]);
                chartXY.Series[1].Points.AddXY(dataX2[i], dataY[i]);
                
                
            }
            /*
            chartXY.Series[0].Points.AddXY(1, 2);
            chartXY.Series[0].Points.AddXY(2, 3);
            chartXY.Series[0].Points.AddXY(3, 4);
            chartXY.Series[0].Points.AddXY(4, 5);
            chartXY.Series[0].Points.AddXY(5, 6);
            */
        }

        private void comboBox_MaxY_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void comboBox_MaxX_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void comboBox_MinY_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void comboBox_MinX_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
