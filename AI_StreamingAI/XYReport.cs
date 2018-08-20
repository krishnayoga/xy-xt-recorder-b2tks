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
using Microsoft.CSharp.RuntimeBinder;

namespace AI_StreamingAI
{
    public partial class XYReport : Form
    {
        int i, j;
        double[] dataX1;
        double[] dataX2;
        double[] dataY;
        int jumlah_data;

        DateTime datee = new DateTime();

        public XYReport()
        {
            InitializeComponent();
        }

        private void fileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open File";
            open.Filter = "CSV Files (*.csv)|*.csv";
            open.ShowDialog();
            File.Text = open.FileName.ToString();
            //Date.Text = DateTime.Now.ToShortDateString();
            //Waktu.Text = DateTime.Now.ToLongTimeString();


            load_judul();

            dataX1 = new double[jumlah_data];
            dataX2 = new double[jumlah_data];
            dataY = new double[jumlah_data];

            
            load_data();

            plot_chart();
            

            //res.Columns.AutoFit();
            //book.SaveAs(File.Text);
            

            //button_start.Enabled = false;
            //button_pause.Enabled = false;
            //startRecordToolStripMenuItem.Enabled = true;
        }

        private void load_judul()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            TitleMain.Text = Convert.ToString(res.Cells[1, 2].Value);
            ConsumerMain.Text = Convert.ToString(res.Cells[2, 2].Value);
            SenseMain.Text = Convert.ToString(res.Cells[3, 2].Value);

            MaxX1.Text = Convert.ToString(res.Cells[12, 2].Value);
            MinX1.Text = Convert.ToString(res.Cells[13, 2].Value);
            MaxX2.Text = Convert.ToString(res.Cells[14, 2].Value);
            minX2.Text = Convert.ToString(res.Cells[15, 2].Value);
            MaxY.Text = Convert.ToString(res.Cells[10, 2].Value);
            MinY.Text = Convert.ToString(res.Cells[11, 2].Value);
            
            datee = Convert.ToDateTime(res.Cells[4, 2].Value);
            Waktu.Text = Convert.ToString(res.Cells[5, 2].Value);

            Date.Text = Convert.ToString(datee);

            jumlah_data = 1000;

            book.Close();
            ex.Quit();
        }

        private void load_data()
        {
            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;

            for (i = 16; i < jumlah_data; i++){
                dataX1[i-16] = Convert.ToDouble(res.Cells[i,2].Value);
                dataX2[i-16] = Convert.ToDouble(res.Cells[i, 3].Value);
                dataY[i-16] = Convert.ToDouble(res.Cells[i, 4].Value); 
            }

            book.Close();
            ex.Quit();
        }

        private void plot_chart()
        {

        }
    }
}
