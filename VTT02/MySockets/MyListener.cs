using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace VTT02.MySockets
{
    class MyListener
    {
#region Global Değişkenler
// --------------------------------------------------------------------------------------------------------------------------------------- 
        Socket               SocketListener, SocketClient;
        int                  Port;
        byte[]               Buffer = new byte[1024];

        public delegate void DlgRecivedOk(string ReceivedString, string IPAdress);
        public DlgRecivedOk  ReceivedOk;
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion

#region MyListener Class'ını kur
// --------------------------------------------------------------------------------------------------------------------------------------- 
        public MyListener(int PortNo)
        {
            this.Port                                       = PortNo;
            SocketListener                                  = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion

#region Start Listen - Dinlemeyi Başlat
// --------------------------------------------------------------------------------------------------------------------------------------- 
        public void StartListen()
        {
            SocketListener.Bind(new IPEndPoint(IPAddress.Any, this.Port));
            
// Kaç adet bağlantıyı kabul edecek ?
            SocketListener.Listen(100);

// Bağlantıyı kabul etmeyi başlat
            SocketListener.BeginAccept(new AsyncCallback(OnAccept), null);
        }
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion

#region OnAccept - Bağlantı kabul edildi
// --------------------------------------------------------------------------------------------------------------------------------------- 
        private void OnAccept(IAsyncResult ar)
        {
// Bağlantıyı sonlandır & Soketi ClientSocket'e aktar.
            SocketClient                                    = SocketListener.EndAccept(ar);

// Start Data Receive
            StartDataReceive();
        }
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion

#region Start Data Receive - Byte Array almayı başlat
// --------------------------------------------------------------------------------------------------------------------------------------- 
        private void StartDataReceive()
        {
            SocketClient.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion

#region OnReceive - Byte Array alındı
// --------------------------------------------------------------------------------------------------------------------------------------- 
        private void OnReceive(IAsyncResult ar)
        {
// Almayı sonlandır. & Gelen byte'ın boyutu
            int Length                                      = SocketClient.EndReceive(ar);

// Bağlantı koptu
            if (Length <= 0)                                                                   
                return;

// Gelen byte'ı işle & String'e çevir.
            byte[] SizedBuffer                              = new byte[Length];
            Array.Copy(Buffer, SizedBuffer, Length);

// ReceivedOk Evet'ını tetikle & Strin'i dışarıya ver
            ReceivedOk(Encoding.ASCII.GetString(SizedBuffer), IPAddress.Parse(((IPEndPoint)SocketClient.RemoteEndPoint).Address.ToString()).ToString());

// Data Receive tekrar başlat
            SocketClient.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
// --------------------------------------------------------------------------------------------------------------------------------------- 
#endregion
    }
}
