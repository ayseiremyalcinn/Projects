namespace serialData
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel4 = new System.Windows.Forms.Panel();
            this.wifi = new System.Windows.Forms.PictureBox();
            this.noWifi = new System.Windows.Forms.PictureBox();
            this.comports = new System.Windows.Forms.ComboBox();
            this.baudrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wifi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noWifi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(32)))), ((int)(((byte)(39)))));
            this.panel4.Controls.Add(this.wifi);
            this.panel4.Controls.Add(this.noWifi);
            this.panel4.Controls.Add(this.comports);
            this.panel4.Controls.Add(this.baudrate);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.buttonConnect);
            this.panel4.Controls.Add(this.btnDisconnect);
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Location = new System.Drawing.Point(3, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(430, 163);
            this.panel4.TabIndex = 1;
            // 
            // wifi
            // 
            this.wifi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wifi.Location = new System.Drawing.Point(6, 7);
            this.wifi.Name = "wifi";
            this.wifi.Size = new System.Drawing.Size(47, 33);
            this.wifi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.wifi.TabIndex = 11;
            this.wifi.TabStop = false;
            this.wifi.Visible = false;
            // 
            // noWifi
            // 
            this.noWifi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.noWifi.Location = new System.Drawing.Point(3, 7);
            this.noWifi.Name = "noWifi";
            this.noWifi.Size = new System.Drawing.Size(47, 33);
            this.noWifi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.noWifi.TabIndex = 10;
            this.noWifi.TabStop = false;
            // 
            // comports
            // 
            this.comports.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comports.FormattingEnabled = true;
            this.comports.Location = new System.Drawing.Point(144, 36);
            this.comports.Name = "comports";
            this.comports.Size = new System.Drawing.Size(117, 25);
            this.comports.TabIndex = 5;
            // 
            // baudrate
            // 
            this.baudrate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.baudrate.FormattingEnabled = true;
            this.baudrate.Items.AddRange(new object[] {
            "3200",
            "9600",
            "11520"});
            this.baudrate.Location = new System.Drawing.Point(144, 72);
            this.baudrate.Name = "baudrate";
            this.baudrate.Size = new System.Drawing.Size(117, 25);
            this.baudrate.TabIndex = 6;
            this.baudrate.Text = "11520";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Black", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(40, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "BoudRate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Black", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(72, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "PORT:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.AutoSize = true;
            this.buttonConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonConnect.Font = new System.Drawing.Font("Arial Black", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonConnect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonConnect.Location = new System.Drawing.Point(309, 48);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(69, 18);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.AutoSize = true;
            this.btnDisconnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisconnect.Font = new System.Drawing.Font("Arial Black", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDisconnect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDisconnect.Location = new System.Drawing.Point(300, 85);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(90, 18);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(266, 36);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(164, 79);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 250;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 162);
            this.Controls.Add(this.panel4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wifi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noWifi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox wifi;
        private System.Windows.Forms.PictureBox noWifi;
        protected System.Windows.Forms.ComboBox comports;
        public System.Windows.Forms.ComboBox baudrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label buttonConnect;
        private System.Windows.Forms.Label btnDisconnect;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer2;
    }
}

