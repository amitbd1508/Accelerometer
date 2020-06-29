using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GyroAndAcelero
{
    public partial class Accelerometer : Form
    {
        //SerialPort port;
        SerialPort port = new SerialPort();

        string comPort, RxString;
        string[] ArduinoData=null;
        string[] ports = SerialPort.GetPortNames();
        double sX, sY, sZ;

        
        double x = 200 - 25, y = 200 - 25;
        public Accelerometer()
        {
            InitializeComponent();
            
            foreach (string por in ports)
            {
                comboBox.Items.Add(por);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            x = (double)(200.0 / 17000.0) * sX-25+200;
            y = (double)(200.0 / 17000.0) * sZ-25+200;

            if (x < 0)
                x = x - 200;
            if (y < 0)
                y = y - 200;

           
            e.Graphics.FillRectangle(Brushes.Black, 0, 200, 400, 3);
            e.Graphics.FillRectangle(Brushes.Black, 200,0, 3, 400);
            e.Graphics.FillRectangle(Brushes.Red,(int)x,(int)y,50,50);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string t = comboBox.Text.ToString();
            btnOpen.Enabled = false;
            btnClose.Enabled = true;
            

        }
        
        
        
        
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                port.Close();
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Port Is open");
            }
            
            
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            comPort = Convert.ToString(comboBox.SelectedItem); // Set selected COM port

            try
            {
                //port = new SerialPort(comPort, 115200, Parity.None, 8, StopBits.One);

                port.PortName = comPort;
                port.BaudRate = 115200;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;

                port.Open();  // Open COM port

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect :: " + ex.Message, "Sorry, I just couldn't do it...");
            }

        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port.IsOpen == true)
            {
                RxString = port.ReadLine();
                richTextBox1.Text = RxString + "\n";
                ArduinoData = RxString.Split(',', '\n', '\r');

                if (ArduinoData.Count() == 4) //ensures we have all data, plus line end ("\n" or "\r")
                {
                    sX = Convert.ToDouble(ArduinoData[0]);
                    sY = Convert.ToDouble(ArduinoData[1]);
                    sZ = Convert.ToDouble(ArduinoData[2]);

                    Invalidate();
                }
            }
        }
    }
}
