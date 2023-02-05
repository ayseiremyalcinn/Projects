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
using Emgu.CV;
using Emgu.CV.Structure;
//using LiveCharts;
//using LiveCharts.Wpf;
//using LiveCharts.Defaults;
using System.Globalization;
using GMap.NET.MapProviders;
using SimpleTcp;
using System.IO;
using FluentFTP;
using WinSCP;
using System.Windows.Forms.DataVisualization.Charting;

namespace cervos
{
    public partial class mainForm : Form
    {
        #region Tanımlamalar
        string textname;
        string output;
        int virgul;
        string videoPath;
        string videoName;
        int counter;
        int counterInis;
        //int counterAskida;
        int counterVideo;
        Capture _capture;
        VideoWriter video_writer;
        String[] liste;
        String savePath = Environment.CurrentDirectory;
        public static SimpleTcpClient client;
        StringBuilder stringBuffer = new StringBuilder();
        SessionOptions sessionOptions;
        Session FTPsession;
        OpenFileDialog file;


        bool _recording = true;
        bool ayrilma = false;
        bool aktifInis = false;

        #endregion

        #region Grafik serileri
        /*LineSeries basincSeries = new LineSeries();
        LineSeries yukseklikSeries = new LineSeries();
        LineSeries hızSeries = new LineSeries();
        LineSeries sicaklikSeries = new LineSeries();
        LineSeries pil1sSeries = new LineSeries();*/
        #endregion

        #region THREADS
        Thread thStatu;
        Thread thCamera;
        Thread thChart;
        Thread thMap;
        Thread thDataGrid;
        Thread thIMU;
        Thread thSendVideo;
        #endregion


        public mainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            

            tikAyrilma.Visible = false;
            tikInis.Visible = false;
            tikVideo.Visible = false;
            tikAskida.Visible = false;
            txtBuild();

            #region GRAFIK SETUP

            /*
            //............. Grafik........................................

            basincChart.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
            basincSeries.Stroke= System.Windows.Media.Brushes.DeepSkyBlue;
            basincSeries.Fill = System.Windows.Media.Brushes.Transparent;

            yukseklikChart.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
            yukseklikSeries.Stroke = System.Windows.Media.Brushes.LightBlue;
            yukseklikSeries.Fill = System.Windows.Media.Brushes.Transparent;

            hizChart.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
            hızSeries.Stroke = System.Windows.Media.Brushes.Bisque;
            hızSeries.Fill = System.Windows.Media.Brushes.Transparent;

            sicaklikChart.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
            sicaklikSeries.Stroke = System.Windows.Media.Brushes.LightSalmon;
            sicaklikSeries.Fill = System.Windows.Media.Brushes.Transparent;

            pilChart.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
            pil1sSeries.Stroke = System.Windows.Media.Brushes.Salmon;
            pil1sSeries.Fill = System.Windows.Media.Brushes.Transparent;


            basincChart.Series = new SeriesCollection();
            basincChart.LegendLocation = LegendLocation.Top;
            basincSeries.Title = "Basınç (Pa)";
            basincChart.Series.Add(basincSeries);
            basincSeries.Values = new ChartValues<ObservablePoint>();

            yukseklikChart.Series = new SeriesCollection();
            yukseklikChart.LegendLocation = LegendLocation.Top;
            yukseklikSeries.Title = "Yükseklik (m)";
            yukseklikChart.Series.Add(yukseklikSeries);
            yukseklikSeries.Values = new ChartValues<ObservablePoint>();

            hizChart.Series = new SeriesCollection();
            hizChart.LegendLocation = LegendLocation.Top;
            hızSeries.Title = "İniş hızı (m/s)";
            hizChart.Series.Add(hızSeries);
            hızSeries.Values = new ChartValues<ObservablePoint>();

            sicaklikChart.Series = new SeriesCollection();
            sicaklikChart.LegendLocation = LegendLocation.Top;
            sicaklikSeries.Title = "Sıcaklık (Celsius)";
            sicaklikChart.Series.Add(sicaklikSeries);
            sicaklikSeries.Values = new ChartValues<ObservablePoint>();

            pilChart.Series = new SeriesCollection();
            pilChart.LegendLocation = LegendLocation.Top;
            pil1sSeries.Title = "Pil gerilimi (Volt)";
            pilChart.Series.Add(pil1sSeries);
            pil1sSeries.Values = new ChartValues<ObservablePoint>();
            */
            #endregion

