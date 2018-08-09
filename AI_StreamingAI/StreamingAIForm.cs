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
   public partial class StreamingAIForm : Form
   {
      #region fields  

      double[]     m_dataScaled;

      bool         m_isFirstOverRun = true;
      double       m_xInc;

        string[] arrAvgData;
        string[] arrData;
        double[] arrSumData;
        #endregion

        public StreamingAIForm()
      {
         InitializeComponent();
      }

      public StreamingAIForm(int deviceNumber)
      {
         InitializeComponent();
			waveformAiCtrl1.SelectedDevice = new DeviceInformation(deviceNumber);
      }
      
      private void StreamingBufferedAiForm_Load(object sender, EventArgs e)
      {
         //The default device of project is demo device, users can choose other devices according to their needs. 
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
			try
         {
			//The WaveformAiCtrl has been disposed.
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
                        label1.Text = arrAvgData[0];
                        label2.Text = arrAvgData[1];
                        label3.Text = arrAvgData[2];
                        //Console.WriteLine("i ke " + i + " arrsumdata :" + arrSumData[i]);

                        if (checkBox_holdX.Checked)
                        {
                            chartXY.Series[0].Points.AddXY(0.01, arrAvgData[0]);
                        }
                        else
                        {
                            chartXY.Series[0].Points.AddXY(arrAvgData[0], arrAvgData[1]);
                        }

                    }
                }));

            }
            catch
            {
                MessageBox.Show("nilai x dan y salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
    }
}