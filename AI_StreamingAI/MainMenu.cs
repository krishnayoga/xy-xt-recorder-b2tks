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

namespace AI_StreamingAI
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void recorderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainHelp mainHelp = new MainHelp();
            mainHelp.ShowDialog();
        }

        private void xTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XTRecorder xtrec = new XTRecorder();
            xtrec.ShowDialog();
        }

        private void xYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYRecorder xyrec = new XYRecorder();
            xyrec.ShowDialog();
        }

        private void xTToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            XTReport xtreport = new XTReport();
            xtreport.ShowDialog();
        }

        private void xYToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            XYReport xyreport = new XYReport();
            xyreport.ShowDialog();
        }
    }
}
