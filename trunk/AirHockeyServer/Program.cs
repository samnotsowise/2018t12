/*
 *      AirHockeyServer Program Class
 * 
 * Description:
 *          Class to start a server, and 
 *          read in/send player co ordinates
 *      
 * Author:
 *      Gary Tyre
 */

using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;

namespace AirHockeyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int intPort = 2000;
            // create a configuration for the server
            NetConfiguration config = new NetConfiguration("T12Hockey");
            config.MaxConnections = 128;
            config.Port = intPort;

            // create server and start listening for connections
            NetServer server = new NetServer(config);
            server.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
            server.Start();

            // create a buffer to read data into
            NetBuffer buffer = server.CreateBuffer();

            // keep running until the user presses a key
            Console.WriteLine("Press ESC to quit server");
            Console.WriteLine("AirHockeyX Game Sever Started on Port " + intPort.ToString());
            Console.WriteLine("Host Name of Current Machine = " + System.Net.Dns.GetHostName());
            bool keepRunning = true;
            while (keepRunning)
            {
                NetMessageType type;
                NetConnection sender;

                // check if any messages has been received
                while (server.ReadMessage(buffer, out type, out sender))
                {
                    switch (type)
                    {
                        case NetMessageType.DebugMessage:
                            //Console.WriteLine(buffer.ReadString());
                            break;
                        case NetMessageType.ConnectionApproval:
                            //Console.WriteLine("Approval; hail is " + buffer.ReadString());
                            sender.Approve();
                            break;
                        case NetMessageType.StatusChanged:
                            string statusMessage = buffer.ReadString();
                            NetConnectionStatus newStatus = (NetConnectionStatus)buffer.ReadByte();
                            //Console.WriteLine("New status for " + sender + ": " + newStatus + " (" + statusMessage + ")");
                            break;
                        case NetMessageType.Data:
                            // A client sent this data!
                            int userId = buffer.ReadInt32();
                            int x = buffer.ReadInt32();
                            int y = buffer.ReadInt32();

                            //string msg = "User Id " + userId.ToString() + " sent co-ords X:" + x.ToString() + " Y:" + y.ToString();
                            //Console.WriteLine(msg);
                            // send to everyone, including sender
                            NetBuffer sendBuffer = server.CreateBuffer();
                            sendBuffer.Write(userId);
                            sendBuffer.Write(x);
                            sendBuffer.Write(y);

                            // send using ReliableInOrder
                            server.SendToAll(sendBuffer, NetChannel.ReliableInOrder1);

                            //Console.WriteLine("Sending Co-ords for user " + userId + " - X:" + x.ToString() + " Y:" + y.ToString() + " to all users
                            break;
                    }
                }

                // User pressed ESC?
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo info = Console.ReadKey();
                    if (info.Key == ConsoleKey.Escape)
                        Console.WriteLine("Exiting Server...");
                        keepRunning = false;
                }

                Thread.Sleep(1);
            }

            server.Shutdown("Application exiting");
        }
    }
}