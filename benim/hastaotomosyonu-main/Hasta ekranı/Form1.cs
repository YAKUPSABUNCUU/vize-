using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
namespace Hasta_ekranı
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public void yaz(string j)
        {
            if (label2.InvokeRequired)
            {
                isim del = new isim(yaz);
                this.label2.Invoke(del, new object[] { j });

            }
            else
            {



                label2.Text = j;

               
                
            }
            }
        
    public delegate void isim(string s);
    void baglan()
        {
            while(true)
            {
                try
                {
                    Thread.Sleep(3000);
                    TcpClient baglan = new TcpClient();
                    baglan.Connect("127.0.0.1", 5555);
                    NetworkStream oku = baglan.GetStream(); /*ağı dinle*/
                    byte[] cevap = new byte[2000];
                    oku.Read(cevap, 0, cevap.Length); /*gelen değeri oku*/
                    
                    yaz(Encoding.UTF8.GetString(cevap));
                    oku.Close();
                }
                catch(Exception hata)
                {
                   
                }
            }

        }
        Thread islem;
        private void Form1_Load(object sender, EventArgs e)
        {
             islem = new Thread(baglan);
            islem.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            islem.Abort(); /*işlemi sonlandır*/
        }
    }
}
