using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using FootballPlayer;

namespace TCP_server
{
    class Program
    {
        private static int _nextId = 1;
        public static List<FootballPlayer.FootballPlayer> _list = new List<FootballPlayer.FootballPlayer>();
        static void Main(string[] args)
        {
            Console.WriteLine("Server is ready");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();

                while (true)
                {
                    TcpClient socket = listener.AcceptTcpClient();
                    Console.WriteLine("New client: " + socket.Client.RemoteEndPoint.ToString());


                Task.Run(() =>
                    {
                        HandleClient(socket);
                    });
                }

        }
            
            static void HandleClient(TcpClient socket)
            {
                NetworkStream ns = socket.GetStream();
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns);
                while (true)
                {
                    writer.WriteLine(
                        "Hejsan velkommen til min server håber du føler dig godt tilpas, " +
                        "\r\nher kommer der nogen kommandoer du kan bruge " +
                        "\r\nhyg dig peace jeg elsker kage");
                    writer.Flush();
                    writer.WriteLine(
                        "Kommandoer:\r\n" +
                        "add\r\n" +
                        "show\r\n" +
                        "id\r\n" +
                        "farvel");
                    writer.Flush();
                string message2 = reader.ReadLine();

                    if (message2.ToLower().StartsWith("add"))
                    {
                        writer.WriteLine("Tilfoj venligst din jsonstring");
                        writer.Flush();
                    //eksempel på string {"Id": 1, "Name": "Cola", "Price": 1, "ShirtNumber": 5}
                    string message = reader.ReadLine(); 
                        if (message.StartsWith("{")) 
                        {
                            FootballPlayer.FootballPlayer fromJson = JsonSerializer.Deserialize<FootballPlayer.FootballPlayer>(message); 
                            fromJson.Id = _nextId++; 
                            _list.Add(fromJson); 
                            writer.WriteLine("Det er modtaget");
                        }
                        else
                        { 
                            writer.WriteLine("Det skal være en jsonstring"); 
                            writer.Flush();
                        }
                    }


                    if (message2.ToLower().StartsWith("show"))
                    {
                        writer.WriteLine("viser din liste det var så lidt");
                        writer.Flush(); 
                        foreach (FootballPlayer.FootballPlayer value in _list)
                        {
                            Console.WriteLine($"Fra listen: ID: {value.Id} Name: {value.Name} Price: {value.Price} ShirtNumber: {value.ShirtNumber}");
                            writer.WriteLine(
                                $"Fra listen: ID: {value.Id} Name: {value.Name} Price: {value.Price} ShirtNumber: {value.ShirtNumber}");
                        writer.Flush();
                        }
                    }

                    if (message2.ToLower().StartsWith("id"))
                    {
                        writer.WriteLine("Venligst vælg en id");
                        writer.Flush(); 
                        string message = reader.ReadLine();

                        try
                        {

                            writer.WriteLine("Her den valgt objekt ud fra valgte id");
                            writer.Flush();
                            int number = int.Parse(message);
                            var result = _list.Find(x => x.Id.Equals(number));

                            Console.WriteLine($"Fra listen: ID: {result.Id} Name: {result.Name} Price: {result.Price} ShirtNumber: {result.ShirtNumber}");
                            writer.WriteLine(
                                $"Fra listen: ID: {result.Id} Name: {result.Name} Price: {result.Price} ShirtNumber: {result.ShirtNumber}");
                            writer.Flush();

                    }
                        catch (Exception e)
                        {
                            writer.WriteLine(e);
                            writer.Flush();
                    }





                        
                    }
                    if (message2.ToLower().StartsWith("farvel"))
                    {
                        writer.WriteLine("så skrid dog med dig :(");
                        writer.Flush();
                        socket.Close();
                        break;
                    }
            }
            }
    }
}
