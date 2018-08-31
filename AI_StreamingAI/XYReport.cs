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
 * Section Length = 512;
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
using System.Drawing.Imaging;

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
                if (open.ShowDialog() == DialogResult.OK)
                {
                    File.Text = open.FileName.ToString();
                    loadDataToolStripMenuItem.Enabled = true;
                    load_judul();
                }
                
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

            ValY.Text = Convert.ToString(res.Cells[6, 2].Value);
            U1.Text = Convert.ToString(res.Cells[7, 2].Value);
            ValX1.Text = Convert.ToString(res.Cells[8, 2].Value);
            U2.Text = Convert.ToString(res.Cells[9, 2].Value);
            ValX2.Text = Convert.ToString(res.Cells[8, 2].Value);
            U3.Text = Convert.ToString(res.Cells[9, 2].Value);

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

            chartXY.Series[0].Color = Color.Red;
            chartXY.Series[1].Color = Color.Blue;

            chartXY.ChartAreas[0].AxisX.Crossing = 0;
            chartXY.ChartAreas[0].AxisY.Crossing = 0;

        }
        
        private void plot_chart()
        {
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

        private void replotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartXY.ChartAreas[0].AxisX.CustomLabels.Clear();
            plot_chart();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXYReport helpXYReport = new HelpXYReport();
            helpXYReport.ShowDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToPNGToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //this.chartXY.SaveImage(File.Text + ".png", ChartImageFormat.Png);

                Bitmap printscreen = new Bitmap(1364, 725);

                Graphics graphics = Graphics.FromImage(printscreen as Image);

                graphics.CopyFromScreen(0, 40, 0, 60, printscreen.Size);

                printscreen.Save(File.Text + ".png", ImageFormat.Png);

                MessageBox.Show("Sukses menyimpan chart", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan chart", "Save PNG Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printToPrinterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(print_page);
            CaptureScreen();
            pd.Print();
        }

        Bitmap memoryImage;
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 50, 0, 90, s);
        }

        private void print_page(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
            /*
            int x = SystemInformation.WorkingArea.X;
            int y = SystemInformation.WorkingArea.Y;
            int width = this.Width;
            int height = this.Height;

            Rectangle bounds = new Rectangle(0, 0, 1500, 1000);

            Bitmap img = new Bitmap(1500, 1000);

            this.DrawToBitmap(img, bounds);
            Point p = new Point(0, 0);
            e.Graphics.DrawImage(img, p);

            /*
            System.Drawing.Font printFont = new System.Drawing.Font("Arial", 12);
            Rectangle myRec = new System.Drawing.Rectangle(30, 100, 800, 500);
            e.Graphics.DrawString(TitleMain.Text, printFont, Brushes.Black, 30, 40);
            e.Graphics.DrawString(ConsumerMain.Text, printFont, Brushes.Black, 30, 60);
            e.Graphics.DrawString(SenseMain.Text, printFont, Brushes.Black, 30, 80);
            chartXY.Printing.PrintPaint(e.Graphics, myRec);
            e.Graphics.DrawString("Garis biru: Sensor " + ValY1.Text, printFont, Brushes.Black, 30, 620);
            e.Graphics.DrawString("Garis merah: Sensor " + ValY2.Text, printFont, Brushes.Black, 30, 640);
            e.Graphics.DrawString("Tanggal pengujian: " + Date.Text, printFont, Brushes.Black, 30, 660);
            e.Graphics.DrawString("Waktu pengujian: " + Waktu.Text, printFont, Brushes.Black, 30, 680);
            */
        }
       
        private void SenseMain_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
