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
namespace hasta_sıra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient yeni = null;
        public void gonder()
        {
            try
            {
                TcpListener dinle = new TcpListener(IPAddress.Parse("127.0.0.1"), 5555);
                dinle.Start(); /*dinlemeyi başlat*/
                while (true)
                {
                    Thread.Sleep(1000); /*CPU yorulmasın diye 1 sn uyuttum*/

                    if (gonderim == true) /*burayı sürekli istek atıp ağı doldurmamak adına yapıyorum eğer ayrı değerler ise istek atsın doktor her butona tıkladığında true olacak zira sonra false yapacaz */
                    {


                        yeni = dinle.AcceptTcpClient(); /*gelen bağlantıyı kabul et*/
                        byte[] cevap = Encoding.UTF8.GetBytes(sira.ToString()+" Sıranız gelmiştir."); /*sira nosunu önce string ardından byte dönüştürüyorum*/
                        NetworkStream oku_yaz = yeni.GetStream(); /*dinle*/
                        oku_yaz.Write(cevap, 0, cevap.Length); /*gönder*/
                        yeni.Close();
                        gonderim = false;
                    }

                }

            }
            catch (Exception hata)
            {
            }
            
            

        }



        Thread baslat;
        private void Form1_Load(object sender, EventArgs e)
        {
            baslat = new Thread(gonder); /*yeni bir iplik oluştur*/
            baslat.Start(); /*başlat*/


        }
        int sira = 0;
        Boolean gonderim = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if(gonderim==false)
            {
                gonderim = true;
            }
            else
            {
                gonderim = false;
            }
            sira++;
            label2.Text = Convert.ToString(sira);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gonderim == false)
            {
                gonderim = true;
            }
            else
            {
                gonderim = false;
            }
            sira--;
            label2.Text = Convert.ToString(sira);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            baslat.Abort(); /*işlemi sonlandır*/
        }
    }
}
