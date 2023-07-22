using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VTT02.MySockets;

using System.Net.Sockets;
using System.Net;

namespace VTT02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        MyListener TCPListen                               = new MyListener(4545);
         
        private void button1_Click(object sender, EventArgs e)
        {
            TCPListen.StartListen();
            TCPListen.ReceivedOk += new MyListener.DlgRecivedOk(TCPOkumaOk);       
        }

        void TCPOkumaOk(string ReceiveString, string IPAdress)
        {
            textBox1.Text                                   = ReceiveString;
            textBox2.Text                                   = IPAdress;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Socket server;// Declare your Socket
            //IPEndPoint ip = new IPEndPoint(IPAddress.Parse("10.126.225.44"), 4545);
            IPEndPoint ip; // Declare your IPEndpoint

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ip = new IPEndPoint(IPAddress.Parse("192.168.0.46"), 4545);
            server.Connect(ip);

            if (server.Connected == true)
            {

                String szData = "00200001000000000000#";//Nutrunner Communication Start
                byte[] byData = Encoding.Default.GetBytes(szData);//System.Text.Encoding.ASCII.GetBytes(szData);

                int sent = server.Send(byData, SocketFlags.None);
            }
        }
    }
}
