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
using System.Drawing.Imaging;
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
        int batas_chart_1, batas_chart_2, batas_chart_3, batas_chart_4, batas_chart_5, batas_chart_6;
        int batas_chart_7, batas_chart_8, batas_chart_9, batas_chart_10, batas_chart_11;

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void ValY2_Click(object sender, EventArgs e)
        {

        }

        double tanggal, jam, elapsed_time;

        DateTime datee = new DateTime();

        public XTReport()
        {
            InitializeComponent();
        }

        private void XTRec_Load(object sender, EventArgs e)
        {

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
                MessageBox.Show("Error open file");
            }
            
        }

        private void load_judul()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            TitleMain.Text = Convert.ToString(res.Cells[1, 2].Value+res.Cells[1,3].Value);
            ConsumerMain.Text = Convert.ToString(res.Cells[2, 2].Value);
            SenseMain.Text = Convert.ToString(res.Cells[3, 2].Value);

            ValY1.Text = Convert.ToString(res.Cells[6, 2].Value);
            U1.Text = Convert.ToString(res.Cells[7, 2].Value);
            ValY2.Text = Convert.ToString(res.Cells[8, 2].Value);
            U2.Text = Convert.ToString(res.Cells[9, 2].Value);
          
            MaxY1.Text = Convert.ToString(res.Cells[11, 2].Value);
            MinY1.Text = Convert.ToString(res.Cells[12, 2].Value);
            MaxY2.Text = Convert.ToString(res.Cells[13, 2].Value);
            MinY2.Text = Convert.ToString(res.Cells[14, 2].Value);

            datee = Convert.ToDateTime(res.Cells[4, 2].Value);
            Date.Text = datee.ToString("dd/MM/yyyy");

            jam = double.Parse(Convert.ToString(res.Cells[5, 2].Value));
            DateTime jam_text = DateTime.FromOADate(jam);
            Waktu.Text = jam_text.ToLongTimeString();

            elapsed_time = double.Parse(Convert.ToString(res.Cells[15, 2].Value));
            DateTime elapsed_time_text = DateTime.FromOADate(elapsed_time);
            Time.Text = elapsed_time_text.ToString("HH:mm:ss:fff");

            jumlah_data = Convert.ToInt32(res.Cells[10,4].Value);

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

            for (i = 17; i < (jumlah_data+17); i++)
            {
                dataY1[i - 17] = Convert.ToDouble(res.Cells[i, 3].Value);
                dataY2[i - 17] = Convert.ToDouble(res.Cells[i, 4].Value);

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

            chartXY.Series[0].Color = Color.Red;
            chartXY.Series[1].Color = Color.Blue;

            chartXY.ChartAreas[0].AxisX.Crossing = 0;
            chartXY.ChartAreas[0].AxisY.Crossing = 0;
        }

        private void plot_chart()
        {

            max_x_chart = Convert.ToInt32(comboBox_MaxX.Text) * 61 * 9 + 20;
            min_x_chart = -max_x_chart;
            max_y_chart = Convert.ToInt32(comboBox_MaxY.Text);
            min_y_chart = Convert.ToInt32(comboBox_MinY.Text);

            //Console.WriteLine(max_x_chart + "    " + min_x_chart + "   " + max_y_chart + "   " + min_y_chart);

            //chartXY.ChartAreas[0].AxisY.Crossing = 0;

            chartXY.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 1000000000;
            chartXY.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            
            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = 0;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = max_x_chart / 10;
            chartXY.ChartAreas[0].AxisY.Interval = max_y_chart / 10;

            label_chart_1 = Convert.ToDouble(max_x_chart) * 0;
            label_chart_2 = (Convert.ToDouble(max_x_chart)-20) / 61 / 9 / 10;
            label_chart_3 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 2;
            label_chart_4 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 3;
            label_chart_5 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 4;
            label_chart_6 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 5;
            label_chart_7 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 6;
            label_chart_8 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 7;
            label_chart_9 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 8;
            label_chart_10 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10 * 9;
            label_chart_11 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9;

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
        }

        private void replotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartXY.ChartAreas[0].AxisX.CustomLabels.Clear();
            plot_chart();
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
            if (!Char.IsDigit(ch) && ch != 8 && ch !=45)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void comboBox_MinX_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void comboBox_MaxY_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printToPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //this.chartXY.SaveImage(File.Text + ".png", ChartImageFormat.Png);
                
                Bitmap printscreen = new Bitmap(1364, 723);

                Graphics graphics = Graphics.FromImage(printscreen as Image);

                graphics.CopyFromScreen(0, 50, 0, 90, printscreen.Size);

                printscreen.Save(File.Text+".png", ImageFormat.Png);

                MessageBox.Show("Sukses menyimpan chart", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan chart", "Save PNG Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printToPrinterToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXTReport helpXTReport = new HelpXTReport();
            helpXTReport.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void ValY1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void File_Click(object sender, EventArgs e)
        {

        }
        private void TitleMain_Click(object sender, EventArgs e)
        {

        }
    }
}
