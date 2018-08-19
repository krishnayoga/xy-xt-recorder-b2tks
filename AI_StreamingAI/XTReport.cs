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
    public partial class XTRec : Form
    {
        public XTRec()
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
        }
    }
}
