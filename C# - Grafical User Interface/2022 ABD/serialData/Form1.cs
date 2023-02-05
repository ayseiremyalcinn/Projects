using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serialData
{
    public partial class Form1 : Form
    {
        public static SerialPort port;
        public static string File_path = @"C:\Users\aysei\Desktop\Dokumanlar\UYDU\2022 CanSat\Kukla veri\containerTelemetry.txt";

        public static string File_path2 = @"C:\Users\aysei\Desktop\Dokumanlar\UYDU\2022 CanSat\Kukla veri\payloadTelemetry.txt";
        string[] contdata;
        string[] paydata;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (comports.Text == "")
                {
                    MessageBox.Show("Error!: Choose COM Port and Baud Rate!");
                }
                else
                {
                    serialPort1.PortName = comports.Text;
                    serialPort1.Parity = Parity.None;
                    serialPort1.StopBits = StopBits.One;
                    serialPort1.Open();
                    port = serialPort1;

                    if (serialPort1.IsOpen)
                    {
                        timer1.Start();
                        timer2.Start();
                       
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comports.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            comports.Items.AddRange(ports);
            contdata = File.ReadAllText(File_path).Split('\n');
            paydata = File.ReadAllText(File_path2).Split('\n');

        }
        int i = 0;  
        int j = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            serialPort1.WriteLine(contdata[i]);
            i++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            serialPort1.WriteLine(paydata[j]);
            j++;
        }
    }
}
