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
        static void Main(string[] args)
        {
            try
            {              
                Console.WriteLine("Введите ip адрес сервера");
                serverip = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("Введите порт сервера");
                port = int.Parse(Console.ReadLine());               
                Console.Clear();               
                client = new TcpClient(Convert.ToString(serverip), port);
                while (true)
                {                   
                    NetworkStream stream = client.GetStream();
                    fullmessage = znach[rnum.Next(0, 4)];
                    byte[] data = Encoding.Unicode.GetBytes(fullmessage);
                    stream.Write(data);
                    Console.WriteLine("Я: " + fullmessage);
                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    Console.WriteLine("Боты: " + builder.ToString());
                    Thread.Sleep(Sleeptime());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
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
