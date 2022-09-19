using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;

namespace ITried
{
    public class ServerHandler
    {

            ObservableCollection<ClientHandler> clients;
            public ServerHandler(ObservableCollection<ClientHandler> clientsList) {
            this.clients = clientsList;
            }

            static TcpListener Server; // сервер для прослушивания
            

            public void AddConnection(ClientHandler clientObject)
            {
               clients.Add(clientObject);
            }
            protected internal void RemoveConnection(int id)
            {
            // получаем по id закрытое подключение
            ClientHandler client = clients.FirstOrDefault(c => c.Id == id);
                // и удаляем его из списка подключений
                if (client != null)
                    clients.Remove(client);
                    
            }
            // прослушивание входящих подключений
            public void Listen()
            {
                try
                {
                    Server = new TcpListener(IPAddress.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory+"\\IPAdressConfig.txt"))), 8888);
                    Server.Start();
                    

                    while (true)
                    {
                        TcpClient tcpClient = Server.AcceptTcpClient();
                    //if (((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address == IPAddress.Parse(""))
                    //{

                    //}
                        ClientHandler clientObject = new ClientHandler(tcpClient,this);
                        Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                        
                        clientThread.Start();
                        
                        AddConnection(clientObject);
                        
                    }
                }
                catch (Exception ex)
                {
                      
                      Disconnect();
                }
            }

            // трансляция сообщения подключенным клиентам
            public void BroadcastMessage(string message,params int[] id)
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                for (int i = 0; i < clients.Count; i++)
                {
                    if (id.Contains(clients[i].Id) ) // если id клиента входит в условие
                    {
                        clients[i].Stream.Write(data, 0, data.Length); //передача данных
                        
                    }
                }
            }


                public void ShutdownClient(params int[] id)
                {
                        try
                        {
                                byte[] data = Encoding.Unicode.GetBytes("Shutdown");
                                for (int i = 0; i < clients.Count; i++)
                                {
                                    if (id.Contains(clients[i].Id)) // если id клиента не равно id отправляющего
                                    {
                                        clients[i].Stream.Write(data, 0, data.Length); //передача данных
                                        clients[i].Close();
                                        
                                    }
                                }
                         }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    
                }



              // отключение всех клиентов
              protected internal void Disconnect()
              {
                
                

                for (int i = 0; i < clients.Count; i++)
                {
                    clients[i].Close(); //отключение клиента
                }

                 Server.Stop(); //остановка сервера

              }
    }
}
