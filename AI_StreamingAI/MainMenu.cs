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
    }
}
