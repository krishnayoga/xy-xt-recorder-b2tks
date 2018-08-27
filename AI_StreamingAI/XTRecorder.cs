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
using System.Text;
using System.Web.UI.DataVisualization;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.IO;
//ini
using System.Diagnostics;
//itu
using Automation.BDaq;
using excel = Microsoft.Office.Interop.Excel;


namespace AI_StreamingAI
{
    public partial class XTRecorder : Form
    {
        #region fields  
        double[] m_dataScaled;
        bool m_isFirstOverRun = true;
        double m_xInc;
        int dataCount = 0;
        int recCount = 0;
        double last_x = 0;
        double last_x_holdX = 0;
        bool firstChecked = true;
        string[] arrAvgData;
        string[] arrData;
        double[] arrSumData;
        double[] dataPrint;
        double max_y_1 = 0;
        double min_y_1 = 1000;
        double max_y_2 = 0;
        double min_y_2 = 0;
        double factor_baca_y_1 = 1, factor_baca_y_2 = 1;
        int max_x_chart;
        int min_x_chart;
        int max_y_chart;
        int min_y_chart;
        //ini untuk testing
        int ms = 0, s, m, h;
        int zero = 0, ten = 10;
        //itu untuk testing
        //ini
        double label_chart_1, label_chart_2, label_chart_3, label_chart_4, label_chart_5, label_chart_6;
        double label_chart_7, label_chart_8, label_chart_9, label_chart_10, label_chart_11;
        double pos_label_1, pos_label_2, pos_label_3, pos_label_4, pos_label_5, pos_label_6;
        double pos_label_7, pos_label_8, pos_label_9, pos_label_10, pos_label_11;
        int batas_chart_1, batas_chart_2, batas_chart_3, batas_chart_4, batas_chart_5, batas_chart_6;
        int batas_chart_7, batas_chart_8, batas_chart_9, batas_chart_10, batas_chart_11;
        //itu
        bool recordData;
        string load_data;
        double balance_1 = 0, balance_2 = 0;
        #endregion

        //ini
        Timer timer = new Timer();
        Timer timer_plot = new Timer();
        Timer timer_hold = new Timer();
        
        //List<DateTime> TimeList = new List<DateTime>();

        Stopwatch watch = new Stopwatch();
        Stopwatch timer_holdX = new Stopwatch();
        //itu

