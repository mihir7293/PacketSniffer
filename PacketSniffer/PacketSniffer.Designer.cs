namespace PacketSniffer
{
    partial class PacketSniffer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacketSniffer));
            this.lblDevice = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.bgLoadDeviceList = new System.ComponentModel.BackgroundWorker();
            this.dataGridPackets = new System.Windows.Forms.DataGridView();
            this.cmbDeviceList = new System.Windows.Forms.ComboBox();
            this.hexbData = new Be.Windows.Forms.HexBox();
            this.rb562 = new System.Windows.Forms.RadioButton();
            this.rbReturns = new System.Windows.Forms.RadioButton();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPackets)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(10, 10);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(46, 13);
            this.lblDevice.TabIndex = 1;
            this.lblDevice.Text = "Devices";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(357, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(451, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.status.Location = new System.Drawing.Point(0, 467);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(800, 22);
            this.status.TabIndex = 4;
            this.status.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // dataGridPackets
            // 
            this.dataGridPackets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPackets.Location = new System.Drawing.Point(13, 44);
            this.dataGridPackets.Name = "dataGridPackets";
            this.dataGridPackets.Size = new System.Drawing.Size(775, 196);
            this.dataGridPackets.TabIndex = 5;
            this.dataGridPackets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPackets_CellClick);
            this.dataGridPackets.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridPackets_DataBindingComplete);
            // 
            // cmbDeviceList
            // 
            this.cmbDeviceList.FormattingEnabled = true;
            this.cmbDeviceList.Items.AddRange(new object[] {
            "Loading..."});
            this.cmbDeviceList.Location = new System.Drawing.Point(61, 6);
            this.cmbDeviceList.Name = "cmbDeviceList";
            this.cmbDeviceList.Size = new System.Drawing.Size(256, 21);
            this.cmbDeviceList.TabIndex = 0;
            // 
            // hexbData
            // 
            this.hexbData.ColumnInfoVisible = true;
            this.hexbData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.hexbData.LineInfoVisible = true;
            this.hexbData.Location = new System.Drawing.Point(13, 247);
            this.hexbData.Name = "hexbData";
            this.hexbData.ReadOnly = true;
            this.hexbData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexbData.Size = new System.Drawing.Size(775, 217);
            this.hexbData.StringViewVisible = true;
            this.hexbData.TabIndex = 6;
            this.hexbData.UseFixedBytesPerLine = true;
            this.hexbData.VScrollBarVisible = true;
            // 
            // rb562
            // 
            this.rb562.AutoSize = true;
            this.rb562.Checked = true;
            this.rb562.Location = new System.Drawing.Point(570, 10);
            this.rb562.Name = "rb562";
            this.rb562.Size = new System.Drawing.Size(43, 17);
            this.rb562.TabIndex = 7;
            this.rb562.TabStop = true;
            this.rb562.Text = "562";
            this.rb562.UseVisualStyleBackColor = true;
            // 
            // rbReturns
            // 
            this.rbReturns.AutoSize = true;
            this.rbReturns.Location = new System.Drawing.Point(619, 10);
            this.rbReturns.Name = "rbReturns";
            this.rbReturns.Size = new System.Drawing.Size(62, 17);
            this.rbReturns.TabIndex = 8;
            this.rbReturns.Text = "Returns";
            this.rbReturns.UseVisualStyleBackColor = true;
            // 
            // PacketSniffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 489);
            this.Controls.Add(this.rbReturns);
            this.Controls.Add(this.rb562);
            this.Controls.Add(this.hexbData);
            this.Controls.Add(this.dataGridPackets);
            this.Controls.Add(this.status);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.cmbDeviceList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PacketSniffer";
            this.Text = "PacketSniffer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CaptureForm_FormClosing);
            this.Load += new System.EventHandler(this.CaptureForm_Load);
            this.Resize += new System.EventHandler(this.CaptureForm_Resize);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPackets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.ComponentModel.BackgroundWorker bgLoadDeviceList;
        private System.Windows.Forms.DataGridView dataGridPackets;
        private System.Windows.Forms.ComboBox cmbDeviceList;
        private Be.Windows.Forms.HexBox hexbData;
        private System.Windows.Forms.RadioButton rb562;
        private System.Windows.Forms.RadioButton rbReturns;
    }
}