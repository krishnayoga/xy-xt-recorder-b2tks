namespace AI_StreamingAI
{
    partial class XYReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XYReport));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Waktu = new System.Windows.Forms.Label();
            this.File = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_MaxX = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.comboBox_MaxY = new System.Windows.Forms.ComboBox();
            this.Date = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_MinY = new System.Windows.Forms.Label();
            this.comboBox_MinX = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_MinY = new System.Windows.Forms.ComboBox();
            this.TitleMain = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SenseMain = new System.Windows.Forms.Label();
            this.chartXY = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ConsumerMain = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.U3 = new System.Windows.Forms.Label();
            this.ValX2 = new System.Windows.Forms.Label();
            this.U2 = new System.Windows.Forms.Label();
            this.U1 = new System.Windows.Forms.Label();
            this.ValX1 = new System.Windows.Forms.Label();
            this.ValY = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.minX2 = new System.Windows.Forms.TextBox();
            this.MaxX2 = new System.Windows.Forms.TextBox();
            this.MinX1 = new System.Windows.Forms.TextBox();
            this.MaxX1 = new System.Windows.Forms.TextBox();
            this.MinY = new System.Windows.Forms.TextBox();
            this.MaxY = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // Waktu
            // 
            this.Waktu.Location = new System.Drawing.Point(1237, 659);
            this.Waktu.Name = "Waktu";
            this.Waktu.Size = new System.Drawing.Size(108, 19);
            this.Waktu.TabIndex = 72;
            this.Waktu.Text = "Waktu Pembacaan";
            this.Waktu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // File
            // 
            this.File.Location = new System.Drawing.Point(1078, 640);
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(267, 19);
            this.File.TabIndex = 70;
            this.File.Text = "File Directory";
            this.File.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Range Max - Y";
            // 
            // comboBox_MaxX
            // 
            this.comboBox_MaxX.FormattingEnabled = true;
            this.comboBox_MaxX.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox_MaxX.Location = new System.Drawing.Point(159, 45);
            this.comboBox_MaxX.Name = "comboBox_MaxX";
            this.comboBox_MaxX.Size = new System.Drawing.Size(87, 21);
            this.comboBox_MaxX.TabIndex = 40;
            this.comboBox_MaxX.Text = "10";
            this.comboBox_MaxX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_MaxX_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Setting Skala";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(156, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(78, 13);
            this.label21.TabIndex = 34;
            this.label21.Text = "Range Max - X";
            // 
            // comboBox_MaxY
            // 
            this.comboBox_MaxY.FormattingEnabled = true;
            this.comboBox_MaxY.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox_MaxY.Location = new System.Drawing.Point(25, 45);
            this.comboBox_MaxY.Name = "comboBox_MaxY";
            this.comboBox_MaxY.Size = new System.Drawing.Size(87, 21);
            this.comboBox_MaxY.TabIndex = 33;
            this.comboBox_MaxY.Text = "10";
            this.comboBox_MaxY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_MaxY_KeyPress);
            // 
            // Date
            // 
            this.Date.Location = new System.Drawing.Point(1081, 659);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(110, 19);
            this.Date.TabIndex = 71;
            this.Date.Text = "Tanggal Pembacaan";
            this.Date.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label_MinY);
            this.panel1.Controls.Add(this.comboBox_MinX);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.comboBox_MinY);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox_MaxX);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.comboBox_MaxY);
            this.panel1.Location = new System.Drawing.Point(1074, 154);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 137);
            this.panel1.TabIndex = 69;
            // 
            // label_MinY
            // 
            this.label_MinY.Location = new System.Drawing.Point(22, 82);
            this.label_MinY.Name = "label_MinY";
            this.label_MinY.Size = new System.Drawing.Size(78, 13);
            this.label_MinY.TabIndex = 45;
            this.label_MinY.Text = "Range Min - Y";
            // 
            // comboBox_MinX
            // 
            this.comboBox_MinX.FormattingEnabled = true;
            this.comboBox_MinX.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox_MinX.Location = new System.Drawing.Point(159, 101);
            this.comboBox_MinX.Name = "comboBox_MinX";
            this.comboBox_MinX.Size = new System.Drawing.Size(87, 21);
            this.comboBox_MinX.TabIndex = 44;
            this.comboBox_MinX.Text = "-10";
            this.comboBox_MinX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_MinX_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Range Min - X";
            // 
            // comboBox_MinY
            // 
            this.comboBox_MinY.FormattingEnabled = true;
            this.comboBox_MinY.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox_MinY.Location = new System.Drawing.Point(25, 101);
            this.comboBox_MinY.Name = "comboBox_MinY";
            this.comboBox_MinY.Size = new System.Drawing.Size(87, 21);
            this.comboBox_MinY.TabIndex = 42;
            this.comboBox_MinY.Text = "-10";
            this.comboBox_MinY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox_MinY_KeyPress);
            // 
            // TitleMain
            // 
            this.TitleMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleMain.Location = new System.Drawing.Point(335, 34);
            this.TitleMain.Name = "TitleMain";
            this.TitleMain.Size = new System.Drawing.Size(740, 38);
            this.TitleMain.TabIndex = 65;
            this.TitleMain.Text = "Judul Pengujian";
            this.TitleMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNameToolStripMenuItem,
            this.loadDataToolStripMenuItem,
            this.replotToolStripMenuItem,
            this.printToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1349, 24);
            this.menuStrip1.TabIndex = 61;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileNameToolStripMenuItem
            // 
            this.fileNameToolStripMenuItem.Name = "fileNameToolStripMenuItem";
            this.fileNameToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.fileNameToolStripMenuItem.Text = "&File Name";
            this.fileNameToolStripMenuItem.Click += new System.EventHandler(this.fileNameToolStripMenuItem_Click);
            // 
            // loadDataToolStripMenuItem
            // 
            this.loadDataToolStripMenuItem.Enabled = false;
            this.loadDataToolStripMenuItem.Name = "loadDataToolStripMenuItem";
            this.loadDataToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.loadDataToolStripMenuItem.Text = "Load Data";
            this.loadDataToolStripMenuItem.Click += new System.EventHandler(this.loadDataToolStripMenuItem_Click);
            // 
            // replotToolStripMenuItem
            // 
            this.replotToolStripMenuItem.Name = "replotToolStripMenuItem";
            this.replotToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.replotToolStripMenuItem.Text = "&Replot";
            this.replotToolStripMenuItem.Click += new System.EventHandler(this.replotToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(52, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(277, 131);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 62;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "Range Max Y";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.comboBox4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.comboBox5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(1717, 346);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(271, 66);
            this.panel2.TabIndex = 63;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(211, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Minute";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox4.Location = new System.Drawing.Point(109, 9);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(87, 21);
            this.comboBox4.TabIndex = 48;
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "50",
            "60",
            "90",
            "100",
            "150",
            "200"});
            this.comboBox5.Location = new System.Drawing.Point(109, 36);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(87, 21);
            this.comboBox5.TabIndex = 46;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 45;
            this.label6.Text = "Range Max X";
            // 
            // SenseMain
            // 
            this.SenseMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SenseMain.Location = new System.Drawing.Point(335, 120);
            this.SenseMain.Name = "SenseMain";
            this.SenseMain.Size = new System.Drawing.Size(740, 48);
            this.SenseMain.TabIndex = 66;
            this.SenseMain.Text = "Sensor X vs Sensor Y";
            this.SenseMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartXY
            // 
            this.chartXY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartXY.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chartXY.Legends.Add(legend1);
            this.chartXY.Location = new System.Drawing.Point(0, 189);
            this.chartXY.Name = "chartXY";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chartXY.Series.Add(series1);
            this.chartXY.Series.Add(series2);
            this.chartXY.Size = new System.Drawing.Size(1071, 489);
            this.chartXY.TabIndex = 64;
            this.chartXY.Text = "chart1";
            // 
            // ConsumerMain
            // 
            this.ConsumerMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsumerMain.Location = new System.Drawing.Point(335, 72);
            this.ConsumerMain.Name = "ConsumerMain";
            this.ConsumerMain.Size = new System.Drawing.Size(736, 48);
            this.ConsumerMain.TabIndex = 67;
            this.ConsumerMain.Text = "Nama Customer";
            this.ConsumerMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.U3);
            this.panel5.Controls.Add(this.ValX2);
            this.panel5.Controls.Add(this.U2);
            this.panel5.Controls.Add(this.U1);
            this.panel5.Controls.Add(this.ValX1);
            this.panel5.Controls.Add(this.ValY);
            this.panel5.Controls.Add(this.label26);
            this.panel5.Controls.Add(this.label25);
            this.panel5.Controls.Add(this.minX2);
            this.panel5.Controls.Add(this.MaxX2);
            this.panel5.Controls.Add(this.MinX1);
            this.panel5.Controls.Add(this.MaxX1);
            this.panel5.Controls.Add(this.MinY);
            this.panel5.Controls.Add(this.MaxY);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Location = new System.Drawing.Point(1074, 23);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(271, 135);
            this.panel5.TabIndex = 73;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(246, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 65;
            this.label9.Text = "(Y)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(240, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 64;
            this.label8.Text = "(X2)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 63;
            this.label2.Text = "(X1)";
            // 
            // U3
            // 
            this.U3.AutoSize = true;
            this.U3.Location = new System.Drawing.Point(159, 94);
            this.U3.Name = "U3";
            this.U3.Size = new System.Drawing.Size(26, 13);
            this.U3.TabIndex = 61;
            this.U3.Text = "Unit";
            // 
            // ValX2
            // 
            this.ValX2.AutoSize = true;
            this.ValX2.Location = new System.Drawing.Point(1, 94);
            this.ValX2.Name = "ValX2";
            this.ValX2.Size = new System.Drawing.Size(50, 13);
            this.ValX2.TabIndex = 60;
            this.ValX2.Text = "Value X2";
            // 
            // U2
            // 
            this.U2.AutoSize = true;
            this.U2.Location = new System.Drawing.Point(159, 67);
            this.U2.Name = "U2";
            this.U2.Size = new System.Drawing.Size(26, 13);
            this.U2.TabIndex = 59;
            this.U2.Text = "Unit";
            // 
            // U1
            // 
            this.U1.AutoSize = true;
            this.U1.Location = new System.Drawing.Point(159, 40);
            this.U1.Name = "U1";
            this.U1.Size = new System.Drawing.Size(26, 13);
            this.U1.TabIndex = 58;
            this.U1.Text = "Unit";
            // 
            // ValX1
            // 
            this.ValX1.AutoSize = true;
            this.ValX1.Location = new System.Drawing.Point(1, 67);
            this.ValX1.Name = "ValX1";
            this.ValX1.Size = new System.Drawing.Size(50, 13);
            this.ValX1.TabIndex = 54;
            this.ValX1.Text = "Value X1";
            // 
            // ValY
            // 
            this.ValY.AutoSize = true;
            this.ValY.Location = new System.Drawing.Point(1, 40);
            this.ValY.Name = "ValY";
            this.ValY.Size = new System.Drawing.Size(44, 13);
            this.ValY.TabIndex = 53;
            this.ValY.Text = "Value Y";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(121, 19);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(24, 13);
            this.label26.TabIndex = 52;
            this.label26.Text = "Min";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(69, 19);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(27, 13);
            this.label25.TabIndex = 51;
            this.label25.Text = "Max";
            // 
            // minX2
            // 
            this.minX2.Location = new System.Drawing.Point(111, 91);
            this.minX2.Name = "minX2";
            this.minX2.ReadOnly = true;
            this.minX2.Size = new System.Drawing.Size(48, 20);
            this.minX2.TabIndex = 49;
            // 
            // MaxX2
            // 
            this.MaxX2.Location = new System.Drawing.Point(57, 91);
            this.MaxX2.Name = "MaxX2";
            this.MaxX2.ReadOnly = true;
            this.MaxX2.Size = new System.Drawing.Size(48, 20);
            this.MaxX2.TabIndex = 48;
            // 
            // MinX1
            // 
            this.MinX1.Location = new System.Drawing.Point(111, 64);
            this.MinX1.Name = "MinX1";
            this.MinX1.ReadOnly = true;
            this.MinX1.Size = new System.Drawing.Size(48, 20);
            this.MinX1.TabIndex = 46;
            // 
            // MaxX1
            // 
            this.MaxX1.Location = new System.Drawing.Point(57, 64);
            this.MaxX1.Name = "MaxX1";
            this.MaxX1.ReadOnly = true;
            this.MaxX1.Size = new System.Drawing.Size(48, 20);
            this.MaxX1.TabIndex = 45;
            // 
            // MinY
            // 
            this.MinY.Location = new System.Drawing.Point(111, 37);
            this.MinY.Name = "MinY";
            this.MinY.ReadOnly = true;
            this.MinY.Size = new System.Drawing.Size(48, 20);
            this.MinY.TabIndex = 43;
            // 
            // MaxY
            // 
            this.MaxY.Location = new System.Drawing.Point(57, 37);
            this.MaxY.Name = "MaxY";
            this.MaxY.ReadOnly = true;
            this.MaxY.Size = new System.Drawing.Size(48, 20);
            this.MaxY.TabIndex = 42;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(-1, 4);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Decimal Value";
            // 
            // XYReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1349, 687);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.Waktu);
            this.Controls.Add(this.File);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TitleMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SenseMain);
            this.Controls.Add(this.chartXY);
            this.Controls.Add(this.ConsumerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1365, 726);
            this.MinimumSize = new System.Drawing.Size(1364, 726);
            this.Name = "XYReport";
            this.Text = "Report XY Recorder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.XYReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Waktu;
        private System.Windows.Forms.Label File;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_MaxX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox comboBox_MaxY;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label TitleMain;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label SenseMain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXY;
        private System.Windows.Forms.Label ConsumerMain;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label U3;
        private System.Windows.Forms.Label ValX2;
        private System.Windows.Forms.Label U2;
        private System.Windows.Forms.Label U1;
        private System.Windows.Forms.Label ValX1;
        private System.Windows.Forms.Label ValY;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox minX2;
        private System.Windows.Forms.TextBox MaxX2;
        private System.Windows.Forms.TextBox MinX1;
        private System.Windows.Forms.TextBox MaxX1;
        private System.Windows.Forms.TextBox MinY;
        private System.Windows.Forms.TextBox MaxY;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label_MinY;
        private System.Windows.Forms.ComboBox comboBox_MinX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_MinY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem loadDataToolStripMenuItem;
    }
}