        public XTRecorder()
        {
            InitializeComponent();
            //ini
            timer.Tick += new EventHandler(timer_stopwatch);
            timer.Interval = 100;

            timer_plot.Tick += new EventHandler(plotChart);
            timer_plot.Interval = 100;
            //itu

            timer_hold.Tick += new EventHandler(timer_hold_x);
            timer_hold.Interval = 100;
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
                MessageBox.Show("Device belum terpasang!", "XT Recorder",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                this.Close();
                return;
            }

            int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
            int sectionLength = waveformAiCtrl1.Record.SectionLength;
            m_dataScaled = new double[chanCount * sectionLength];

            dataPrint = new double[3];

            this.Text = "XT Recorder (" + waveformAiCtrl1.SelectedDevice.Description + ")";

            button_start.Enabled = false;
            button_stop.Enabled = false;
            button_pause.Enabled = false;

            chartXY.Series[0].IsXValueIndexed = false;

            try
            {
                string file_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                StreamReader read = new StreamReader(Path.Combine(file_path, "config.txt"));

                for (int i = 1; i < 8; i++)
                {
                    if (i == 1)
                    {
                        TitleMain.Text = read.ReadLine();
                    }
                    else if (i == 2)
                    {
                        ConsumerMain.Text = read.ReadLine();
                    }
                    else if (i == 3)
                    {
                        SenseMain.Text = read.ReadLine();
                    }
                    else if (i == 4)
                    {
                        Sensor1.Text = read.ReadLine();
                    }
                    else if (i == 5)
                    {
                        Unit1.Text = read.ReadLine();
                    }
                    else if (i == 6)
                    {
                        Sensor2.Text = read.ReadLine();
                    }
                    else if (i == 7)
                    {
                        Unit2.Text = read.ReadLine();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Tidak ditemukan config.txt pada Folder My Documents", "Config file tidak ada!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                this.Close();
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
                            arrData[j] = m_dataScaled[cnt].ToString("F3");
                            arrSumData[j] += m_dataScaled[cnt];
                            //Console.WriteLine("j ke " + j + " arrsumdata :" + arrSumData[j] + " m_datascaled: " + m_dataScaled[cnt] + " cnt: " + cnt + " chancount: " + chanCount);
                        }
                        //addListViewItems(listViewAi, arrData);
                    }
                    arrAvgData = new string[arrSumData.Length];

                    for (int i = 0; i < arrSumData.Length; i++)
                    {
                        arrAvgData[i] = (arrSumData[i] / sectionLength).ToString("F3");
                        //ValueX1.Text = arrAvgData[0];
                        //ValueY.Text = arrAvgData[1];
                        //label3.Text = arrAvgData[2];
                        //Console.WriteLine("i ke " + i + " arrsumdata :" + arrSumData[i]);
                        dataCount++;
                    }

                    dataPrint[0] = Convert.ToDouble(arrAvgData[0]) * factor_baca_y_1;
                    dataPrint[1] = Convert.ToDouble(arrAvgData[1]) * factor_baca_y_2;

                    if (checkBox_InvertY1.Checked)
                    {
                        dataPrint[0] = -dataPrint[0];
                    }
                    if (checkBox_InvertY2.Checked)
                    {
                        dataPrint[1] = -dataPrint[1];
                    }
                    
                    ValueY1.Text = (dataPrint[0]-balance_1).ToString();
                    ValueY2.Text = (dataPrint[1]-balance_2).ToString();
                    
                    //channel 0
                    if (dataPrint[0] > max_y_1)
                    {
                        max_y_1 = dataPrint[0] - balance_1;
                    }

                    if (dataPrint[0] < min_y_1)
                    {
                        min_y_1 = dataPrint[0] - balance_1;
                    }

                    //channel 1
                    if (dataPrint[1] > max_y_2)
                    {
                        max_y_2 = dataPrint[1] - balance_2;
                    }

                    if (dataPrint[1] < min_y_2)
                    {
                        min_y_2 = dataPrint[1] - balance_2;
                    }
                    
                    //chartXY.Series[0].Points.AddXY(arrAvgData[0], arrAvgData[1]);

                    MaxY1.Text = max_y_1.ToString();
                    MinY1.Text = min_y_1.ToString();
                    MaxY2.Text = max_y_2.ToString();
                    MinY2.Text = min_y_2.ToString();

                    if (checkBox_holdX.Checked && firstChecked)
                    {
                        last_x_holdX = last_x;

                        //last_y_0 = dataPrint[0];
                        //last_y_1 = dataPrint[1];
                        //last_x = dataCount.ToString();
                        firstChecked = false;
                    }

                    if (checkBox_holdX.Checked)
                    {
                        timer_hold.Start();
                        timer_holdX.Start();
                    }
                    if (!checkBox_holdX.Checked)
                    {
                        timer_holdX.Stop();
                        timer_holdX.Reset();
                        timer_hold.Stop();
                    }
                    //plotChart(dataPrint);

                }));
                Console.WriteLine(dataCount / 3);

            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan saat akuisisi data. Silahkan restart program", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #region chart
        private void startChart()
        {
            chartXY.Series.Clear();
            chartXY.Series.Add("Series 1");
            chartXY.Series.Add("Series 2");
            chartXY.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartXY.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chartXY.Series[0].Color = Color.Blue;
            chartXY.Series[1].Color = Color.Red;
        }

