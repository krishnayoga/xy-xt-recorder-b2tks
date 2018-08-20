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
    public partial class XTReport : Form
    {
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
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Open File";
            open.Filter = "CSV Files (*.csv)|*.csv";
            open.ShowDialog();
            File.Text = open.FileName.ToString();
            Date.Text = DateTime.Now.ToShortDateString();
            Waktu.Text = DateTime.Now.ToLongTimeString();

            excel.Application ex = new excel.Application();
            excel.Workbook book = ex.Workbooks.Open(File.Text);
            excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;
            /*
            res.Cells[10, 2] = MaxY1.Text;
            res.Cells[11, 2] = MinY1.Text;
            res.Cells[12, 2] = MaxY2.Text;
            res.Cells[13, 2] = MinY2.Text;
            res.Cells[14, 2] = Time.Text;
            */
            res.Columns.AutoFit();
            book.SaveAs(File.Text);
            book.Close();
            ex.Quit();

            //button_start.Enabled = false;
            //button_pause.Enabled = false;
            //startRecordToolStripMenuItem.Enabled = true;
        }
    }
}
