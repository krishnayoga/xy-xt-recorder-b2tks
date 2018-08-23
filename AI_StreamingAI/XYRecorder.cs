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
using System.Diagnostics;
using Automation.BDaq;
using System.IO;
using excel = Microsoft.Office.Interop.Excel;

namespace AI_StreamingAI
{
    public partial class XYRecorder : Form
    {
        #region fields  

        double[] m_dataScaled;
        bool m_isFirstOverRun = true;
        double m_xInc;
        int dataCount = 0;
        int recCount = 0;
        double last_x_0;
        double last_x_1;
        bool firstChecked = true;
        string[] arrAvgData;
        string[] arrData;
        double[] arrSumData;
        double[] dataPrint;
        double max_x_1 = -1000;
        double min_x_1 = 1000;
        double max_x_2 = -1000;
        double min_x_2 = 1000;
        double max_y = -1000;
        double min_y = 1000;
        double factor_baca_x_1 = 1, factor_baca_x_2 = 1, factor_baca_y = 1;
        int max_x_chart;
        int min_x_chart;
        int max_y_chart;
        int min_y_chart;
        bool recordData;
        string load_data;

        #endregion

        Stopwatch watch = new Stopwatch();
        
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
                MessageBox.Show("Device belum terpasang!", "StreamingAI");
                this.Close();
                return;
            }

		    int chanCount = waveformAiCtrl1.Conversion.ChannelCount;
		    int sectionLength = waveformAiCtrl1.Record.SectionLength;
		    m_dataScaled = new double[chanCount * sectionLength];

            dataPrint = new double[3];