        private void initChart()
        {
            //ini
            max_x_chart = Convert.ToInt32(RangeX.Text) * 61 * 9 + 20;
            //itu
            min_x_chart = -max_x_chart;
            max_y_chart = Convert.ToInt32(RangeY.Text);
            min_y_chart = -max_y_chart;

            
            //this.chartXY.Titles.Add(TitleMain.Text);

            /*
            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = min_x_chart;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = 1;
            chartXY.ChartAreas[0].AxisY.Interval = 1;
            */
            chartXY.ChartAreas[0].AxisX.Title = "Waktu (Menit)";
            chartXY.ChartAreas[0].AxisY.Title = Sensor1.Text + " (" + Unit1.Text + ")" + " & "+ Sensor2.Text + " (" + Unit2.Text + ")"; 
            

            //ini
            //chartXY.ChartAreas[0].AxisX.Crossing = 0;
            chartXY.ChartAreas[0].AxisY.Crossing = 0;

            chartXY.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 1000000000;
            chartXY.ChartAreas[0].AxisX.IsLabelAutoFit = false;

            chartXY.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartXY.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            

            //chartXY.Series[0].XValueType = ChartValueType.DateTime;
            //chartXY.Series[1].XValueType = ChartValueType.DateTime;

            //DateTime dt = DateTime.MinValue;

            //chartXY.ChartAreas[0].AxisX.Minimum = dt.AddMinutes(0).ToOADate();
            //chartXY.ChartAreas[0].AxisX.Maximum = dt.AddMinutes(50).ToOADate();

            //chartXY.ChartAreas[0].AxisX.Interval = 60;
            //chartXY.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;

            //chartXY.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";

            //this.chartXY.Titles.Add("pt. B2TKS - BPPT");


            chartXY.ChartAreas[0].AxisX.Maximum = max_x_chart;
            chartXY.ChartAreas[0].AxisX.Minimum = 0;
            chartXY.ChartAreas[0].AxisY.Maximum = max_y_chart;
            chartXY.ChartAreas[0].AxisY.Minimum = min_y_chart;
            chartXY.ChartAreas[0].AxisX.Interval = max_x_chart / 10;
            chartXY.ChartAreas[0].AxisY.Interval = max_y_chart / 10;

            label_chart_1 = Convert.ToDouble(max_x_chart) * 0;
            label_chart_2 = (Convert.ToDouble(max_x_chart) - 20) / 61 / 9 / 10;
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


            //CustomLabel chart_label = new CustomLabel(pos_label_2 - 0.5, pos_label_2 + 0.5, label_chart_2.ToString(), 1, LabelMarkStyle.None);

            //chartXY.ChartAreas[0].AxisX.CustomLabels.Add(chart_label);

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
            //chartXY.ChartAreas[0].AxisX.CustomLabels.Add(0.5, 1.5, label_chart_2.ToString());

            //ini untuk testing
            //Console.WriteLine(max_x_chart);
            //itu untuk testing
            //chartXY.ChartAreas[0].AxisX.Interval = max_x_chart / 10;
            //chartXY.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            //chartXY.ChartAreas[0].AxisY.Interval = max_y_chart / 10;


            //chartXY.ChartAreas[0].AxisX.Title = "waktu";
            //chartXY.ChartAreas[0].AxisY.Title = "nilai";
        }

        //ini
        void timer_stopwatch(object sender, EventArgs e)
        {
            //DateTime now = DateTime.Now;
            //TimeList.Add(now);


            Time.Text = watch.Elapsed.ToString(); // ini untuk timer sejak di klik start, ganti nama labelnya itu
        }
        //itu

        private void plotChart(object sender, EventArgs e)
        {
            if (checkBox_holdX.Checked)
            {
                if (check1.Checked)
                {
                    chartXY.Series[0].Points.AddXY(last_x_holdX, dataPrint[0]-balance_1);
                }

                if (check2.Checked)
                {
                    chartXY.Series[1].Points.AddXY(last_x_holdX, dataPrint[1]-balance_2);
                }

            }

            if (!checkBox_holdX.Checked)
            {
                last_x += 1;
                if (check1.Checked)
                {
                    chartXY.Series[0].Points.AddXY(last_x, dataPrint[0]-balance_1);
                }

                if (check2.Checked)
                {
                    chartXY.Series[1].Points.AddXY(last_x, dataPrint[1]-balance_2);
                }
                firstChecked = true;
            }
            if (recordData)
            {
                if (check1.Checked && !check2.Checked)
                {
                    dataPrint[1] = 0;

                }
                else if (check2.Checked && !check1.Checked)
                {
                    dataPrint[0] = 0;
                }

                StreamWriter sw = new StreamWriter(File.Text, append: true);

                sw.WriteLine("{0},{1},{2},{3}", DateTime.Now.ToString("hh:mm:ss:fff"), watch.Elapsed.ToString(), dataPrint[0]-balance_1, dataPrint[1]-balance_2);

                sw.Close();
                recCount++;
            }

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
            */
        }
        #endregion

