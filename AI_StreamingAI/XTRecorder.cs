using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automation.BDaq;

namespace AI_StreamingAI
{
    public partial class XTRecorder : Form
    {
        #region fields  
        double[]     m_dataScaled;
        bool         m_isFirstOverRun = true;
        double       m_xInc;
        string[]    arrAvgData;
        string[]    arrData;
        double[]    arrSumData;
        #endregion

        public XTRecorder()
        {
            InitializeComponent();             
        }

        public XTRecorder(int deviceNumber)
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
            ErrorCode err = ErrorCode.Success;

			err = waveformAiCtrl1.Prepare();
            m_xInc = 1.0 / waveformAiCtrl1.Conversion.ClockRate;
            if (err == ErrorCode.Success)
            {
		        err = waveformAiCtrl1.Start();
                //waveformAiCtrl1.DataReady += new EventHandler<BfdAiEventArgs>(waveformAiCtrl1_DataReady);
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

		private void waveformAiCtrl1_DataReady(object sender, BfdAiEventArgs args)
        {
            Console.WriteLine("halooooooooooooooooooooo");
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

                ErrorCode err = ErrorCode.Success;
				int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
				int sectionLength = waveformAiCtrl1.Record.SectionLength;
                err = waveformAiCtrl1.GetData(args.Count, m_dataScaled);

                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    HandleError(err);
                    return;
                }
                System.Diagnostics.Debug.WriteLine(args.Count.ToString());

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
                            arrData[j] = m_dataScaled[cnt].ToString("F4");
                            arrSumData[j] += m_dataScaled[cnt];
                            Console.WriteLine("j ke " + j + " arrsumdata :" + arrSumData[j] + " m_datascaled: " + m_dataScaled[cnt] + " cnt: " + cnt + " chancount: " + chanCount);
                        }
                        //addListViewItems(listViewAi, arrData);
                    }

                    arrAvgData = new string[arrSumData.Length];

                    for (int i = 0; i < arrSumData.Length; i++)
                    {
                        arrAvgData[i] = (arrSumData[i] / sectionLength).ToString("F4");
                        ValueY1.Text = arrAvgData[0];
                        //label2.Text = arrAvgData[1];
                        //label3.Text = arrAvgData[2];
                        //Console.WriteLine("i ke " + i + " arrsumdata :" + arrSumData[i]);
                        chartXY.Series[0].Points.AddY(arrAvgData[0]);

                    }
                    //editListViewItems(listViewAi, 0, arrAvgData);

                    //listViewAi.EndUpdate();
                }));

            }
			catch (System.Exception)
            {

            }   
        }

        private void button_pause_Click(object sender, EventArgs e)
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

        private void button_stop_Click(object sender, EventArgs e)
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

        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;

            err = waveformAiCtrl1.Prepare();
            m_xInc = 1.0 / waveformAiCtrl1.Conversion.ClockRate;
            if (err == ErrorCode.Success)
            {
                err = waveformAiCtrl1.Start();
                //waveformAiCtrl1.DataReady += new EventHandler<BfdAiEventArgs>(waveformAiCtrl1_DataReady);
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

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void stopRecordToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void chartXY_Click(object sender, EventArgs e)
        {
           
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXTRec helpxtrec = new HelpXTRec();
            helpxtrec.ShowDialog();
        }

        private void Sensor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValY1.Text = Sensor1.Text;
            Unit1.Items.Clear();
            switch (Sensor1.Text)
            {
                case "Load Cell":
                    Unit1.Items.Add("kg");
                    Unit1.Items.Add("N");
                    Unit1.Items.Add("kN");
                    Unit1.Items.Add("Ton");
                    break;
                case "LVDT":
                    Unit1.Items.Add("cm");
                    Unit1.Items.Add("mm");
                    break;
                case "SG":
                    Unit1.Items.Add("uS");
                    break;
                case "Pressure":
                    Unit1.Items.Add("Kg/cm2");
                    Unit1.Items.Add("Mpa");
                    Unit1.Items.Add("Psi");
                    Unit1.Items.Add("Bar");
                    break;
                case "Volt":
                    Unit1.Items.Add("V");
                    Unit1.Items.Add("mV");
                    break;
            }
        }

        private void Sensor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValY2.Text = Sensor2.Text;
            Unit2.Items.Clear();
            switch (Sensor2.Text)
            {
                case "Load Cell":
                    Unit2.Items.Add("kg");
                    Unit2.Items.Add("N");
                    Unit2.Items.Add("kN");
                    Unit2.Items.Add("Ton");
                    break;
                case "LVDT":
                    Unit2.Items.Add("cm");
                    Unit2.Items.Add("mm");
                    break;
                case "SG":
                    Unit2.Items.Add("uS");
                    break;
                case "Pressure":
                    Unit2.Items.Add("Kg/cm2");
                    Unit2.Items.Add("Mpa");
                    Unit2.Items.Add("Psi");
                    Unit2.Items.Add("Bar");
                    break;
                case "Volt":
                    Unit2.Items.Add("V");
                    Unit2.Items.Add("mV");
                    break;
            }
        }

        private void Unit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            U1.Text = Unit1.Text;
        }

        private void ValueY1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ValY1_Click(object sender, EventArgs e)
        {

        }

        private void Unit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            U2.Text = Unit2.Text;
        }

        private void RangeX_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TitleMain.Text = Title.Text;
            ConsumerMain.Text = Consumer.Text;
            SenseMain.Text = Sense1.Text + "&" + Sense2.Text + "Vs Waktu";
        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void TitleMain_Click(object sender, EventArgs e)
        {

        }

        private void U2_Click(object sender, EventArgs e)
        {

        }

        private void U1_Click(object sender, EventArgs e)
        {

        }
    }
}