            this.Text = "Streaming AI(" + waveformAiCtrl1.SelectedDevice.Description + ")";

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
                    load_data = read.ReadLine();
                    if (i == 1)
                    {
                        TitleMain.Text = load_data;
                    }
                    else if (i == 2)
                    {
                        ConsumerMain.Text = load_data;
                    }
                    else if (i == 3)
                    {
                        SenseMain.Text = load_data;
                    }
                    else if (i == 4)
                    {
                        SensorY.Text = load_data;
                    }
                    else if (i == 5)
                    {
                        UnitY.Text = load_data;
                    }
                    else if (i == 6)
                    {
                        SensorX1.Text = load_data;
                    }
                    else if (i == 7)
                    {
                        UnitX1.Text = load_data;
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

                    if (recordData)
                    {
                        if (check1.Checked&&!check2.Checked)
                        {
                            dataPrint[1] = 0;
          
                        }
                        else if (check2.Checked&&!check1.Checked)
                        {
                            dataPrint[0] = 0;
                        }
                        
                        StreamWriter sw = new StreamWriter(File.Text, append: true);

                        sw.WriteLine("{0},{1},{2},{3}", DateTime.Now.ToString("hh:mm:ss:fff"), dataPrint[0], dataPrint[1], dataPrint[2]);

                        sw.Close();
                        recCount++;
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
                    textBox_stopwatch.Text = watch.Elapsed.ToString();
                }));
                Console.WriteLine(dataCount / 3);
                
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan saat akuisisi data. Silahkan restart program", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region chart
        private void startChart()
        {
            chartXY.Series.Clear();
            chartXY.Series.Add("Series 1");
            chartXY.Series.Add("Series 2");
            chartXY.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartXY.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chartXY.ChartAreas[0].AxisX.Crossing = 0;
            chartXY.ChartAreas[0].AxisY.Crossing = 0;

            chartXY.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartXY.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            chartXY.Series[0].Color = Color.Blue;
            chartXY.Series[1].Color = Color.Red;

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
            chartXY.ChartAreas[0].AxisX.Interval = max_x_chart/10;
            chartXY.ChartAreas[0].AxisY.Interval = max_y_chart/10;
            
            chartXY.ChartAreas[0].AxisX.Title = SensorX1.Text + " (" + UnitX1.Text +")";
            chartXY.ChartAreas[0].AxisY.Title = SensorY.Text + " (" + UnitY.Text + ")";

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
        #endregion

        #region menu button
        //fungsi untuk menu start button
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            watch.Reset();
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
            startStripMenuItem1.Enabled = false;
            button_start.Enabled = true;
            button_stop.Enabled = true;

            if (check1.Checked)
            {
                factor_baca_x_1 = Convert.ToDouble(factor_x_1.Text);
            }
            if (check2.Checked)
            {
                factor_baca_x_2 = Convert.ToDouble(factor_x_2.Text);
            }
            
            factor_baca_y = Convert.ToDouble(factor_y.Text);

            watch.Start();
            startChart();
            initChart();
            button_start.Enabled = true;
            button_stop.Enabled = true;
            balanceToolStripMenuItem.Enabled = false;
            recCount = 0;
            button_pause.Enabled = false;
        }

        //fungsi untuk menu stop button
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }

            watch.Stop();

            button_start.Enabled = false;
            button_pause.Enabled = false;
            button_stop.Enabled = false;
            Array.Clear(m_dataScaled, 0, m_dataScaled.Length);
            
            startStripMenuItem1.Enabled = true;
            balanceToolStripMenuItem.Enabled = true;
        }

        //Button isi filename
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
            
            startStripMenuItem1.Enabled = true;
            button_start.Enabled = false;
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
            max_x_1 = 0;
            min_x_1 = 0;
            max_x_2 = 0;
            min_x_2 = 0;
            max_y = 0;
            min_y = 0;
            startStripMenuItem1.Enabled = true;
            button_stop.Enabled = true;
            watch.Reset();
            
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
            SenseMain.Text =  SensorX1.Text + " vs " + SensorY.Text;
            if (check1.Checked)
            {
                ValX1.Text = SensorX1.Text;
            }
            else
            {
                ValX1.Text = "---";
            }
            if (check2.Checked)
            {
                ValX2.Text = SensorX1.Text;
            }
            else
            {
                ValX2.Text = "---";
            }

        }

        //fungsi untuk print to png
        private void printToPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.chartXY.SaveImage(File.Text+".png", ChartImageFormat.Png);
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan chart", "Save PNG Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //fungsi untuk menu start record
        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_start.Enabled = false;
            button_pause.Enabled = true;
            recordData = true;
            try
            {
                StreamWriter write = new StreamWriter(File.Text);
                write.WriteLine("Judul," + TitleMain.Text);
                write.WriteLine("Customer," + ConsumerMain.Text);
                write.WriteLine("Grafik," + SenseMain.Text);
                write.WriteLine("Tanggal," + Date.Text);
                write.WriteLine("Waktu," + Waktu.Text);
                write.WriteLine("SensorY," + SensorY.Text);
                write.WriteLine("UnitY," + UnitY.Text);
                write.WriteLine("SensorX1," + SensorX1.Text);
                write.WriteLine("UnitX1," + UnitX1.Text);
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
                write.WriteLine("MaxY,");
                write.WriteLine("MinY,");
                write.WriteLine("MaxX1,");
                write.WriteLine("MinX1,");
                write.WriteLine("MaxX2,");
                write.WriteLine("MinX2,");
                write.WriteLine("Waktu,Nilai Y,Nilai X1, Nilai X2");
                write.Close();
            }
            catch
            {
                MessageBox.Show("Gagal membuka " + File.Text, "Error membuka file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
           
            label_Alert.Text = "Recording.....!!!";
        }

        //fungsi untuk menu stop record button
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            button_pause.Enabled = false;
            ErrorCode err = ErrorCode.Success;
            err = waveformAiCtrl1.Stop();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
                return;
            }
            recordData = false;
            button_start.Enabled = true;
            button_pause.Enabled = false;

            try
            {
                excel.Application ex = new excel.Application();
                excel.Workbook book = ex.Workbooks.Open(File.Text);
                excel.Worksheet res = ex.ActiveSheet as excel.Worksheet;
                res.Cells[10, 4] = recCount;
                res.Cells[11, 2] = MaxY.Text;
                res.Cells[12, 2] = MinY.Text;
                res.Cells[13, 2] = MaxX1.Text;
                res.Cells[14, 2] = MinX1.Text;
                res.Cells[15, 2] = MaxX2.Text;
                res.Cells[16, 2] = minX2.Text;
                res.Columns.AutoFit();
                book.SaveAs(File.Text);
                book.Close();
                ex.Quit();
            }
            catch
            {
                MessageBox.Show("Gagal menyimpan file " + File.Text, "Error menyimpan file", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            

            watch.Stop();
            label_Alert.Text = "";
        }


        #endregion

        #region unnecessary
        private void check2_CheckedChanged(object sender, EventArgs e)
        {
           if (check2.Checked)
            {
                factor_x_2.ReadOnly = false;
                factor_x_2.Text = "1";
                ValX2.Text = SensorX1.Text;
                star5.Text = "*";
                label_ColorX2.Text = "---";
            }
            else
            {
                factor_x_2.ReadOnly = true;
                factor_x_2.Text = "-";
                ValX2.Text = "---";
                star5.Text = " ";
                label_ColorX2.Text = " ";
            }

        }

        private void check1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (check1.Checked)
            {
                factor_x_1.ReadOnly = false;
                factor_x_1.Text = "1";
                ValX1.Text = SensorX1.Text;
                label_ColorX1.Text = "---";
                star2.Text = "*";

            }
            else
            {
                factor_x_1.ReadOnly = true;
                factor_x_1.Text = "-";
                label_ColorX1.Text = " ";
                ValX1.Text = "---";
                star2.Text = "*";
                
            }

        }
        private void SensorX1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValX1.Text = SensorX1.Text;
            UnitX1.Items.Clear();
            UnitX1.Text = "";
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
            if (check2.Checked==true)
            {
                ValX2.Text = SensorX1.Text;
            }
        }

        

        private void SensorY_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValY.Text = SensorY.Text;
            UnitY.Items.Clear();
            UnitY.Text = "";
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
        

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpXYRec helpxy = new HelpXYRec();
            helpxy.ShowDialog();
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

        private void factor_x_1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void factor_x_2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void rangeX_chart_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void factor_y_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }

        private void rangeY_chart_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 45)
            {
                e.Handled = true;
            }
            //Input hanya angka
        }
        

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartXY.SaveImage(File.Text+".png", ChartImageFormat.Png);
        }
        #endregion












        #region void kosong
        private void SensorX2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UnitY_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UnitX1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UnitX2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
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
        private void chartXY_Click(object sender, EventArgs e)
        {

        }
        private void Date_Click(object sender, EventArgs e)
        {
            
        }

        private void printToPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TitleMain_Click(object sender, EventArgs e)
        {

        }

        private void rangeY_chart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SenseMain_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void rangeX_chart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void factor_x_1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string file_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            StreamWriter write = new StreamWriter(Path.Combine(file_path, "config.txt"));
            write.WriteLine(TitleMain.Text);
            write.WriteLine(ConsumerMain.Text);
            write.WriteLine(SenseMain.Text);
            write.WriteLine(SensorY.Text);
            write.WriteLine(UnitY.Text);
            write.WriteLine(SensorX1.Text);
            write.WriteLine(UnitX1.Text);
            write.Close();
        }

        private void checkBox_invertX2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label_ColorX2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox_invertX1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ConsumerMain_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void factor_y_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion
    }
}