            #region HARITA SETUP
            //............. MAP........................................
            gMap.DragButton = MouseButtons.Left;
            gMap.MapProvider = GMapProviders.GoogleMap;
            #endregion

            #region FTP SETUP

            sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "192.168.4.2",
                UserName = "micro",
                Password = "python",
            };

            FTPsession = new Session();


            #endregion

        }

        private void btnConnect_Click(object sender, EventArgs e)              // BAGLAN
        {
            try
            {
                String temp = string.Concat(txtIP.Text, ":");
                String ipport = string.Concat(temp, textBox2.Text);
                client = new SimpleTcpClient(ipport);
                client.Connect();

                if (client.IsConnected)
                {
                    checkConnect.Visible = true;
                    btnConnect.Enabled = false;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)            // BAGLANTIYI KES
        {
            client.Disconnect();
            checkConnect.Visible = false;
            btnConnect.Enabled = true;
        }

        private void btnStart_Click_1(object sender, EventArgs e)               // BASLAT
        {
            thCamera = new Thread(new ThreadStart(setCamera));
            thCamera.Start();
            Thread.Sleep(50);

            client.Events.DataReceived += Events_DataReceived;


            btnStart.Enabled = false;
            btnStop.Enabled = true;
            Thread.Sleep(100);

            client.Send("micro" + "\r\n");
            Thread.Sleep(100);
            client.Send("python" + "\r\n");

        }

        private void btnStop_Click_1(object sender, EventArgs e)                 // DURDUR
        {
            _capture.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;

        }

        //..............................VERI ALMA..............................

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                String newData = Encoding.UTF8.GetString(e.Data);


                if (newData.Contains("74575"))
                {
                    stringBuffer = new StringBuilder();
                    virgul = 0;


                    foreach (char c in newData)
                    {

                       
                        if (c == ',') virgul++;

                        if (c == '*')
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
                
            });
        }


        private void button2_Click(object sender, EventArgs e)                   // VIDEO AKTARIM BUTONU
        {
            OpenFileDialog ofd = new OpenFileDialog();
            file = ofd;
            if (file.ShowDialog() == DialogResult.OK)
            {
                videoPath = file.FileName;
                videoName = file.SafeFileName;
                MessageBox.Show("Gönderilmek istenen dosya:" + videoPath, "", MessageBoxButtons.OKCancel);
            }
            client.Send("video = True\n\r");
            backgroundWorker1.RunWorkerAsync();

        }



        //...............................................BACKGROUND WORKERS.....................................


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = "192.168.4.1",
                    UserName = "micro",
                    Password = "python",
                    //TimeoutInMilliseconds = 100000,

                };

                FTPsession = new Session();
                FTPsession.Open(sessionOptions);


                thSendVideo = new Thread(new ThreadStart(sendVideo));
                thSendVideo.Start();
                Thread.Sleep(50);



            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*sayacVideo.Text = $"% {e.ProgressPercentage}";
            videoProgress.Value = e.ProgressPercentage;
            videoProgress.Update();*/
            videoProgress.PerformStep();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tikVideo.Visible = true;
            sendToPy("V");
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                /*ObservablePoint basincPoint = new ObservablePoint(int.Parse(liste[1], CultureInfo.InvariantCulture), double.Parse(liste[3], CultureInfo.InvariantCulture));
                basincSeries.Values.Add(basincPoint);

                ObservablePoint yukseklikPoint = new ObservablePoint(int.Parse(liste[1], CultureInfo.InvariantCulture), double.Parse(liste[4], CultureInfo.InvariantCulture));
                yukseklikSeries.Values.Add(yukseklikPoint);

                ObservablePoint hızPoint = new ObservablePoint(int.Parse(liste[1], CultureInfo.InvariantCulture), double.Parse(liste[5], CultureInfo.InvariantCulture));
                hızSeries.Values.Add(hızPoint);

                ObservablePoint sicaklikPoint = new ObservablePoint(int.Parse(liste[1], CultureInfo.InvariantCulture), double.Parse(liste[6], CultureInfo.InvariantCulture));
                sicaklikSeries.Values.Add(sicaklikPoint);

                ObservablePoint pil1sPoint = new ObservablePoint(int.Parse(liste[1], CultureInfo.InvariantCulture), double.Parse(liste[7], CultureInfo.InvariantCulture));
                pil1sSeries.Values.Add(pil1sPoint);*/



                chart1.Series.FindByName("Basınç                         (Pa)").Points.AddXY(liste[1], liste[3]);
                chart1.ChartAreas[0].AxisY.Minimum = 85000;
                chart1.ChartAreas[0].AxisY.Maximum = 95000;
               // chart1.ChartAreas[0].AxisY.Interval = 100;
                chart2.Series.FindByName("Yükseklik                                 (m)").Points.AddXY(liste[1], liste[4]);
                chart3.Series.FindByName("İniş Hızı                 (m/s)").Points.AddXY(liste[1], liste[5]);
                chart4.Series.FindByName(" Sıcaklık                                 (Celsius)").Points.AddXY(liste[1], liste[6]);
                chart5.Series.FindByName("Pil Gerilimi                   (Volt)").Points.AddXY(liste[1], liste[7]);



            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
            
           
        }

        



        //...............................................THREADS.....................................


        private void durumPaneli()
        {
            try
            {
                durum.Text = "DURUM: " + liste[11];
                if (ayrilma)        // ayrilma bilgisi
                {
                    tikAyrilma.Visible = true;
                    carpiAyrilma.Visible = false;
                    ayrilma = false;
                }
                if (aktifInis)            // aktif inis bilgisi
                {
                    if (counterInis < 100)
                    {
                        counterInis += 25;
                    }
                    inisProgress.Value = (int)(400 - float.Parse(liste[4], CultureInfo.InvariantCulture));   // yaklasik deger buldu
                    sayacInis.Text = "%" + (inisProgress.Value/4);                    
                }

                if (liste[16] == "1")   //video aktarim bilgisi
                {
                    if (counterVideo < 100)
                    {
                        counterVideo += 10;
                    }
                    
                    videoProgress.PerformStep();
                    sayacVideo.Text = "%" + counterVideo;
                }
               

                if (inisProgress.Value == 100)
                {
                    tikInis.Visible = true;

                }

                if (videoProgress.Value == 100)
                {
                    
                    tikVideo.Visible = true;

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
                try
                {
                    if (_capture == null)
                    {
                        _capture = new Capture(0
                            );
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                _capture.ImageGrabbed += _capture_ImageGrabbed;
                _capture.Start();
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

        private void setMap()
        {
            try
            {
                gMap.Position = new GMap.NET.PointLatLng(double.Parse(liste[8], CultureInfo.InvariantCulture), double.Parse(liste[9], CultureInfo.InvariantCulture));
                
                gMap.MinZoom = 16;
                gMap.MaxZoom = 200;
                gMap.Zoom = 16;
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void dataGrid()
        {

            try
            {
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                dataGridView2[0, dataGridView2.RowCount - 1].Selected = true;
                dataGridView2.Rows.Add();

                dataGridView2.Rows[counter].Cells[0].Value = liste[0];
                dataGridView2.Rows[counter].Cells[1].Value = liste[1];
                dataGridView2.Rows[counter].Cells[2].Value = liste[2];
                dataGridView2.Rows[counter].Cells[3].Value = liste[3];
                dataGridView2.Rows[counter].Cells[4].Value = liste[4];
                dataGridView2.Rows[counter].Cells[5].Value = liste[5];
                dataGridView2.Rows[counter].Cells[6].Value = liste[6];
                dataGridView2.Rows[counter].Cells[7].Value = liste[7];
                dataGridView2.Rows[counter].Cells[8].Value = liste[8];
                dataGridView2.Rows[counter].Cells[9].Value = liste[9];
                dataGridView2.Rows[counter].Cells[10].Value = liste[10];
                dataGridView2.Rows[counter].Cells[11].Value = liste[11];
                dataGridView2.Rows[counter].Cells[12].Value = liste[12];
                dataGridView2.Rows[counter].Cells[13].Value = liste[13];
                dataGridView2.Rows[counter].Cells[14].Value = liste[14];
                dataGridView2.Rows[counter].Cells[15].Value = liste[15];
                dataGridView2.Rows[counter].Cells[16].Value = liste[16];

               
                counter++;
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
                    simulasyon.Rotate(double.Parse(liste[13], CultureInfo.InvariantCulture), double.Parse(liste[12], CultureInfo.InvariantCulture), double.Parse(liste[14], CultureInfo.InvariantCulture));
                    textBox3.Text = liste[12];
                    textBox4.Text = liste[14];
                    textBox5.Text = liste[13];

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

                TransferOperationResult transferResult;
                transferResult = FTPsession.PutFiles(@videoPath, "/sd/" + videoName, false, transferOptions);

                transferResult.Check();
                client.Connect();
                Thread.Sleep(50);
                client.Send("micro\r\n");
                Thread.Sleep(50);
                client.Send("python\r\n");
                Thread.Sleep(50);

                if (transferResult.IsSuccess) client.Send(" getvideoname('" + videoName + "')\r\n");
                client.Send("video = False\n\r");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        //...............................................KOMUTLAR.....................................

        private void button3_Click(object sender, EventArgs e)         //AYRILMA
        {
            try
            {
                sendToPy("A");
                tikAyrilma.Visible = true;
                carpiAyrilma.Visible = false;

                button1.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)        // MANUEL TAHRIK
        {
            try
            {
                sendToPy("?");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            sendToPy("B");  // bir
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            sendToPy("I");  // iki
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            sendToPy("U");   // uc
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            sendToPy("D");    // dort
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            sendToPy("E");   // bes
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            sendToPy("L");   // alti
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            sendToPy("Y");  // yedi
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            sendToPy("S");   // SIFIR
        }

        private void buzzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendToPy("Z");   // Buzzeri calistirir.
        }

        private void motorlarıDurdurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendToPy("O");   // Motorlari durdurur.
        }


        // .......................................FONKSIYONLAR.........................................

        private string statu(int value)
        {
            if(value == 0)
            {
                return "Bekleme";
            }
            else if(value == 1)
            {
                return "Yükselme";
            }
            else if (value == 2)
            {
                return "Model Uydu İniş";
            }
            else if (value == 3)
            {
                ayrilma = true;
                return "Ayrılma";
            }
            else if (value == 4)
            {
                aktifInis = true;
                return "Görev Yükü İniş";
            }
            else
            {
                return "Kurtarma";
            }
        }
        void txtBuild()
        {
            textname = DateTime.Now.ToString().Replace(' ', '-');
            textname = textname.Replace(':', ';');
            textname = string.Concat(textname, ".csv");


            output = string.Concat("Console", textname);

            output = savePath + @"\Saves\" + output;

        }

        void saveText(String text)
        {
            StreamWriter writer = new StreamWriter(output, true);

            writer.WriteLine(text);
            writer.Close();
        }

        
        private void sendToPy(String cmd)
        {
            try
            {
                client.Send("uart.write(\"" + cmd + "\")\r\n");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        
    }
}

