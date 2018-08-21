﻿/*
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
    public partial class XTReport : Form
    {
        int i, j;
        double[] dataY1;
        double[] dataY2;
        int jumlah_data;
        int max_x_chart;
        int min_x_chart;
        int max_y_chart;
        int min_y_chart;
        double label_chart_1, label_chart_2, label_chart_3, label_chart_4, label_chart_5, label_chart_6;
        double label_chart_7, label_chart_8, label_chart_9, label_chart_10, label_chart_11;
        double pos_label_1, pos_label_2, pos_label_3, pos_label_4, pos_label_5, pos_label_6;
        double pos_label_7, pos_label_8, pos_label_9, pos_label_10, pos_label_11;

        private void File_Click(object sender, EventArgs e)
        {

        }

        private void TitleMain_Click(object sender, EventArgs e)
        {

        }

        int batas_chart_1, batas_chart_2, batas_chart_3, batas_chart_4, batas_chart_5, batas_chart_6;
        int batas_chart_7, batas_chart_8, batas_chart_9, batas_chart_10, batas_chart_11;

        double tanggal, jam, elapsed_time;

















        public XTReport()
        {
            InitializeComponent();
        }

        private void XTRec_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            }
            catch
            {
                MessageBox.Show("Error open file");
            }
            

            load_judul();

            
            /*
            res.Cells[10, 2] = MaxY1.Text;
            res.Cells[11, 2] = MinY1.Text;
            res.Cells[12, 2] = MaxY2.Text;
            res.Cells[13, 2] = MinY2.Text;
            res.Cells[14, 2] = Time.Text;
            
            res.Columns.AutoFit();
            book.SaveAs(File.Text);
            */
            
            
        }

        private void load_judul()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            TitleMain.Text = Convert.ToString(res.Cells[1, 2].Value);
            ConsumerMain.Text = Convert.ToString(res.Cells[2, 2].Value);
            SenseMain.Text = Convert.ToString(res.Cells[3, 2].Value);

            MaxY1.Text = Convert.ToString(res.Cells[10, 2].Value);
            MinY1.Text = Convert.ToString(res.Cells[11, 2].Value);
            MaxY2.Text = Convert.ToString(res.Cells[12, 2].Value);
            MinY2.Text = Convert.ToString(res.Cells[13, 2].Value);

            //tanggal = double.Parse(Convert.ToString(res.Cells[4, 2].Value));
            jam = double.Parse(Convert.ToString(res.Cells[5, 2].Value));
            elapsed_time = double.Parse(Convert.ToString(res.Cells[14, 2].Value));

            Console.WriteLine("tanggal: " + tanggal + " jam: " + jam + " elapsed_time: " + elapsed_time);


            Date.Text = Convert.ToString(res.Cells[4, 2].Value);
            DateTime jam_text = DateTime.FromOADate(jam);
            DateTime elapsed_time_text = DateTime.FromOADate(elapsed_time);

            Waktu.Text = Convert.ToString(jam_text);
            Time.Text = Convert.ToString(elapsed_time);
            
            jumlah_data = 160;

            book.Close();
            ex.Quit();
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataY1 = new double[jumlah_data];
            dataY2 = new double[jumlah_data];

            load_data();
            
            for (i = 0; i < jumlah_data; i++)
            {
                //Console.WriteLine("data ke: "+i+" dataY1: " + dataY1[i] + " dataY2: " + dataY2[i]);
            }
            
            init_chart();
            plot_chart();

            plot_data();
        }

        private void load_data()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            for (i = 16; i < (jumlah_data+16); i++)
            {
                dataY1[i - 16] = Convert.ToDouble(res.Cells[i, 3].Value);
                dataY2[i - 16] = Convert.ToDouble(res.Cells[i, 4].Value);

                //Console.WriteLine("i ke- " + i + " dataY1: " + dataY1[i - 16] + " dataY2: " + dataY2[i - 16]);
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

            max_x_chart = Convert.ToInt32(comboBox_MaxX.Text) * 61 * 9 + 1;
            min_x_chart = -max_x_chart; //ini nani digantiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii
            max_y_chart = Convert.ToInt32(comboBox_MaxY.Text);
            min_y_chart = Convert.ToInt32(comboBox_MinY.Text);

            Console.WriteLine(max_x_chart + "    " + min_x_chart + "   " + max_y_chart + "   " + min_y_chart);

            chartXY.ChartAreas[0].AxisY.Crossing = 0;

            chartXY.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 1000000000;
            chartXY.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            
            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = 0;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = max_x_chart / 10;
            chartXY.ChartAreas[0].AxisY.Interval = max_y_chart / 10;

            label_chart_1 = Convert.ToDouble(max_x_chart) * 0;
            label_chart_2 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10;
            label_chart_3 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 2;
            label_chart_4 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 3;
            label_chart_5 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 4;
            label_chart_6 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 5;
            label_chart_7 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 6;
            label_chart_8 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 7;
            label_chart_9 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 8;
            label_chart_10 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9 / 10 * 9;
            label_chart_11 = (Convert.ToDouble(max_x_chart) - 1) / 61 / 9;


            pos_label_1 = max_x_chart * 0;
            pos_label_2 = max_x_chart / 10;
            pos_label_3 = max_x_chart / 10 * 2;
            pos_label_4 = max_x_chart / 10 * 3;
            pos_label_5 = max_x_chart / 10 * 4;
            pos_label_6 = max_x_chart / 10 * 5;
            pos_label_7 = max_x_chart / 10 * 6;
            pos_label_8 = max_x_chart / 10 * 7;
            pos_label_9 = max_x_chart / 10 * 8;
            pos_label_10 = max_x_chart / 10 * 9;
            pos_label_11 = max_x_chart;

            batas_chart_1 = 1;
            batas_chart_2 = Convert.ToInt32(pos_label_2) / 10 * 3;
            batas_chart_3 = Convert.ToInt32(pos_label_3) / 10 * 3;
            batas_chart_4 = Convert.ToInt32(pos_label_4) / 10 * 3;
            batas_chart_5 = Convert.ToInt32(pos_label_5) / 10 * 3;
            batas_chart_6 = Convert.ToInt32(pos_label_6) / 10 * 3;
            batas_chart_7 = Convert.ToInt32(pos_label_7) / 10 * 3;
            batas_chart_8 = Convert.ToInt32(pos_label_8) / 10 * 3;
            batas_chart_9 = Convert.ToInt32(pos_label_9) / 10 * 3;
            batas_chart_10 = Convert.ToInt32(pos_label_10) / 10 * 3;
            batas_chart_11 = Convert.ToInt32(pos_label_11) / 10 * 3; ;
            
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_1 - batas_chart_1, pos_label_1 + batas_chart_1, label_chart_1.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_2 - batas_chart_2, pos_label_2 + batas_chart_2, label_chart_2.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_3 - batas_chart_3, pos_label_3 + batas_chart_3, label_chart_3.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_4 - batas_chart_4, pos_label_4 + batas_chart_4, label_chart_4.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_5 - batas_chart_5, pos_label_5 + batas_chart_5, label_chart_5.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_6 - batas_chart_6, pos_label_6 + batas_chart_6, label_chart_6.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_7 - batas_chart_7, pos_label_7 + batas_chart_7, label_chart_7.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_8 - batas_chart_8, pos_label_8 + batas_chart_8, label_chart_8.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_9 - batas_chart_9, pos_label_9 + batas_chart_9, label_chart_9.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_10 - batas_chart_10, pos_label_10 + batas_chart_10, label_chart_10.ToString("F1"), 1, LabelMarkStyle.None);
            chartXY.ChartAreas[0].AxisX.CustomLabels.Add(pos_label_11 - batas_chart_11, pos_label_11 + batas_chart_11, label_chart_11.ToString("F1"), 1, LabelMarkStyle.None);
            //itu

        }

        private void plot_data()
        {
            for (i = 0; i < jumlah_data; i++)
            {
                chartXY.Series[0].Points.AddXY(i, dataY1[i]);
                chartXY.Series[1].Points.AddXY(i, dataY2[i]);


            }
            /*
            chartXY.Series[0].Points.AddXY(1, 2);
            chartXY.Series[0].Points.AddXY(2, 3);
            chartXY.Series[0].Points.AddXY(3, 4);
            chartXY.Series[0].Points.AddXY(4, 5);
            chartXY.Series[0].Points.AddXY(5, 6);
            */
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
