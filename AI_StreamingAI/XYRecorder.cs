using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.DataVisualization;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using Automation.BDaq;
using System.IO;

namespace AI_StreamingAI
{
    public partial class XYRecorder : Form
    {
        #region fields  
        double[]            m_dataScaled;
        bool                m_isFirstOverRun = true;
        double              m_xInc;
        int dataCount       = 0;
        string              last_x_0, last_x_1;
        bool firstChecked   = true;
        string[]            arrAvgData;
        string[]            arrData;
        double[]            arrSumData;
        double max_x        = 0;
        double min_x        = 1000;
        double max_y        = 0;
        double min_y        = 1000;
        int                 max_x_chart;
        int                 min_x_chart;
        int                 max_y_chart;
        int                 min_y_chart;
        #endregion

        public XYRecorder()
        {
            InitializeComponent();
        }

        public XYRecorder(int deviceNumber)
        {
            InitializeComponent();
			waveformAiCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
        }
      
        private void StreamingBufferedAiForm_Load(object sender, EventArgs e)
        {
            if (!waveformAiCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StreamingAI");
                this.Close();
                return;
            }

		    int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
		    int sectionLength = waveformAiCtrl1.Record.SectionLength;
		    m_dataScaled = new double[chanCount * sectionLength];

		    this.Text = "Streaming AI(" + waveformAiCtrl1.SelectedDevice.Description + ")";

            button_start.Enabled = true;
            button_stop.Enabled = false;
            button_pause.Enabled = false;

            //chartXY.Series[0].BorderWidth = 10;
            chartXY.Series[0].IsXValueIndexed = false;
            initChart();
        }

        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
		        MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "StreamingAI");
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            
        }

	    private void waveformAiCtrl1_DataReady(object sender, BfdAiEventArgs args)
        {
	        try
            {
                if (waveformAiCtrl1.State == ControlState.Idle)
                {
				    return;
                }
                if (m_dataScaled.Length < args.Count)
                {
                    m_dataScaled = new double[args.Count];
                }

                //Console.WriteLine(args.Count);

                ErrorCode err = ErrorCode.Success;
				int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
				int sectionLength = waveformAiCtrl1.Record.SectionLength;
                err = waveformAiCtrl1.GetData(args.Count, m_dataScaled);

                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
                //System.Diagnostics.Debug.WriteLine(args.Count.ToString());

                this.Invoke(new Action(() =>
                {
                    arrSumData = new double[chanCount];
                    //listViewAi.BeginUpdate();

                    for (int i = 0; i < sectionLength; i++)
                    {
                        arrData = new string[chanCount];
                        for (int j = 0; j < chanCount; j++)
                        {
                            int cnt = i * chanCount + j;
                            arrData[j] = m_dataScaled[cnt].ToString("F1");
                            arrSumData[j] += m_dataScaled[cnt];
                            //Console.WriteLine("j ke " + j + " arrsumdata :" + arrSumData[j] + " m_datascaled: " + m_dataScaled[cnt] + " cnt: " + cnt + " chancount: " + chanCount);
                        }
                        //addListViewItems(listViewAi, arrData);
                    }
                    arrAvgData = new string[arrSumData.Length];

                    for (int i = 0; i < arrSumData.Length; i++)
                    {
                        arrAvgData[i] = (arrSumData[i] / sectionLength).ToString("F1");
                        ValueX1.Text = arrAvgData[0];
                        ValueY.Text = arrAvgData[1];
                        //label3.Text = arrAvgData[2];
                        //Console.WriteLine("i ke " + i + " arrsumdata :" + arrSumData[i]);
                        dataCount++;

                        if (Convert.ToDouble(arrAvgData[0]) > max_x)
                        {
                            max_x = Convert.ToDouble(arrAvgData[0]);
                        }
                        if(Convert.ToDouble(arrAvgData[0]) < min_x)
                        {
                            min_x = Convert.ToDouble(arrAvgData[0]);
                        }
                        if(Convert.ToDouble(arrAvgData[1]) > max_y)
                        {
                            max_y = Convert.ToDouble(arrAvgData[1]);
                        }
                        if(Convert.ToDouble(arrAvgData[1]) < min_y)
                        {
                            min_y = Convert.ToDouble(arrAvgData[1]);
                        }
                        //chartXY.Series[0].Points.AddXY(arrAvgData[0], arrAvgData[1]);
                        
                        MaxX1.Text = max_x.ToString();
                        MinX1.Text = min_x.ToString();
                        MaxY.Text = max_y.ToString();
                        MinY.Text = min_y.ToString();
                        
                    }
                    if(checkBox_holdX.Checked && firstChecked)
                    {
                        last_x_0 = arrAvgData[0];
                        last_x_1 = arrAvgData[1];
                        //last_x = dataCount.ToString();
                        firstChecked = false;
                    }
                    plotChart(arrAvgData);
                }));
                Console.WriteLine(dataCount / 3);
                
            }
            catch
            {
                MessageBox.Show("nilai x dan y salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }   
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
         
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
			
        }
     
		private void waveformAiCtrl1_CacheOverflow(object sender, BfdAiEventArgs e)
        {
            MessageBox.Show("WaveformAiCacheOverflow");
        }

        private void waveformAiCtrl1_Overrun(object sender, BfdAiEventArgs e)
        {
            if (m_isFirstOverRun)
            {
                MessageBox.Show("WaveformAiOverrun");
                m_isFirstOverRun = false;
            }
        }

        private void initChart()
        {    
            chartXY.Series.Clear();
            chartXY.Series.Add("Series 1");
            chartXY.Series.Add("Series 2");
            chartXY.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartXY.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //this.chartXY.Titles.Add("pt. B2TKS - BPPT");

            chartXY.ChartAreas[0].AxisX.Maximum = 10;
            chartXY.ChartAreas[0].AxisX.Minimum = 0;
            chartXY.ChartAreas[0].AxisY.Maximum = 10;
            chartXY.ChartAreas[0].AxisY.Minimum = 0;
            chartXY.ChartAreas[0].AxisX.Interval = 1;
            chartXY.ChartAreas[0].AxisY.Interval = 1;
            
            chartXY.ChartAreas[0].AxisX.Title = "";
            chartXY.ChartAreas[0].AxisY.Title = "";
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void plotChart(string[] data)
        {
            if (!checkBox_holdX.Checked)
            {
                chartXY.Series[0].Points.AddXY(Convert.ToDouble(arrAvgData[0]), Convert.ToDouble(arrAvgData[2]));
                chartXY.Series[1].Points.AddXY(Convert.ToDouble(arrAvgData[1]), Convert.ToDouble(arrAvgData[2]));
                firstChecked = true;
            }
            if (checkBox_holdX.Checked)
            {
                chartXY.Series[0].Points.AddXY(Convert.ToDouble(last_x_0), Convert.ToDouble(arrAvgData[2]));
                chartXY.Series[1].Points.AddXY(Convert.ToDouble(last_x_1), Convert.ToDouble(arrAvgData[2]));
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;

            err = waveformAiCtrl1.Prepare();
            m_xInc = 1.0 / waveformAiCtrl1.Conversion.ClockRate;
            if (err == ErrorCode.Success)
            {
                err = waveformAiCtrl1.Start();
                Console.WriteLine("halooo");
                //waveformAiCtrl1.DataReady += new EventHandler<BfdAiEventArgs>(waveformAiCtrl1_DataReady);
                //chartXY.Series[0].Points.AddXY(arrAvgData[1], arrAvgData[0]);
            }

            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            button_start.Enabled = false;
            button_pause.Enabled = true;
            button_stop.Enabled = true;
        }

        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            button_start.Enabled = true;
            button_pause.Enabled = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_stop.Enabled = false;
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartXY.SaveImage("D:\\chart.png", ChartImageFormat.Png);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_holdX_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void SensorY_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValY.Text = SensorY.Text;
            UnitY.Items.Clear();
            switch (SensorY.Text)
            {
                case "Load Cell":
                    UnitY.Items.Add("kg");
                    UnitY.Items.Add("N");
                    UnitY.Items.Add("kN");
                    UnitY.Items.Add("Ton");
                    break;
                case "LVDT":
                    UnitY.Items.Add("cm");
                    UnitY.Items.Add("mm");
                    break;
                case "SG":
                    UnitY.Items.Add("uS");
                    break;
                case "Pressure":
                    UnitY.Items.Add("Kg/cm2");
                    UnitY.Items.Add("Mpa");
                    UnitY.Items.Add("Psi");
                    UnitY.Items.Add("Bar");
                    break;
                case "Volt":
                    UnitY.Items.Add("V");
                    UnitY.Items.Add("mV");
                    break;
            }
        }

        private void ValX1_Click(object sender, EventArgs e)
        {

        }

        private void fileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save File";
            save.Filter = "CSV Files (*.csv)|*.csv|Text Files(*.txt)|*.txt";
            save.ShowDialog();
            File.Text = save.FileName.ToString();
            Date.Text = DateTime.Now.ToShortDateString();
            Waktu.Text = DateTime.Now.ToLongTimeString();
        }

        private void SensorX1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValX1.Text = SensorX1.Text;
            UnitX1.Items.Clear();
            switch (SensorX1.Text)
            {
                case "Load Cell":
                    UnitX1.Items.Add("kg");
                    UnitX1.Items.Add("N");
                    UnitX1.Items.Add("kN");
                    UnitX1.Items.Add("Ton");
                    break;
                case "LVDT":
                    UnitX1.Items.Add("cm");
                    UnitX1.Items.Add("mm");
                    break;
                case "SG":
                    UnitX1.Items.Add("uS");
                    break;
                case "Pressure":
                    UnitX1.Items.Add("Kg/cm2");
                    UnitX1.Items.Add("Mpa");
                    UnitX1.Items.Add("Psi");
                    UnitX1.Items.Add("Bar");
                    break;
                case "Volt":
                    UnitX1.Items.Add("V");
                    UnitX1.Items.Add("mV");
                    break;
            }
        }

        private void SensorX2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValX2.Text = SensorX2.Text;
            UnitX2.Items.Clear();
            switch (SensorX2.Text)
            {
                case "Load Cell":
                    UnitX2.Items.Add("kg");
                    UnitX2.Items.Add("N");
                    UnitX2.Items.Add("kN");
                    UnitX2.Items.Add("Ton");
                    break;
                case "LVDT":
                    UnitX2.Items.Add("cm");
                    UnitX2.Items.Add("mm");
                    break;
                case "SG":
                    UnitX2.Items.Add("uS");
                    break;
                case "Pressure":
                    UnitX2.Items.Add("Kg/cm2");
                    UnitX2.Items.Add("Mpa");
                    UnitX2.Items.Add("Psi");
                    UnitX2.Items.Add("Bar");
                    break;
                case "Volt":
                    UnitX2.Items.Add("V");
                    UnitX2.Items.Add("mV");
                    break;
            }
        }

        private void UnitY_SelectedIndexChanged(object sender, EventArgs e)
        {
            U1.Text = UnitY.Text;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ValX2_Click(object sender, EventArgs e)
        {

        }

        private void ValueX2_TextChanged(object sender, EventArgs e)
        {

        }

        private void UnitX1_SelectedIndexChanged(object sender, EventArgs e)
        {
            U2.Text = UnitX1.Text;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXYRec helpxy = new HelpXYRec();
            helpxy.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UnitX2_SelectedIndexChanged(object sender, EventArgs e)
        {
            U3.Text = UnitX2.Text;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TitleMain.Text = Title.Text;
            ConsumerMain.Text = Consumer.Text;
            SenseMain.Text = Sense1.Text + " dan " + Sense2.Text + " vs " + Sense3.Text;

        }

        private void Consumer_TextChanged(object sender, EventArgs e)
        {

        }
    }
}