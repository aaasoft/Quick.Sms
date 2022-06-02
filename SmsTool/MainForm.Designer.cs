namespace SmsTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.cbDeviceType = new System.Windows.Forms.ComboBox();
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbSetting = new System.Windows.Forms.GroupBox();
            this.btnRefreshSerialPorts = new System.Windows.Forms.Button();
            this.nudBaudRate = new System.Windows.Forms.NumericUpDown();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tabTestAndLog = new System.Windows.Forms.TabControl();
            this.tpSms = new System.Windows.Forms.TabPage();
            this.nudSendCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSendContent = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSendTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpFeature = new System.Windows.Forms.TabPage();
            this.flpFeatures = new System.Windows.Forms.FlowLayoutPanel();
            this.tpDeviceStatus = new System.Windows.Forms.TabPage();
            this.flpDeviceStatus = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshDeviceStatus = new System.Windows.Forms.Button();
            this.tpTerminal = new System.Windows.Forms.TabPage();
            this.cbSendEncoding = new System.Windows.Forms.ComboBox();
            this.btnSendAT = new System.Windows.Forms.Button();
            this.txtATCommand = new System.Windows.Forms.TextBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).BeginInit();
            this.tabTestAndLog.SuspendLayout();
            this.tpSms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendCount)).BeginInit();
            this.tpFeature.SuspendLayout();
            this.tpDeviceStatus.SuspendLayout();
            this.flpDeviceStatus.SuspendLayout();
            this.tpTerminal.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备类型:";
            // 
            // cbDeviceType
            // 
            this.cbDeviceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeviceType.FormattingEnabled = true;
            this.cbDeviceType.Location = new System.Drawing.Point(112, 77);
            this.cbDeviceType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDeviceType.Name = "cbDeviceType";
            this.cbDeviceType.Size = new System.Drawing.Size(508, 28);
            this.cbDeviceType.TabIndex = 5;
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Location = new System.Drawing.Point(112, 33);
            this.cbSerialPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(227, 28);
            this.cbSerialPort.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "串口号:";
            // 
            // gbSetting
            // 
            this.gbSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSetting.Controls.Add(this.btnRefreshSerialPorts);
            this.gbSetting.Controls.Add(this.nudBaudRate);
            this.gbSetting.Controls.Add(this.btnScan);
            this.gbSetting.Controls.Add(this.btnOpen);
            this.gbSetting.Controls.Add(this.cbDeviceType);
            this.gbSetting.Controls.Add(this.cbSerialPort);
            this.gbSetting.Controls.Add(this.label1);
            this.gbSetting.Controls.Add(this.label6);
            this.gbSetting.Controls.Add(this.label2);
            this.gbSetting.Location = new System.Drawing.Point(18, 20);
            this.gbSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSetting.Name = "gbSetting";
            this.gbSetting.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbSetting.Size = new System.Drawing.Size(759, 135);
            this.gbSetting.TabIndex = 4;
            this.gbSetting.TabStop = false;
            this.gbSetting.Text = "设备";
            // 
            // btnRefreshSerialPorts
            // 
            this.btnRefreshSerialPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshSerialPorts.Location = new System.Drawing.Point(348, 29);
            this.btnRefreshSerialPorts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefreshSerialPorts.Name = "btnRefreshSerialPorts";
            this.btnRefreshSerialPorts.Size = new System.Drawing.Size(55, 39);
            this.btnRefreshSerialPorts.TabIndex = 2;
            this.btnRefreshSerialPorts.Text = "刷新";
            this.btnRefreshSerialPorts.UseVisualStyleBackColor = true;
            this.btnRefreshSerialPorts.Click += new System.EventHandler(this.btnRefreshSerialPorts_Click);
            // 
            // nudBaudRate
            // 
            this.nudBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudBaudRate.Location = new System.Drawing.Point(491, 33);
            this.nudBaudRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudBaudRate.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudBaudRate.Name = "nudBaudRate";
            this.nudBaudRate.Size = new System.Drawing.Size(132, 27);
            this.nudBaudRate.TabIndex = 3;
            this.nudBaudRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBaudRate.Value = new decimal(new int[] {
            115200,
            0,
            0,
            0});
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(631, 29);
            this.btnScan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(112, 39);
            this.btnScan.TabIndex = 4;
            this.btnScan.Text = "智能识别";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(631, 73);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(112, 39);
            this.btnOpen.TabIndex = 100;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(411, 39);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "波特率:";
            // 
            // tabTestAndLog
            // 
            this.tabTestAndLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTestAndLog.Controls.Add(this.tpSms);
            this.tabTestAndLog.Controls.Add(this.tpFeature);
            this.tabTestAndLog.Controls.Add(this.tpDeviceStatus);
            this.tabTestAndLog.Controls.Add(this.tpTerminal);
            this.tabTestAndLog.Location = new System.Drawing.Point(18, 165);
            this.tabTestAndLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabTestAndLog.Name = "tabTestAndLog";
            this.tabTestAndLog.SelectedIndex = 0;
            this.tabTestAndLog.Size = new System.Drawing.Size(759, 569);
            this.tabTestAndLog.TabIndex = 5;
            this.tabTestAndLog.Visible = false;
            // 
            // tpSms
            // 
            this.tpSms.Controls.Add(this.nudSendCount);
            this.tpSms.Controls.Add(this.label5);
            this.tpSms.Controls.Add(this.btnSend);
            this.tpSms.Controls.Add(this.txtSendContent);
            this.tpSms.Controls.Add(this.label4);
            this.tpSms.Controls.Add(this.txtSendTo);
            this.tpSms.Controls.Add(this.label3);
            this.tpSms.Location = new System.Drawing.Point(4, 29);
            this.tpSms.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpSms.Name = "tpSms";
            this.tpSms.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpSms.Size = new System.Drawing.Size(751, 536);
            this.tpSms.TabIndex = 0;
            this.tpSms.Text = "短信";
            this.tpSms.UseVisualStyleBackColor = true;
            // 
            // nudSendCount
            // 
            this.nudSendCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudSendCount.Location = new System.Drawing.Point(107, 435);
            this.nudSendCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nudSendCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudSendCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendCount.Name = "nudSendCount";
            this.nudSendCount.Size = new System.Drawing.Size(120, 27);
            this.nudSendCount.TabIndex = 6;
            this.nudSendCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSendCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 442);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "发送份数：";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(610, 434);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(112, 39);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // txtSendContent
            // 
            this.txtSendContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendContent.Location = new System.Drawing.Point(12, 128);
            this.txtSendContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSendContent.Multiline = true;
            this.txtSendContent.Name = "txtSendContent";
            this.txtSendContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSendContent.Size = new System.Drawing.Size(708, 293);
            this.txtSendContent.TabIndex = 3;
            this.txtSendContent.Text = "这是第{i}条短信，随机内容为：{guid}。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 104);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "发送内容:";
            // 
            // txtSendTo
            // 
            this.txtSendTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSendTo.Location = new System.Drawing.Point(9, 45);
            this.txtSendTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSendTo.Name = "txtSendTo";
            this.txtSendTo.Size = new System.Drawing.Size(712, 27);
            this.txtSendTo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "发送到: (多个号码用逗号分隔)";
            // 
            // tpFeature
            // 
            this.tpFeature.Controls.Add(this.flpFeatures);
            this.tpFeature.Location = new System.Drawing.Point(4, 29);
            this.tpFeature.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpFeature.Name = "tpFeature";
            this.tpFeature.Size = new System.Drawing.Size(850, 794);
            this.tpFeature.TabIndex = 3;
            this.tpFeature.Text = "功能";
            this.tpFeature.UseVisualStyleBackColor = true;
            // 
            // flpFeatures
            // 
            this.flpFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFeatures.Location = new System.Drawing.Point(0, 0);
            this.flpFeatures.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flpFeatures.Name = "flpFeatures";
            this.flpFeatures.Size = new System.Drawing.Size(850, 794);
            this.flpFeatures.TabIndex = 0;
            // 
            // tpDeviceStatus
            // 
            this.tpDeviceStatus.Controls.Add(this.flpDeviceStatus);
            this.tpDeviceStatus.Location = new System.Drawing.Point(4, 29);
            this.tpDeviceStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpDeviceStatus.Name = "tpDeviceStatus";
            this.tpDeviceStatus.Size = new System.Drawing.Size(850, 794);
            this.tpDeviceStatus.TabIndex = 2;
            this.tpDeviceStatus.Text = "状态";
            this.tpDeviceStatus.UseVisualStyleBackColor = true;
            // 
            // flpDeviceStatus
            // 
            this.flpDeviceStatus.AutoScroll = true;
            this.flpDeviceStatus.BackColor = System.Drawing.SystemColors.Control;
            this.flpDeviceStatus.Controls.Add(this.btnRefreshDeviceStatus);
            this.flpDeviceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDeviceStatus.Location = new System.Drawing.Point(0, 0);
            this.flpDeviceStatus.Margin = new System.Windows.Forms.Padding(0);
            this.flpDeviceStatus.Name = "flpDeviceStatus";
            this.flpDeviceStatus.Size = new System.Drawing.Size(850, 794);
            this.flpDeviceStatus.TabIndex = 7;
            this.flpDeviceStatus.SizeChanged += new System.EventHandler(this.FlpDeviceStatus_SizeChanged);
            // 
            // btnRefreshDeviceStatus
            // 
            this.btnRefreshDeviceStatus.Location = new System.Drawing.Point(0, 0);
            this.btnRefreshDeviceStatus.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefreshDeviceStatus.Name = "btnRefreshDeviceStatus";
            this.btnRefreshDeviceStatus.Size = new System.Drawing.Size(577, 39);
            this.btnRefreshDeviceStatus.TabIndex = 6;
            this.btnRefreshDeviceStatus.Text = "全部刷新";
            this.btnRefreshDeviceStatus.UseVisualStyleBackColor = true;
            this.btnRefreshDeviceStatus.Click += new System.EventHandler(this.BtnRefreshDeviceStatus_Click);
            // 
            // tpTerminal
            // 
            this.tpTerminal.Controls.Add(this.cbSendEncoding);
            this.tpTerminal.Controls.Add(this.btnSendAT);
            this.tpTerminal.Controls.Add(this.txtATCommand);
            this.tpTerminal.Controls.Add(this.btnClearLogs);
            this.tpTerminal.Controls.Add(this.txtLogs);
            this.tpTerminal.Location = new System.Drawing.Point(4, 29);
            this.tpTerminal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpTerminal.Name = "tpTerminal";
            this.tpTerminal.Size = new System.Drawing.Size(850, 794);
            this.tpTerminal.TabIndex = 4;
            this.tpTerminal.Text = "终端";
            this.tpTerminal.UseVisualStyleBackColor = true;
            // 
            // cbSendEncoding
            // 
            this.cbSendEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbSendEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSendEncoding.FormattingEnabled = true;
            this.cbSendEncoding.Items.AddRange(new object[] {
            "文本",
            "16进制"});
            this.cbSendEncoding.Location = new System.Drawing.Point(3, 745);
            this.cbSendEncoding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbSendEncoding.Name = "cbSendEncoding";
            this.cbSendEncoding.Size = new System.Drawing.Size(91, 28);
            this.cbSendEncoding.TabIndex = 11;
            // 
            // btnSendAT
            // 
            this.btnSendAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendAT.Location = new System.Drawing.Point(604, 744);
            this.btnSendAT.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSendAT.Name = "btnSendAT";
            this.btnSendAT.Size = new System.Drawing.Size(112, 39);
            this.btnSendAT.TabIndex = 10;
            this.btnSendAT.Text = "发送";
            this.btnSendAT.UseVisualStyleBackColor = true;
            this.btnSendAT.Click += new System.EventHandler(this.BtnSendAT_Click);
            // 
            // txtATCommand
            // 
            this.txtATCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtATCommand.Location = new System.Drawing.Point(106, 745);
            this.txtATCommand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtATCommand.Name = "txtATCommand";
            this.txtATCommand.Size = new System.Drawing.Size(489, 27);
            this.txtATCommand.TabIndex = 9;
            this.txtATCommand.Text = "AT";
            this.txtATCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TxtATCommand_KeyUp);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLogs.Location = new System.Drawing.Point(726, 744);
            this.btnClearLogs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(112, 39);
            this.btnClearLogs.TabIndex = 8;
            this.btnClearLogs.Text = "清空";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // txtLogs
            // 
            this.txtLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogs.Location = new System.Drawing.Point(4, 5);
            this.txtLogs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogs.Size = new System.Drawing.Size(833, 728);
            this.txtLogs.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 749);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(795, 26);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 20);
            this.lblStatus.Text = "就绪";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 775);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabTestAndLog);
            this.Controls.Add(this.gbSetting);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbSetting.ResumeLayout(false);
            this.gbSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).EndInit();
            this.tabTestAndLog.ResumeLayout(false);
            this.tpSms.ResumeLayout(false);
            this.tpSms.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendCount)).EndInit();
            this.tpFeature.ResumeLayout(false);
            this.tpDeviceStatus.ResumeLayout(false);
            this.flpDeviceStatus.ResumeLayout(false);
            this.tpTerminal.ResumeLayout(false);
            this.tpTerminal.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDeviceType;
        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbSetting;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TabControl tabTestAndLog;
        private System.Windows.Forms.TabPage tpSms;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSendContent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSendTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpDeviceStatus;
        private System.Windows.Forms.Button btnRefreshDeviceStatus;
        private System.Windows.Forms.TabPage tpFeature;
        private System.Windows.Forms.FlowLayoutPanel flpFeatures;
        private System.Windows.Forms.NumericUpDown nudSendCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpTerminal;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Button btnSendAT;
        private System.Windows.Forms.TextBox txtATCommand;
        private System.Windows.Forms.FlowLayoutPanel flpDeviceStatus;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ComboBox cbSendEncoding;
        private System.Windows.Forms.NumericUpDown nudBaudRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnRefreshSerialPorts;
    }
}

