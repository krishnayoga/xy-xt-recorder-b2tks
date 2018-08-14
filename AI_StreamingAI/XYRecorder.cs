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

        double[] m_dataScaled;
        bool m_isFirstOverRun = true;
        double m_xInc;
        int dataCount = 0;
        double last_x_0;
        double last_x_1;
        bool firstChecked = true;
        string[] arrAvgData;
        string[] arrData;
        double[] arrSumData;
        double[] dataPrint;
        double max_x_1 = 0;
        double min_x_1 = 1000;
        double max_x_2 = 0;
        double min_x_2 = 1000;
        double max_y = 0;
        double min_y = 1000;
        int factor_baca_x_1 = 1, factor_baca_x_2 = 1, factor_baca_y = 1;
        int max_x_chart;
        int min_x_chart;
        int max_y_chart;
        int min_y_chart;

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

            dataPrint = new double[3];

            this.Text = "Streaming AI(" + waveformAiCtrl1.SelectedDevice.Description + ")";

            button_start.Enabled = true;
            button_stop.Enabled = false;
            button_pause.Enabled = false;

            chartXY.Series[0].IsXValueIndexed = false;
        }

        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
		        MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString(), "StreamingAI");
            }
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
                        //ValueX1.Text = arrAvgData[0];
                        //ValueY.Text = arrAvgData[1];
                        //label3.Text = arrAvgData[2];
                        //Console.WriteLine("i ke " + i + " arrsumdata :" + arrSumData[i]);
                        dataCount++;
                    }

                    dataPrint[0] = Convert.ToDouble(arrAvgData[0]) * factor_baca_x_1;
                    dataPrint[1] = Convert.ToDouble(arrAvgData[1]) * factor_baca_x_2;
                    dataPrint[2] = Convert.ToDouble(arrAvgData[2]) * factor_baca_y;

                    if (checkBox_invertX1.Checked)
                    {
                        dataPrint[0] = -dataPrint[0];
                    }
                    if (checkBox_invertX2.Checked)
                    {
                        dataPrint[1] = -dataPrint[1];
                    }
                    if (checkBox_invertY.Checked)
                    {
                        dataPrint[2] = -dataPrint[2];
                    }

                    ValueX1.Text = dataPrint[0].ToString();
                    ValueX2.Text = dataPrint[1].ToString();
                    ValueY.Text = dataPrint[2].ToString();

                    //channel 0
                    if (dataPrint[0] > max_x_1)
                    {
                        max_x_1 = dataPrint[0];
                    }

                    if (dataPrint[0] < min_x_1)
                    {
                        min_x_1 = dataPrint[0];
                    }
                    
                    //channel 1
                    if (dataPrint[1] > max_x_2)
                    {
                        max_x_2 = dataPrint[1];
                    }

                    if (dataPrint[1] < min_x_2)
                    {
                        min_x_2 = dataPrint[1];
                    }

                    //channel 2
                    if (dataPrint[2] > max_y)
                    {
                        max_y = dataPrint[2];
                    }

                    if (dataPrint[2] < min_y)
                    {
                        min_y = dataPrint[2];
                    }

                    //chartXY.Series[0].Points.AddXY(arrAvgData[0], arrAvgData[1]);

                    Console.WriteLine(max_x_2);

                    MaxX1.Text = max_x_1.ToString();
                    MinX1.Text = min_x_1.ToString();
                    MaxX2.Text = max_x_2.ToString();
                    minX2.Text = min_x_2.ToString();
                    MaxY.Text = max_y.ToString();
                    MinY.Text = min_y.ToString();

                    if (checkBox_holdX.Checked && firstChecked)
                    {
                        last_x_0 = dataPrint[0];
                        last_x_1 = dataPrint[1];
                        //last_x = dataCount.ToString();
                        firstChecked = false;
                    }

                    plotChart(dataPrint);
                    
                }));
                Console.WriteLine(dataCount / 3);
                
            }
            catch
            {
                MessageBox.Show("nilai x dan y salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }   
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

        private void startChart()
        {
            chartXY.Series.Clear();
            chartXY.Series.Add("Series 1");
            chartXY.Series.Add("Series 2");
            chartXY.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartXY.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

        }

        private void initChart()
        {
            max_x_chart = Convert.ToInt32(rangeX_chart.Text);
            min_x_chart = -max_x_chart;
            max_y_chart = Convert.ToInt32(rangeY_chart.Text);
            min_y_chart = -max_y_chart;

            
            //this.chartXY.Titles.Add("pt. B2TKS - BPPT");

            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = min_x_chart;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = 1;
            chartXY.ChartAreas[0].AxisY.Interval = 1;
            
            chartXY.ChartAreas[0].AxisX.Title = "";
            chartXY.ChartAreas[0].AxisY.Title = "";
            
        }

        private void plotChart(double[] data)
        {
            /*
            if (checkBox2.Checked)
            {
                dataPrint[0] = -(Convert.ToDouble(arrAvgData[0]));
                Console.WriteLine("halo" + dataPrint[0]);
                last_x_0 = -last_x_0;
            }
            if (checkBox3.Checked)
            {
                dataPrint[1] = -(Convert.ToDouble(arrAvgData[1]));
                last_x_1 = -last_x_1;
            }
            if (checkBox1.Checked)
            {
                dataPrint[2] = -(Convert.ToDouble(arrAvgData[2]));
            }
            */

            if (!checkBox_holdX.Checked)
            {
                if (check1.Checked)
                {
                    chartXY.Series[0].Points.AddXY(dataPrint[0], dataPrint[2]);
                }
                if (check2.Checked)
                {
                    chartXY.Series[1].Points.AddXY(dataPrint[1], dataPrint[2]);
                }
                firstChecked = true;
            }

            if (checkBox_holdX.Checked)
            {
                if (check1.Checked)
                {
                    chartXY.Series[0].Points.AddXY(last_x_0, dataPrint[2]);
                }
                if (check2.Checked)
                {
                    chartXY.Series[1].Points.AddXY(last_x_1, dataPrint[2]);
                }
            }
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

            if (check1.Checked)
            {
                factor_baca_x_1 = Convert.ToInt32(factor_x_1.Text);
            }
            if (check2.Checked)
            {
                factor_baca_x_2 = Convert.ToInt32(factor_x_2.Text);
            }
            
            factor_baca_y = Convert.ToInt32(factor_y.Text);

            startChart();
            initChart();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) //pause
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e) //stop
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

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartXY.SaveImage("D:\\chart.png", ChartImageFormat.Png);
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

        //fungsi untuk menu balance
        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (check1.Checked)
            {
                chartXY.Series[0].Points.Clear();
            }
            if (check2.Checked)
            {
                chartXY.Series[1].Points.Clear();
            }
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);
        }

        //fungsi untuk menu replot
        private void replotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initChart();
        }

        //fungsi untuk tombol update
        private void button1_Click(object sender, EventArgs e)
        {
            TitleMain.Text = Title.Text;
            ConsumerMain.Text = Consumer.Text;
            SenseMain.Text = Sense1.Text + " dan " + Sense2.Text + " vs " + Sense3.Text;

        }













        #region void kosong
        private void Consumer_TextChanged(object sender, EventArgs e)
        {

        }  
        private void button_start_Click(object sender, EventArgs e)
        {

        }
        private void button_pause_Click(object sender, EventArgs e)
        {

        }
        private void button_stop_Click(object sender, EventArgs e)
        {

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
        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void button_save_Click(object sender, EventArgs e)
        {

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
        private void ValX1_Click(object sender, EventArgs e)
        {

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
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
    }
}