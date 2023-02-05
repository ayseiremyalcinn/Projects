using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace GCS_pdr
{
    public partial class Form1 : Form
    {

       
        string[] pressureDatas;

        MqttClient client;
        string clientId;
        string topic;
        string textname;
        string output;
        string savePath = Environment.CurrentDirectory;



        public static SerialPort port;
        int i;
        int counter = 0;
        int counter1 = 0;

        string[] containerTelemetry;
        string[] payloadTelemetry;

        #region THREAD

        Thread thChartC;
        Thread thChartP;
        Thread thIMU;
        Thread thMap;
        Thread thGauge;
        Thread thGridP;
        Thread thGridC;

        #endregion




        public Form1()
        {
            
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
       


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region SERIAL PORT SETUP

                comports.Items.Clear();
                String[] ports = SerialPort.GetPortNames();
                comports.Items.AddRange(ports);
                #endregion

                #region MQTT SETUP

                string BrokerAddress = "cansat.info";
                client = new MqttClient(BrokerAddress);

                clientId = Guid.NewGuid().ToString();
                client.Connect(clientId, "1099", "Toenvipy551");

                topic = "teams/1099";

                #endregion

                #region CHARTS SETUP

                chartTempC.ChartAreas[0].AxisY.Minimum = 19;
                chartTempC.ChartAreas[0].AxisY.Maximum = 25;

                chartVoltC.ChartAreas[0].AxisY.Minimum = 8;
                chartVoltC.ChartAreas[0].AxisY.Maximum = 8.5;

                chartTempP.ChartAreas[0].AxisY.Minimum = 19;
                chartTempP.ChartAreas[0].AxisY.Maximum = 25;

                chartVoltP.ChartAreas[0].AxisY.Minimum = 16.8;
                chartVoltP.ChartAreas[0].AxisY.Maximum = 16.9;

                #endregion

                #region MAP SETUP
                gMap.DragButton = MouseButtons.Left;
                gMap.MapProvider = GMapProviders.GoogleMap;
                gMap.MinZoom = 10;
                gMap.MaxZoom = 200;
                gMap.Zoom = 18;


                #endregion

                label6.Text = DateTime.Now.ToLongTimeString();

                txtBuild();

                radialGauge1.StartAngle = -90;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }


        }




        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {

                if (comports.Text == "")
                {
                    MessageBox.Show("Hata!: COM Port ve Baud Rate Seçiniz ");
                }
                else
                {

                    serialPort1.PortName = comports.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;

                    serialPort1.Open();
                    port = serialPort1;

                    if (serialPort1.IsOpen)
                    {
                        wifi.Visible = true;
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

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string newData = serialPort1.ReadExisting();

                if (newData != null)
                {


                    string[] telemetri = newData.Split(',');
                    if (telemetri.Length > 3)
                    {
                        saveText(newData);
                        client.Publish(topic, Encoding.UTF8.GetBytes(newData));
                        if (telemetri[3] == "T")
                        {
                            payloadTelemetry = telemetri;
                            if (telemetri.Length == 18)
                            {
                                thChartP = new Thread(new ThreadStart(drawChartP));
                                thChartP.Start();
                                Thread.Sleep(30);

                                thGauge = new Thread(new ThreadStart(Gauge));
                                thGauge.Start();
                                Thread.Sleep(30);

                                thIMU = new Thread(new ThreadStart(IMUsim));
                                thIMU.Start();
                                Thread.Sleep(30);

                                thGridP = new Thread(new ThreadStart(dataGridP));
                                thGridP.Start();
                                Thread.Sleep(30);
                            }



                        }
                        else if (telemetri[3] == "C")
                        {
                            containerTelemetry = telemetri;

                            if (telemetri.Length >= 16)
                            {
                                label7.Text = containerTelemetry[1];
                                label9.Text = containerTelemetry[14];

                                thChartC = new Thread(new ThreadStart(drawChartC));
                                thChartC.Start();
                                Thread.Sleep(30);

                                thMap = new Thread(new ThreadStart(setMap));
                                thMap.Start();
                                Thread.Sleep(30);

                                thGridC = new Thread(new ThreadStart(dataGridC));
                                thGridC.Start();
                                Thread.Sleep(30);
                            }

                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
                
            
        }

        void IMUsim()
        {
            try
            {
                chartGyro.Series.FindByName("ROLL").Points.AddXY(payloadTelemetry[2], payloadTelemetry[7]);
                chartGyro.Series.FindByName("PITCH").Points.AddXY(payloadTelemetry[2], payloadTelemetry[8]);
                chartGyro.Series.FindByName("YAW").Points.AddXY(payloadTelemetry[2], payloadTelemetry[9]);

                chartAccel.Series.FindByName("ROLL").Points.AddXY(payloadTelemetry[2], payloadTelemetry[10]);
                chartAccel.Series.FindByName("PITCH").Points.AddXY(payloadTelemetry[2], payloadTelemetry[11]);
                chartAccel.Series.FindByName("YAW").Points.AddXY(payloadTelemetry[2], payloadTelemetry[12]);

                chartMag.Series.FindByName("ROLL").Points.AddXY(payloadTelemetry[2], payloadTelemetry[13]);
                chartMag.Series.FindByName("PITCH").Points.AddXY(payloadTelemetry[2], payloadTelemetry[14]);
                chartMag.Series.FindByName("YAW").Points.AddXY(payloadTelemetry[2], payloadTelemetry[15]);
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void drawChartC()
        {
            try
            {


                if (double.Parse(containerTelemetry[6], CultureInfo.InvariantCulture) > chartAltitude.ChartAreas[0].AxisY.Maximum)
                {
                    chartAltitude.ChartAreas[0].AxisY.Maximum = double.Parse(containerTelemetry[6], CultureInfo.InvariantCulture);
                }
                if (double.Parse(containerTelemetry[6], CultureInfo.InvariantCulture) < chartAltitude.ChartAreas[0].AxisY.Minimum)
                {
                    chartAltitude.ChartAreas[0].AxisY.Minimum = double.Parse(containerTelemetry[6], CultureInfo.InvariantCulture);
                }
                if (double.Parse(containerTelemetry[7], CultureInfo.InvariantCulture) > chartTempC.ChartAreas[0].AxisY.Maximum)
                {
                    chartTempC.ChartAreas[0].AxisY.Maximum = double.Parse(containerTelemetry[7], CultureInfo.InvariantCulture);
                }
                if (double.Parse(containerTelemetry[7], CultureInfo.InvariantCulture) < chartTempC.ChartAreas[0].AxisY.Minimum)
                {
                    chartTempC.ChartAreas[0].AxisY.Minimum = double.Parse(containerTelemetry[7], CultureInfo.InvariantCulture);
                }
                if (double.Parse(containerTelemetry[8], CultureInfo.InvariantCulture) > chartVoltC.ChartAreas[0].AxisY.Maximum)
                {
                    chartVoltC.ChartAreas[0].AxisY.Maximum = double.Parse(containerTelemetry[8], CultureInfo.InvariantCulture);
                }
                if (double.Parse(containerTelemetry[8], CultureInfo.InvariantCulture) < chartVoltC.ChartAreas[0].AxisY.Minimum)
                {
                    chartVoltC.ChartAreas[0].AxisY.Minimum = double.Parse(containerTelemetry[8], CultureInfo.InvariantCulture);
                }

                BeginInvoke((Action)(() => {
                    chartAltitude.Series.FindByName("Altitude                                 (m)").Points.AddXY(containerTelemetry[2], containerTelemetry[6]);
                    chartTempC.Series.FindByName("Temperature                                 (Celsius)").Points.AddXY(containerTelemetry[2], containerTelemetry[7]);
                    chartVoltC.Series.FindByName("Voltage                   (Volt)").Points.AddXY(containerTelemetry[2], containerTelemetry[8]);
                }));
                

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void drawChartP()
        {
            try
            {

                if (double.Parse(payloadTelemetry[4], CultureInfo.InvariantCulture) > chartAltitudeP.ChartAreas[0].AxisY.Maximum)
                {
                    chartAltitudeP.ChartAreas[0].AxisY.Maximum = double.Parse(payloadTelemetry[4], CultureInfo.InvariantCulture);
                }
                if (double.Parse(payloadTelemetry[4], CultureInfo.InvariantCulture) < chartAltitudeP.ChartAreas[0].AxisY.Minimum)
                {
                    chartAltitudeP.ChartAreas[0].AxisY.Minimum = double.Parse(payloadTelemetry[4], CultureInfo.InvariantCulture);
                }
                if (double.Parse(payloadTelemetry[5], CultureInfo.InvariantCulture) > chartTempP.ChartAreas[0].AxisY.Maximum)
                {
                    chartTempP.ChartAreas[0].AxisY.Maximum = double.Parse(payloadTelemetry[5], CultureInfo.InvariantCulture);
                }
                if (double.Parse(payloadTelemetry[5], CultureInfo.InvariantCulture) < chartTempP.ChartAreas[0].AxisY.Minimum)
                {
                    chartTempP.ChartAreas[0].AxisY.Minimum = double.Parse(payloadTelemetry[5], CultureInfo.InvariantCulture);
                }
                if (double.Parse(payloadTelemetry[6], CultureInfo.InvariantCulture) > chartVoltP.ChartAreas[0].AxisY.Maximum)
                {
                    chartVoltP.ChartAreas[0].AxisY.Maximum = double.Parse(payloadTelemetry[6], CultureInfo.InvariantCulture);
                }
                if (double.Parse(payloadTelemetry[6], CultureInfo.InvariantCulture) < chartVoltP.ChartAreas[0].AxisY.Minimum)
                {
                    chartVoltP.ChartAreas[0].AxisY.Minimum = double.Parse(payloadTelemetry[6], CultureInfo.InvariantCulture);
                }

                BeginInvoke((Action)(() => {
                    chartAltitudeP.Series.FindByName("Altitude                                 (m)").Points.AddXY(payloadTelemetry[2], payloadTelemetry[4]);
                    chartTempP.Series.FindByName("Temperature                                 (Celsius)").Points.AddXY(payloadTelemetry[2], payloadTelemetry[5]);
                    chartVoltP.Series.FindByName("Voltage                   (Volt)").Points.AddXY(payloadTelemetry[2], payloadTelemetry[6]);

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
                radialGauge1.Value = float.Parse(payloadTelemetry[16], CultureInfo.InvariantCulture);
                radialGauge2.Value = float.Parse(payloadTelemetry[4], CultureInfo.InvariantCulture);
                radialGauge3.Value = float.Parse(payloadTelemetry[6], CultureInfo.InvariantCulture);
                radialGauge4.Value = float.Parse(payloadTelemetry[5], CultureInfo.InvariantCulture);
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
                GMaps.Instance.Mode = AccessMode.ServerAndCache;
                gMap.Position = new PointLatLng(double.Parse(containerTelemetry[11], CultureInfo.InvariantCulture), double.Parse(containerTelemetry[10], CultureInfo.InvariantCulture));
                label35.Text = containerTelemetry[13];
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }


        private void dataGridC()

        {

            try
            {

                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.RowCount - 1;
                dataGridView2[0, dataGridView2.RowCount - 1].Selected = true;
                dataGridView2.Rows.Add();


                dataGridView2.Rows[counter].Cells[0].Value = containerTelemetry[0];
                dataGridView2.Rows[counter].Cells[1].Value = containerTelemetry[1];
                dataGridView2.Rows[counter].Cells[2].Value = containerTelemetry[2];
                dataGridView2.Rows[counter].Cells[3].Value = containerTelemetry[3];
                dataGridView2.Rows[counter].Cells[4].Value = containerTelemetry[4];
                dataGridView2.Rows[counter].Cells[5].Value = containerTelemetry[5];
                dataGridView2.Rows[counter].Cells[6].Value = containerTelemetry[6];
                dataGridView2.Rows[counter].Cells[7].Value = containerTelemetry[7];
                dataGridView2.Rows[counter].Cells[8].Value = containerTelemetry[8];
                dataGridView2.Rows[counter].Cells[9].Value = containerTelemetry[9];
                dataGridView2.Rows[counter].Cells[10].Value = containerTelemetry[10];
                dataGridView2.Rows[counter].Cells[11].Value = containerTelemetry[11];
                dataGridView2.Rows[counter].Cells[12].Value = containerTelemetry[12];
                dataGridView2.Rows[counter].Cells[13].Value = containerTelemetry[13];
                dataGridView2.Rows[counter].Cells[14].Value = containerTelemetry[14];
                dataGridView2.Rows[counter].Cells[15].Value = containerTelemetry[15];

                counter++;
            }
            catch (Exception err)
            {

                MessageBox.Show("data gridde(C) sorun var");
            }

        }

        private void dataGridP()
        {

            try
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                dataGridView1[0, dataGridView1.RowCount - 1].Selected = true;
                dataGridView1.Rows.Add();


                dataGridView1.Rows[counter1].Cells[0].Value = payloadTelemetry[0];
                dataGridView1.Rows[counter1].Cells[1].Value = payloadTelemetry[1];
                dataGridView1.Rows[counter1].Cells[2].Value = payloadTelemetry[2];
                dataGridView1.Rows[counter1].Cells[3].Value = payloadTelemetry[3];
                dataGridView1.Rows[counter1].Cells[4].Value = payloadTelemetry[4];
                dataGridView1.Rows[counter1].Cells[5].Value = payloadTelemetry[5];
                dataGridView1.Rows[counter1].Cells[6].Value = payloadTelemetry[6];
                dataGridView1.Rows[counter1].Cells[7].Value = payloadTelemetry[7];
                dataGridView1.Rows[counter1].Cells[8].Value = payloadTelemetry[8];
                dataGridView1.Rows[counter1].Cells[9].Value = payloadTelemetry[9];
                dataGridView1.Rows[counter1].Cells[10].Value = payloadTelemetry[10];
                dataGridView1.Rows[counter1].Cells[11].Value = payloadTelemetry[11];
                dataGridView1.Rows[counter1].Cells[12].Value = payloadTelemetry[12];
                dataGridView1.Rows[counter1].Cells[13].Value = payloadTelemetry[13];
                dataGridView1.Rows[counter1].Cells[14].Value = payloadTelemetry[14];
                dataGridView1.Rows[counter1].Cells[15].Value = payloadTelemetry[15];
                dataGridView1.Rows[counter1].Cells[16].Value = payloadTelemetry[16];
                dataGridView1.Rows[counter1].Cells[17].Value = payloadTelemetry[17];

                counter1++;
            }
            catch (Exception err)
            {

                MessageBox.Show("data gridde(P) sorun var");
            }

        }
        /*
        private void Gyro()
        {
            double asd = double.Parse(payloadTelemetry[15]);
            asd = (asd * 3.14) / 180; 
            userControl11.PitchAngle = asd;

            asd = double.Parse(payloadTelemetry[7]);
            asd = (asd * 3.14) / 180; 
            userControl11.RollAngle = asd;

            asd = double.Parse(payloadTelemetry[8]);
            asd = (asd * 3.14) / 180; 
            userControl11.YawAngle = asd;

            userControl11.yenile();
        }*/



        private void buttonOn_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (wifi.Visible == true)
                {
                    serialPort1.Write("CMD,1099,CX,ON       ");
                    timer1.Start();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }
        private void buttonOff_Click_1(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CMD,1099,CX,OFF      ");
                timer1.Stop();
                //buttonOn.Visible = true;
                //buttonOff.Visible = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }


        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                wifi.Visible = false;
                noWifi.Visible = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] zaman = label6.Text.Split(':');
                int saat = Int32.Parse(zaman[0]);
                int dakika = Int32.Parse(zaman[1]);
                int saniye = Int32.Parse(zaman[2]);
                if (saniye < 59)
                {
                    saniye++;
                }
                else
                {
                    saniye = 0;
                    if (dakika < 59)
                    {
                        dakika++;
                    }
                    else
                    {
                        dakika = 0;
                        saat++;
                    }
                }
                label6.Text = saat + ":" + dakika + ":" + saniye;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
           
        }

        private void enable_sim_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CMD,1099,SIM,ENABLE  ");
                enable_sim.Visible = false;
                disenable_sim.Visible = true;
                active_sim.Enabled = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void active_sim_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CMD,1099,SIM,ACTIVATE");
                active_sim.Visible = false;
                label8.Visible = false;
                label12.Text = "SIMULATION DISABLE :";
                this.BackColor = Color.FromArgb(35, 32, 39);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void disenable_sim_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write("CMD,1099,SIM,DISABLE ");
                label12.Text = "SIMULATION ENABLE :";
                disenable_sim.Visible = false;
                enable_sim.Visible = true;
                active_sim.Visible = true;
                label8.Visible = true;
                panelScreen.BackColor = Color.FromArgb(20, 20, 20);
                timer2.Stop();
                textBox3.Text = "Upload the file to be used in the simulation.";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();

                if (file.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = file.SafeFileName;
                    string simulationFile = file.FileName;
                    pressureDatas = File.ReadAllLines(simulationFile);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (pressureDatas != null)
                {
                    timer2.Start();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (i < pressureDatas.Length)
                {
                    String bosluk;
                    int boslukSayisi = 6 - pressureDatas[i].Length;
                    if (boslukSayisi == 0) { bosluk = " "; }
                    else { bosluk = "  "; }
                    String komut = "CMD,1099,SIMP," + pressureDatas[i] + bosluk;
                    textBox3.Text = komut;
                    serialPort1.Write(komut);
                    i++;
                }
                else
                {
                    textBox3.Text = "SIMULATION MODE DONE!!";
                    timer2.Stop();
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

                writer.Write(text);
                writer.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void label17_Click(object sender, EventArgs e)
        {
            serialPort1.Write("#                    "); // ayrilma
        }
        

       
        private void label28_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write("S                    ");  // servo 1
        }

        private void label18_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write("U                    ");  // up
        }

        private void label29_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write("B                    ");   // servo 2
        }

        private void label30_Click_1(object sender, EventArgs e)
        {
            serialPort1.Write("I                    ");    //servo 3
        }

        private void label32_Click(object sender, EventArgs e)
        {
            serialPort1.Write("$                    ");   // makara sar
        }

      
        private void label10_Click(object sender, EventArgs e)
        {
            string zaman = "CMD,1099,ST," + DateTime.Now.ToLongTimeString() + " " ;
            serialPort1.Write(zaman);
        }

        private void label27_Click(object sender, EventArgs e)
        {
            serialPort1.Write("D                    ");   // down
        }

        private void label33_Click(object sender, EventArgs e)
        {
            serialPort1.Write("E                    ");  // servo 0
        }
              
    }
    
}
