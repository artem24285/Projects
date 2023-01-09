using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Accord.Video;

namespace Server
{
    internal class Program
    {
        public static bool x = false;
        public static int Team;
        public static IPEndPoint ClientPoint;
        public static IPEndPoint ClientPoint2;
        public static UdpClient udpClient = new UdpClient();
        public static FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);


        static async void Main(string[] args)
        {
            
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"Запуск сервера состоялся в {DateTime.Now}");

                do
                {
                    if (VideoDevices.Count > 0)
                    {
                        Console.WriteLine("\nПодключенная камера:\n");
                        foreach (FilterInfo Devices in VideoDevices)
                        {

                            Console.WriteLine($"{Devices.Name}");
                        }
                    }
                    var Host = Dns.GetHostEntry(Dns.GetHostName());
                    var ClientIP = ConfigurationManager.AppSettings.Get("ClientIP");
                    var ClientIP2 = ConfigurationManager.AppSettings.Get("ClientIP2");
                    var ClientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
                    var ClientPort2 = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort2"));
                    
                    ClientPoint = new IPEndPoint(IPAddress.Parse(ClientIP), ClientPort);
                    ClientPoint2 = new IPEndPoint(IPAddress.Parse(ClientIP2), ClientPort2);

                    if (VideoDevices.Count == 1)
                    {
                        Console.WriteLine($"\nПередача: {ClientPoint}");
                        Console.WriteLine($"\nПередача: {ClientPoint2}");
                    }
                    else if (VideoDevices.Count == 2)
                    {
                        Console.WriteLine($"\nПередача: {ClientPoint}");
                        Console.WriteLine($"\nПередача: {ClientPoint2}");
                    }

                    Console.Write("\nКоманды для сервера: ");
                    Console.WriteLine("\n1. Запустить сервер\n2. Остановить сервер");
                    Console.Write("\nВведите команду для сервера: ");
                    Team = Convert.ToInt16(Console.ReadLine());
                    switch (Team)
                    {
                        case 1:
                            {
                                if (VideoDevices.Count == 1)
                                {
                                    VideoCaptureDevice VideoSource = new VideoCaptureDevice(VideoDevices[0].MonikerString);
                                    VideoSource.NewFrame += VideoSource_NewFrame;
                                    VideoSource.Start();
                                    Console.Write("\nСервер запущен!!!\n");
                                    Console.Write("\nВведите команду для сервера: ");
                                    Team = Convert.ToInt16(Console.ReadLine());
                                    VideoSource.Stop();
                                    Console.WriteLine("\nСервер остановлен!!!");

                                }
                                if (VideoDevices.Count >= 1)
                                {
                                    VideoCaptureDevice VideoSource = new VideoCaptureDevice(VideoDevices[0].MonikerString);
                                    VideoCaptureDevice VidoeSource2 = new VideoCaptureDevice(VideoDevices[1].MonikerString);
                                    VideoSource.NewFrame += VideoSource_NewFrame;
                                    VidoeSource2.NewFrame += VideoSource2_NewFrame;
                                    VideoSource.Start();
                                    VidoeSource2.Start();
                                    Console.Write("\nСервер запущен!!!\n");
                                    Console.Write("\nВведите команду для сервера: ");
                                    Team = Convert.ToInt16(Console.ReadLine());
                                    VideoSource.Stop();
                                    VidoeSource2.Stop();
                                    Console.WriteLine("\nСервер остановлен!!!");

                                }
                                else if (VideoDevices.Count == 0)
                                {
                                    Console.WriteLine("\nПодключенных камер к серверу не обнаруженно!");
                                    Console.Write("\nВведите команду для сервера: ");
                                    Team = Convert.ToInt16(Console.ReadLine());
                                    Console.WriteLine("\nСервер остановлен");
                                }
                                break;
                            }

                        case 2:
                            {
                                Console.WriteLine("\nСервер остановлен");
                                udpClient.Close();
                                break;
                            }
                    }
                } while (Team != 2);

            }
           
        }
       
        //Главная часть видеонаблюдения   
        private static void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            var BMP = new Bitmap(eventArgs.Frame, 800, 600);

            try
            {
                using (var Ms = new MemoryStream())
                {
                    BMP.Save(Ms, ImageFormat.Jpeg);
                    var BYTES = Ms.ToArray();
                    udpClient.Send(BYTES, BYTES.Length, ClientPoint);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        //Главная часть видеонаблюдения при подключении двух и более клиентов
        private static void VideoSource2_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            var Bmp = new Bitmap(eventArgs.Frame, 800, 600);

            try
            {
                using (var MS = new MemoryStream())
                {
                    Bmp.Save(MS, ImageFormat.Jpeg);
                    var BYTES = MS.ToArray();
                    udpClient.Send(BYTES, BYTES.Length, ClientPoint2);
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine(EX);
            }
        }

    }
}
