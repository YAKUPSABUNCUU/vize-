using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace client_testc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient yeni = new TcpClient();
            yeni.Connect("192.168.1.3",2580);
            Console.ReadKey();
        }
    }
}