        #region button menu strip
        //button start menu
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ini untuk testing
            chartXY.Series[0].Points.Clear();

            chartXY.ChartAreas[0].AxisX.CustomLabels.Clear();
            //itu untuk testing

            ErrorCode err = ErrorCode.Success;

            err = waveformAiCtrl1.Prepare();
            m_xInc = 1.0 / waveformAiCtrl1.Conversion.ClockRate;
            if (err == ErrorCode.Success)
            {
                err = waveformAiCtrl1.Start();
                //Console.WriteLine("halooo");
                //waveformAiCtrl1.DataReady += new EventHandler<BfdAiEventArgs>(waveformAiCtrl1_DataReady);
                //chartXY.Series[0].Points.AddXY(arrAvgData[1], arrAvgData[0]);
            }

            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            button_start.Enabled = false;
            button_pause.Enabled = false;
            startRecordToolStripMenuItem.Enabled = true;
            button_stop.Enabled = true;
            balanceToolStripMenuItem.Enabled = true;

            if (check1.Checked)
            {
                factor_baca_y_1 = Convert.ToInt32(Factor1.Text);
            }
            if (check2.Checked)
            {
                factor_baca_y_2 = Convert.ToInt32(Factor2.Text);
            }

            startChart();
            initChart();
            timer_plot.Start();
            recCount = 0;
        }

        //button balance menu
        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartXY.ChartAreas[0].AxisX.CustomLabels.Clear();
            startChart();
            initChart();
            if (check1.Checked)
            {
                chartXY.Series[0].Points.Clear();
                balance_1 = dataPrint[0];
            }
            if (check2.Checked)
            {
                chartXY.Series[1].Points.Clear();
                balance_2 = dataPrint[1];
            }
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);

            timer.Stop();
            timer_plot.Stop();
            watch.Stop();

            last_x = 0;
            last_x_holdX = 0;

            timer_plot.Stop();
            waveformAiCtrl1.Stop();

            waveformAiCtrl1.Start();
            timer_plot.Start();


            max_y_1 = 0;
            min_y_1 = 0;
            max_y_2 = 0;
            min_y_2 = 0;
            button_start.Enabled = true;
            watch.Reset();

        }

        //button stop menu
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            //ini
            timer.Stop();
            timer_plot.Stop();
            watch.Stop();
            //itu

            last_x = 0;
            last_x_holdX = 0;

            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_stop.Enabled = false;
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);
            balanceToolStripMenuItem.Enabled = true;

            balance_1 = 0;
            balance_2 = 0;
        }

        //button start record menu
        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recordData = true;
            //ini
            timer.Start();
            watch.Start();
            //itu
            try
            {
                StreamWriter write = new StreamWriter(File.Text);
                write.WriteLine("Judul," + TitleMain.Text);
                write.WriteLine("Customer," + ConsumerMain.Text);
                write.WriteLine("Grafik," + SenseMain.Text);
                write.WriteLine("Tanggal," + Date.Text);
                write.WriteLine("Waktu," + Waktu.Text);
                write.WriteLine("SensorY1," + Sensor1.Text);
                write.WriteLine("UnitY1," + Unit1.Text);
                write.WriteLine("SensorY2," + Sensor2.Text);
                write.WriteLine("UnitY2," + Unit2.Text);
                if (!check2.Checked && check1.Checked)
                {
                    write.WriteLine("Jenis, 1,Jumlah Data");
                }
                else if (check2.Checked && !check1.Checked)
                {
                    write.WriteLine("Jenis, 2,Jumlah Data");
                }
                else if (check2.Checked && check1.Checked)
                {
                    write.WriteLine("Jenis, 3,Jumlah Data");
                }
                write.WriteLine("MaxY1,");
                write.WriteLine("MinY1,");
                write.WriteLine("MaxY2,");
                write.WriteLine("MinY2,");
                write.WriteLine("Waktu Total");
                write.WriteLine("Waktu Ambil,Waktu,Nilai Y1,Nilai Y2");
                write.Close();
            }
            catch
            {
                MessageBox.Show("Gagal membuka " + File.Text, "Error membuka file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            
            startRecordToolStripMenuItem.Enabled = false;
            button_pause.Enabled = true;
            label_Alert.Text = "Recording.....!!!";
            button_pause.Enabled = true;
        }

        //button stop record menu
        private void stopRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recordData = false;
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            //ini
            timer.Stop();
            timer_plot.Stop();
            watch.Stop();
            //itu

            last_x = 0;
            last_x_holdX = 0;

            try
            {
                excel.Application ex = new excel.Application();
                excel.Workbook book = ex.Workbooks.Open(File.Text);
                excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;
                res.Cells[10, 4] = recCount;
                res.Cells[11, 2] = MaxY1.Text;
                res.Cells[12, 2] = MinY1.Text;
                res.Cells[13, 2] = MaxY2.Text;
                res.Cells[14, 2] = MinY2.Text;
                res.Cells[15, 2] = Time.Text;
                res.Columns.AutoFit();
                book.SaveAs(File.Text);
                book.Close();
                ex.Quit();
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan file " + File.Text, "Error menyimpan file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            button_start.Enabled = false;
            button_pause.Enabled = false;
            startRecordToolStripMenuItem.Enabled = true;
            label_Alert.Text = "";

            balance_1 = 0;
            balance_2 = 0;
        }

        private void check1_CheckedChanged(object sender, EventArgs e)
        {
            Sensor1.Items.Clear();
            if (check1.Checked)
            {
                Sensor1.Items.Add("Volt");
                Sensor1.Items.Add("Pressure");
                Sensor1.Items.Add("SG");
                Sensor1.Items.Add("LVDT");
                Sensor1.Items.Add("Load Cell");
                Factor1.ReadOnly = false;
                Factor1.Text = "1";
                label_star1.Text = "*";
                label_star2.Text = "*";
                label_star3.Text = "*";
                label_ColorY1.Text = "---";
                
            } else
            {
                Factor1.ReadOnly = true;
                Factor1.Text = "-";
                label_ColorY1.Text = " ";
                label_star1.Text = "";
                label_star2.Text = "";
                label_star3.Text = "";
            }
        }

        private void check2_CheckedChanged(object sender, EventArgs e)
        {
            Sensor2.Items.Clear();
            if (check2.Checked)
            {
                Sensor2.Items.Add("Volt");
                Sensor2.Items.Add("Pressure");
                Sensor2.Items.Add("SG");
                Sensor2.Items.Add("LVDT");
                Sensor2.Items.Add("Load Cell");
                Factor2.ReadOnly = false;
                Factor2.Text = "1";
                label_star5.Text = "*";
                label_star6.Text = "*";
                label_star7.Text = "*";
                label_ColorY2.Text = "---";
            }
            else
            {
                Factor2.ReadOnly = true;
                Factor2.Text = "-";
                label_star5.Text = "";
                label_star6.Text = "";
                label_star7.Text = "";
                label_ColorY2.Text = " ";
            }
        }

        private void Factor1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void Factor2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void RangeY_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45 )
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void RangeX_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45 )
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        //button replot menu
        private void replotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartXY.ChartAreas[0].AxisX.CustomLabels.Clear();
            initChart();
        }

        //button print to png menu
        private void printToPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.chartXY.SaveImage(File.Text+".png", ChartImageFormat.Png);
                MessageBox.Show("Sukses menyimpan chart", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan chart", "Save PNG Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //button filename menu
        private void fileNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Title = "Save File";
                save.Filter = "CSV Files (*.csv)|*.csv|Text Files(*.txt)|*.txt";
                save.ShowDialog();
                File.Text = save.FileName.ToString();
                Date.Text = DateTime.Now.ToShortDateString();
                Waktu.Text = DateTime.Now.ToLongTimeString();
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan file " + File.Text, "Gagal menyimpan file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            
            button_start.Enabled = true;
        }
        
        //button help menu
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXTRec helpxtrec = new HelpXTRec();
            helpxtrec.ShowDialog();
        }
        #endregion

        #region fungsi tambahan
        private void timer_hold_x(object sender, EventArgs e)
        {
            textBox_HoldTime.Text = timer_holdX.Elapsed.ToString();
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

        private void printToPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(print_page);
            pd.Print();
        }

        private void print_page(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Font printFont = new System.Drawing.Font("Arial", 12);
            Rectangle myRec = new System.Drawing.Rectangle(30, 100, 800, 500);
            e.Graphics.DrawString(TitleMain.Text, printFont, Brushes.Black, 30, 40);
            e.Graphics.DrawString(ConsumerMain.Text, printFont, Brushes.Black, 30, 60);
            e.Graphics.DrawString(SenseMain.Text, printFont, Brushes.Black, 30, 80);
            chartXY.Printing.PrintPaint(e.Graphics, myRec);
            e.Graphics.DrawString("Garis biru: Sensor " + Sensor1.Text + " Y1", printFont, Brushes.Black, 30, 620);
            e.Graphics.DrawString("Garis merah: Sensor " + Sensor2.Text + " Y2", printFont, Brushes.Black, 30, 640);
            e.Graphics.DrawString("Tanggal pengujian: " + Date.Text, printFont, Brushes.Black, 30, 660);
            e.Graphics.DrawString("Waktu pengujian: " + Waktu.Text, printFont, Brushes.Black, 30, 680);
        }

        private void Date_Click(object sender, EventArgs e)
        {

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

        //update button
        private void button1_Click(object sender, EventArgs e)
        {
            TitleMain.Text = Title.Text;
            ConsumerMain.Text = Consumer.Text;
            SenseMain.Text = Sensor1.Text + " dan " + Sensor2.Text + " vs Waktu";
            if (check1.Checked)
            {
                ValY1.Text = Sensor1.Text;
            } else
            {
                ValY1.Text = "---";
            }
            if (check2.Checked)
            {
                ValY2.Text = Sensor2.Text;
            }
            else
            {
                ValY2.Text = "---";
            }
            try
            {
                string file_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                StreamWriter write = new StreamWriter(Path.Combine(file_path, "config.txt"));
                write.WriteLine(TitleMain.Text);
                write.WriteLine(ConsumerMain.Text);
                write.WriteLine(SenseMain.Text);
                write.WriteLine(Sensor1.Text);
                write.WriteLine(Unit1.Text);
                write.WriteLine(Sensor2.Text);
                write.WriteLine(Unit2.Text);
                write.Close();
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan data ke configuration file", "Error save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
                MessageBox.Show("Terjadi kesalahan. Kode error: " + err.ToString(), "StreamingAI");
            }
        }

        private void Unit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_unitY1.Text = Unit1.Text;
        }

        private void Unit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_unitY2.Text = Unit2.Text;
        }
        #endregion

        #region fungsi kosong
        private void button_SaveConfig_Click(object sender, EventArgs e)
        {

        }

        private void Factor1_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void RangeY_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConsumerMain_Click(object sender, EventArgs e)
        {

        }

        private void RangeX_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void titleToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void ValueY1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ValY1_Click(object sender, EventArgs e)
        {

        }

        private void chartXY_Click(object sender, EventArgs e)
        {

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

        private void label_unitY1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_holdX_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            /*
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
            */
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            /*
            ErrorCode err = ErrorCode.Success;      
			err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
			    HandleError(err);
                return;
            }

            button_start.Enabled = true;
            button_pause.Enabled = false;
            */
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            /*
		    ErrorCode err = ErrorCode.Success;
			err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
			    HandleError(err);
                return;
            }

            //ini
            timer.Stop();
            watch.Stop();
            //itu

            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_stop.Enabled = false;
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);
            */
        }
        #endregion
    }
}