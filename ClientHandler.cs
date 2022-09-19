

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ITried
{
    public class ClientHandler
    {
        static int counter = 1;
        public int Id { get; set; }
        public string userName { get; set; }

        public bool Checked { get; set; }
        protected internal NetworkStream Stream { get; private set; }
        
        TcpClient client;
        ServerHandler server; // объект сервера
        
        public ClientHandler(TcpClient tcpClient , ServerHandler TCPserver)
        {
            client = tcpClient;
            server = TCPserver;
        }

        

        public void Process() {

            try
            {
                
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                userName = message;
                Id = counter;
                counter++;
                while (this.client.Connected)
                {

                }
               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                
            }
            finally {
                Close();
                Thread.CurrentThread.Abort();
            }
        }

        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
            server.RemoveConnection(this.Id);
        }
    }
}
