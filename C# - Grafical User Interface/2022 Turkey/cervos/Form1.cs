using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using System.Globalization;
using GMap.NET.MapProviders;
using SimpleTcp;
using System.IO;
using WinSCP;
using GMap.NET;
using Emgu.CV;
using Emgu.CV.Structure;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using System.IO.Ports;

namespace cervos
{
    public partial class mainForm : Form
    {
        
        #region TANIMLAMALAR

        string textname;
        string output;
        string videoPath;
        string videoName;
        string durumBilgisi;
        string savePath = Environment.CurrentDirectory;
        string[] liste;

        int counterAskida;
        int virgul;
        int counter;
        int ftpSayac;
        int sayac;
        int imuCounter;
        int k;

        long videoSize;

        bool _recording = true;
        bool ayrilma = false;
        bool aktifInis = false;
        bool askidaKalma = false;
        bool videoGonderim = false;
        bool CameraFlag = false;

        public static SimpleTcpClient client;

        StringBuilder stringBuffer = new StringBuilder();
        PointLatLng point1 = new PointLatLng();
        PointLatLng point2 = new PointLatLng();
        public static SerialPort port;
        Session FTPsession;
        Capture _capture;
        VideoWriter video_writer;

        #endregion


        #region THREADS

        Thread thStatu;
        Thread thCamera;
        Thread thChart;
        Thread thMap;
        Thread thDataGrid;
        Thread thIMU;
        Thread thSendVideo;
        Thread thGauge;


        #endregion

        #region FTP SETUP

        SessionOptions sessionOptions = new SessionOptions
        {
            Protocol = Protocol.Ftp,
            HostName = "192.168.4.1",
            UserName = "micro",
            Password = "python",
            FtpMode = FtpMode.Passive,

            TimeoutInMilliseconds = 2000,

        };

        #endregion
        
        public mainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            #region SERIAL PORT SETUP

            comboBox1.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            #endregion

            #region FORM SETUP 
            WindowState = FormWindowState.Maximized;
            this.KeyDown += mainForm_KeyDown;
            this.KeyPreview = true;


            tikAyrilma.Visible = false;
            tikVideo.Visible = false;
            tikAskida.Visible = false;
            txtBuild();

            #endregion

            #region HARITA SETUP
            
             gMap.DragButton = MouseButtons.Left;
             gMap.MapProvider = GMapProviders.GoogleMap;
             GMaps.Instance.Mode = AccessMode.ServerAndCache;
             gMap.MinZoom = 10;
             gMap.MaxZoom = 200;
             gMap.Zoom = 18;
             gMap.Position = new PointLatLng(38.349911,33.985416);

            #endregion

            #region GRAFIKLER SETUP
            /*
            chart1.ChartAreas[0].AxisY.Minimum = 90765;
            chart1.ChartAreas[0].AxisY.Maximum = 90375;

            chart4.ChartAreas[0].AxisY.Minimum = 27.8;
            chart4.ChartAreas[0].AxisY.Maximum = 29.8;

            chart5.ChartAreas[0].AxisY.Minimum = 8;
            chart5.ChartAreas[0].AxisY.Maximum = 9;
            */

            #endregion


        }


        //..............................VERI ALMA..............................

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    String newData = serialPort1.ReadExisting();

