using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPclient
{
    class Program
    {
        static int port;
        static IPAddress serverip;
        static string[] znach = new string[] {"носочек", "помидорчек", "перчик", "Александрович" };
        static Random rnum = new Random();
        static string fullmessage;
        static TcpClient client = null;
        static NetworkStream stream;
        static void Main(string[] args)
        {
            Console.WriteLine("Введите ip адрес сервера");
            serverip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Введите порт сервера");
            port = int.Parse(Console.ReadLine());
            Console.Clear();
            try
            {
                try
                {
                    ClientObject Client = new ClientObject();
                    Client.Connect(serverip, port);
                    Client.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                     
        }
        public class ClientObject
        {           
            public void Connect(IPAddress serverip, int port)
            {
                client = new TcpClient(Convert.ToString(serverip), port);
                stream = client.GetStream();
            }
            public void Start()
            {
                
                Thread SendThread = new Thread(new ThreadStart(Send));
                SendThread.Start();
                Thread GetThread = new Thread(new ThreadStart(Get));
                GetThread.Start();
            }
            public void Send()
            {
                while (true)
                {                  
                    fullmessage = znach[rnum.Next(0, 4)];
                    byte[] data = Encoding.Unicode.GetBytes(fullmessage);
                    stream.Write(data);
                    Console.WriteLine("Я: " + fullmessage);
                    Thread.Sleep(Sleeptime());
                }
            }
            public void Get()
            {
                StringBuilder builder = new StringBuilder();
                byte[] data = new byte[256];
                int bytes = 0;               
                do
                {
                    builder.Clear();
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    Console.WriteLine("Боты: " + builder.ToString());
                }
                while (true);
            }
            static int Sleeptime()
            {
                double randomvalue = rnum.NextDouble() + rnum.Next(3, 12);
                int _sleeptime = Convert.ToInt32(randomvalue * 1000);
                return (_sleeptime);
            }
        }
    }
}