                    runInterface(newData);

                });

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {

                    String newData = Encoding.UTF8.GetString(e.Data);


                    runInterface(newData);


                });
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }



        #region THREADS

        private void durumPaneli()
        {
            try
            {

                durumBilgisi = liste[17];
                statu(durumBilgisi);
                durum.Text = "DURUM: " + durumBilgisi;

                if (ayrilma == true)        // ayrilma bilgisi
                {
                    tikAyrilma.Visible = true;
                    carpiAyrilma.Visible = false;
                    ayrilma = false;
                }
                
                if (askidaKalma)
                {
                    if (counterAskida < 10)
                    {
                        counterAskida += 1;
                    }
                    sayacAskida.Text = counterAskida + "sn";
                }
                


                if (liste[22] == "evet")
                {
                    videoGonderim = false;
                    tikVideo.Visible = true;
                    carpiVideo.Visible = false;
                    videoProgress.Value = 100;
                    sayacVideo.Text = "%100";
                }

                if (videoGonderim)
                {
                    if(videoProgress.Value < 95)
                    {
                        videoProgress.PerformStep();
                        k+=5;
                        sayacVideo.Text = "%" + k.ToString();
                    }
                    else
                    {
                        videoGonderim = false;
                    }

                }

                if (counterAskida == 10)
                {

                    tikAskida.Visible = true;
                    askidaKalma = false;

                }


            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }
        private void setCamera()
        {
            try
            {
                if (_capture == null)
                {
                    _capture = new Capture(0);
                }
                _capture.ImageGrabbed += _capture_ImageGrabbed;
                _capture.Start();
                CameraFlag = true;
                string path = savePath + @"\Saves\" + DateTime.Now.ToString().Replace(' ', '-').Replace(':', ';') + @" output.avi";
                video_writer = new VideoWriter(path, VideoWriter.Fourcc('M', 'P', '4', 'V'), 30, new Size(640, 480), true);
                _recording = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void _capture_ImageGrabbed(object sender, System.EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                _capture.Retrieve(m);
                picOutput.Image = m.ToImage<Bgr, byte>().Bitmap;
                picOutput.SizeMode = PictureBoxSizeMode.Zoom;
                if (_recording && video_writer != null)
                {
                    video_writer.Write(m);
                }
                Thread.Sleep(30);


            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }
        private void plotGraph()
        {
            try
            {
                if (!backgroundWorker2.IsBusy)
                {
                    backgroundWorker2.RunWorkerAsync();
                }

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }
        private void drawMap()
        {

            try
            {
                
                double lat1 = double.Parse(liste[12], CultureInfo.InvariantCulture);
                double lat2 = double.Parse(liste[15], CultureInfo.InvariantCulture);
                double long1 = double.Parse(liste[11], CultureInfo.InvariantCulture);
                double long2 = double.Parse(liste[14], CultureInfo.InvariantCulture);

                
                gMap.Position = new PointLatLng(lat1, long1);

                gMap.Overlays.Clear();

                point1.Lat = lat1;
                point1.Lng = long1;
                point2.Lat = lat2;
                point2.Lng = long2;

                GMarkerGoogle marker = new GMarkerGoogle(point1, GMarkerGoogleType.orange_dot);
                GMarkerGoogle marker2 = new GMarkerGoogle(point2, GMarkerGoogleType.blue_dot);

                GMapOverlay markers = new GMapOverlay("markers");
                gMap.Overlays.Add(markers);
                markers.Markers.Add(marker);
                markers.Markers.Add(marker2);

                
                gMap.Refresh();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }
        
        private void dataGrid()
        {

            try
            {

                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                dataGridView2[0, dataGridView2.RowCount - 1].Selected = true;

                dataGridView2.Rows.Add();


                for (int i = 0; i < 23; i++)
                {
                    dataGridView2.Rows[counter].Cells[i].Value = liste[i];
                }


                if (counter>=4)
                {
                    dataGridView2.Rows.RemoveAt(0);
                }
                else
                {
                    counter++;
                }
           
                


                


            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }


        }
        private void imuSimulation()
        {
            try
            {

                Invoke(new Action(() =>
                {
                    simulasyon.Rotate(double.Parse(liste[19], CultureInfo.InvariantCulture), double.Parse(liste[18], CultureInfo.InvariantCulture), double.Parse(liste[20], CultureInfo.InvariantCulture));
                    textBox5.Text = liste[19];
                    textBox3.Text = liste[18];
                    textBox4.Text = liste[20];


                }));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void sendVideo()
        {
            try
            {

                TransferOptions transferOptions = new TransferOptions();

                transferOptions.TransferMode = TransferMode.Binary;
                transferOptions.OverwriteMode = OverwriteMode.Append;

                TransferOperationResult transferResult;
                transferResult = FTPsession.PutFiles(@videoPath, "/sd/" + videoName, false, transferOptions);
                transferResult.Check();
                tikVideo.Visible = true;
                sayacVideo.Text = "%100";
                videoGonderim = false;
                videoProgress.Value = 100;
                serialPort1.Write("CMD,V,}");  // video evet



            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }


        }


        #endregion

        #region BUTTONS

        private void baglan_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "" || comboBox2.Text == "")
                {
                    MessageBox.Show("Hata!: COM Port ve Baud Rate Seçiniz ");
                }
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = int.Parse(comboBox2.Text);
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;

                    serialPort1.Open();
                    port = serialPort1;

                    if (serialPort1.IsOpen)
                    {
                        checkConnect.Visible = true;
                        baglan.Enabled = false;
                    }
                }
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void balantiyikes_Click(object sender, EventArgs e)
        {
            try
            {
                checkConnect.Visible = false;
                baglan.Enabled = true;
                serialPort1.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void baslat_Click(object sender, EventArgs e)
        {
            try
            {
                thCamera = new Thread(new ThreadStart(setCamera));
                thCamera.Start();
                Thread.Sleep(50);
                
                baslat.Enabled = false;
                durdur.Enabled = true;

                string komut = "CMD,~," + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "}\n";
                serialPort1.Write(komut);

                serialPort1.DataReceived += SerialPort1_DataReceived;

                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void durdur_Click(object sender, EventArgs e)
        {
            try
            {
                _capture.Stop();
                _recording = false;
                CameraFlag = false;
                durdur.Enabled = false;
                baslat.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void videoGonder_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();

                if (file.ShowDialog() == DialogResult.OK)
                {
                    videoPath = file.FileName;
                    videoName = file.SafeFileName;
                    FileInfo fi = new FileInfo(videoPath);
                    videoSize = fi.Length;

                    MessageBox.Show("Gönderilmek istenen dosya:" + videoPath, "", MessageBoxButtons.OKCancel);

                }
                backgroundWorker1.RunWorkerAsync();

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void telnetSwitch_Click_1(object sender, EventArgs e)
        {
            try
            {
                checkConnect.Visible = false;
                noWifi.Visible = true;
                serialPort1.Close();
                client = new SimpleTcpClient(txtIP.Text);
                client.Connect();

                if (client.IsConnected)
                {
                    telnetSwitch.Visible = false;
                    serialSwitch.Visible = true;
                    telorseri.Text = "Telnet";


                    checkConnect.Visible = true;
                    noWifi.Visible = false;

                    Thread.Sleep(100);
                    client.Send("micro" + "\r\n");
                    Thread.Sleep(50);
                    client.Send("python" + "\r\n");

                    client.Send("telemetri = True" + "\r\n");
                    client.Events.DataReceived += Events_DataReceived;
                }
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void serialSwitch_Click_1(object sender, EventArgs e)
        {
            try
            {
                client.Send("telemetri = False" + "\r\n");

                client.Disconnect();



                if (comboBox1.Text == "" || comboBox2.Text == "")
                {
                    MessageBox.Show("Hata!: COM Port ve Baud Rate Seçiniz ");
                }
                else
                {
                    telnetSwitch.Visible = true;
                    serialSwitch.Visible = false;

                    telorseri.Text = "Serial Data";
                    checkConnect.Visible = false;
                    noWifi.Visible = true;

                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = int.Parse(comboBox2.Text);
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;

                    serialPort1.Open();
                    port = serialPort1;

                    if (serialPort1.IsOpen)
                    {
                        checkConnect.Visible = true;
                        noWifi.Visible = false;

                        serialPort1.DataReceived += SerialPort1_DataReceived;
                    }
                }
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void piclock_Click(object sender, EventArgs e)
        {
            rj0.Enabled = true;
            rj1.Enabled = true;
            rj2.Enabled = true;
            rj3.Enabled = true;
            rj4.Enabled = true;
            rj5.Enabled = true;
            rj6.Enabled = true;
            rj7.Enabled = true;
            piclock.Visible = false;
            unlock.Visible = true;

        }

        private void unlock_Click(object sender, EventArgs e)
        {
            rj0.Enabled = false;
            rj1.Enabled = false;
            rj2.Enabled = false;
            rj3.Enabled = false;
            rj4.Enabled = false;
            rj5.Enabled = false;
            rj6.Enabled = false;
            rj7.Enabled = false;
            piclock.Visible = true;
            unlock.Visible = false;
        }



        #endregion

        #region BACKGROUND WORKERS

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                
                FTPsession = new Session();

                FTPsession.Open(sessionOptions);
                thSendVideo = new Thread(new ThreadStart(sendVideo));
                thSendVideo.IsBackground = true;
                thSendVideo.Start();
                videoGonderim = true;

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }            
        }
        

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                BeginInvoke((Action)(() =>
                {

                    if (double.Parse(liste[3], CultureInfo.InvariantCulture) > basinc.ChartAreas[0].AxisY.Maximum)
                    {
                        basinc.ChartAreas[0].AxisY.Maximum = double.Parse(liste[3], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[4], CultureInfo.InvariantCulture) > basinc.ChartAreas[0].AxisY.Maximum)
                    {
                        basinc.ChartAreas[0].AxisY.Maximum = double.Parse(liste[4], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[5], CultureInfo.InvariantCulture) > yuksek.ChartAreas[0].AxisY.Maximum)
                    {
                        yuksek.ChartAreas[0].AxisY.Maximum = double.Parse(liste[5], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[6], CultureInfo.InvariantCulture) > yuksek.ChartAreas[0].AxisY.Maximum)
                    {
                        yuksek.ChartAreas[0].AxisY.Maximum = double.Parse(liste[6], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[8], CultureInfo.InvariantCulture) > hiz.ChartAreas[0].AxisY.Maximum)
                    {
                        hiz.ChartAreas[0].AxisY.Maximum = double.Parse(liste[8], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[9], CultureInfo.InvariantCulture) > sicaklikChart.ChartAreas[0].AxisY.Maximum)
                    {
                        sicaklikChart.ChartAreas[0].AxisY.Maximum = double.Parse(liste[9], CultureInfo.InvariantCulture);
                    }
                    if (double.Parse(liste[10], CultureInfo.InvariantCulture) > pil.ChartAreas[0].AxisY.Maximum)
                    {
                        pil.ChartAreas[0].AxisY.Maximum = double.Parse(liste[10], CultureInfo.InvariantCulture);
                    }

                    basinc.Series.FindByName("Görev Yükü").Points.AddXY(liste[1], liste[3]);
                    basinc.Series.FindByName("Taşıyıcı").Points.AddXY(liste[1], liste[4]);
                    yuksek.Series.FindByName("Görev Yükü").Points.AddXY(liste[1], liste[5]);
                    yuksek.Series.FindByName("Taşıyıcı").Points.AddXY(liste[1], liste[6]);
                    hiz.Series.FindByName("Görev Yükü").Points.AddXY(liste[1], liste[8]);
                    sicaklikChart.Series.FindByName("Görev Yükü").Points.AddXY(liste[1], liste[9]);
                    pil.Series.FindByName("Görev Yükü").Points.AddXY(liste[1], liste[10]);
                    label13.Text = "Yükseklik Farkı: " + liste[7];
                }));
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }


        }

        private void Gauge()
        {
            try
            {

                radialGauge1.Value = Math.Abs(float.Parse(liste[8], CultureInfo.InvariantCulture));
                radialGauge3.Value = float.Parse(liste[10], CultureInfo.InvariantCulture);

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }

        }



        #endregion

        #region COMMANDS


        private void buzzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,Z}\n");
        }

        private void paketleriSifirlaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string komut = "CMD,~," + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "}\n";
            serialPort1.Write(komut);
        }
        private void güvenliBaşlatToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                serialPort1.DataReceived += SerialPort1_DataReceived;
                baslat.Enabled = false;
                durdur.Enabled = true;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }



        }

        private void ayrilmaa_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CMD,#}\n");
                ayrilma = true;
                tahrik.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void tahrik_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,?}\n");

        }

        private void motorlaridurdur_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,X}\n");

        }

        

        private void rj1_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,B}\n");

        }

        private void rj2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,I}\n");

        }

        private void rj3_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,U}\n");

        }

        private void rj4_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,D}\n");

        }

        private void rj5_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,E}\n");

        }

        private void rj6_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,L}\n");

        }

        private void rj7_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,Y}\n");

        }

        private void rj0_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,S}\n");

        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.PageUp)
            {
                menuStrip1.Visible = false;
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                menuStrip1.Visible = true;
            }

            if (e.Control && e.KeyCode == Keys.X)
            {
                serialPort1.Write("CMD,X}\n");
            }


        }

        #endregion

        #region FUNCTIONS
        private void runInterface(String newData)
        {
            try
            {
                //textBox1.Text = newData;


                foreach (char c in newData)
                {

                    if (c == ',') virgul++;

                    if (c == 10)
                    {
                        saveText(stringBuffer.ToString());

                        if (virgul == 22)
                        {


                            liste = stringBuffer.ToString().Split(',');

                            thStatu = new Thread(new ThreadStart(durumPaneli));
                            thStatu.Start();
                            Thread.Sleep(30);

                            thMap = new Thread(new ThreadStart(drawMap));
                            thMap.Start();
                            Thread.Sleep(30);

                            thChart = new Thread(new ThreadStart(plotGraph));
                            thChart.Start();
                            Thread.Sleep(30);

                            thIMU = new Thread(new ThreadStart(imuSimulation));
                            thIMU.Start();
                            Thread.Sleep(30);

                            thDataGrid = new Thread(new ThreadStart(dataGrid));
                            thDataGrid.Start();
                            Thread.Sleep(30);

                            thGauge = new Thread(new ThreadStart(Gauge));
                            thGauge.Start();
                            Thread.Sleep(30);

                        }
                    }
                    else if(c == 36) // $
                    {
                        virgul = 0;
                        stringBuffer = new StringBuilder();
                    }
                    else
                    {
                        stringBuffer.Append(c);
                    }
                }
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }

        }

        private void statu(string stat)
        {
            try
            {
                if (stat == "Ayrilma")
                {
                    ayrilma = true;
                }
                else if (stat == "Gorev_Yuku_inis")
                {
                    aktifInis = true;
                }
                else if (stat == "Askida_Kalma")
                {
                    askidaKalma = true;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            

        }
        void txtBuild()
        {
            try
            {
                textname = DateTime.Now.ToString().Replace(' ', '-');
                textname = textname.Replace(':', ';');
                textname = string.Concat(textname, ".csv");


                output = string.Concat(textname);

                output = savePath + @"\Saves\" + output;
                saveText("Takim No, Paket No, Zaman, Basinc1, Basinc2, Yukseklik1 , yukseklik2, Irtifa Farki, Inis Hizi, Sicaklik, Pil Gerilimi, GPS Longtitude1, GPS Latitude1, GPS Altitude1, GPS Latitude2, GPS Longtitude, GPS Altitude2, Uydu Statusu, Pitch, Roll, Yaw, Donus Sayisi, Video Aktarim Bilgisi");

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        void saveText(String text)
        {
            try
            {
                StreamWriter writer = new StreamWriter(output, true);

                writer.WriteLine(text);
                writer.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void sendToPy(String cmd) 
        {
            try
            {
                client.Send("uart.write(\"" + cmd + "\") \r\n");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void refreshimu_Click(object sender, EventArgs e)
        {
            simulasyon.init3D();
        }

        private void mPUKalibrasyonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,K}\n");
        }

       

        private void kamerayıDurdurToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write("CMD,H}\n");
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CameraFlag)
            {
                _capture.Stop();
            }
                
        }

        

        #endregion





        //....................ARSIVDEN VERI CEKME...........................
        /*
        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog kukla = new OpenFileDialog();
            if (kukla.ShowDialog() == DialogResult.OK)
            {
                
            }

            string DosyaYolu = kukla.FileName;
            string DosyaAdi = kukla.SafeFileName;

            StreamReader sr = new StreamReader(@DosyaYolu);

            String allData = sr.ReadToEnd();
            String[] lines = allData.Split('\n');

            foreach (String line in lines)
            {

                if (line.StartsWith("74575"))
                {
                    stringBuffer = new StringBuilder();
                    virgul = 0;


                    foreach (char c in line)
                    {

                        if (c == ',') virgul++;

                        if (c == 32)
                        {
                            saveText(stringBuffer.ToString());

                            if (virgul == 16)
                            {

                                liste = stringBuffer.ToString().Split(',');

                                thStatu = new Thread(new ThreadStart(durumPaneli));
                                thStatu.Start();
                                Thread.Sleep(50);

                                thChart = new Thread(new ThreadStart(plotGraph));
                                thChart.Start();
                                Thread.Sleep(50);

                                thMap = new Thread(new ThreadStart(setMap));
                                thMap.Start();
                                Thread.Sleep(50);

                                thDataGrid = new Thread(new ThreadStart(dataGrid));
                                thDataGrid.Start();
                                Thread.Sleep(50);

                                thIMU = new Thread(new ThreadStart(imuSimulation));
                                thIMU.Start();
                                Thread.Sleep(50);
                            }
                        }
                        else
                        {
                            stringBuffer.Append(c);
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }*/





    }
}

