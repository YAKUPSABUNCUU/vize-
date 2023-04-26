using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace tiger_windows_server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient yeni = null;

        public void yaz_log(string j)
        {
            if (label2.InvokeRequired)
            {
                isim del = new isim(yaz_log);
                this.listBox1.Invoke(del, new object[] { j });

            }
            else
            {



                listBox1.Items.Add(j);



            }
        }

        public delegate void isim(string s);

        Thread islem;
        public void gonder()
        {
            try
            {
                TcpListener dinle = new TcpListener(IPAddress.Parse("127.0.0.1"), 2222);
                dinle.Start(); /*dinlemeyi başlat*/
                while (true)
                {
                    Thread.Sleep(1000); /*CPU yorulmasın diye 1 sn uyuttum*/



                    yeni = dinle.AcceptTcpClient(); /*gelen bağlantıyı kabul et*/
                    if (yeni.Connected) /*eğer bağlantı varsa*/
                    {
                        yaz_log("Yeni bir bağlantı sağlandı! ip adresi: " + yeni.Client.RemoteEndPoint);
                        byte[] cevap = new byte[1200]; /*gelen cevapları almak için bir byte dizisi*/
                        NetworkStream oku_yaz = yeni.GetStream(); /*dinle*/
                         
                        oku_yaz.Read(cevap, 0, cevap.Length); /*oku*/
                        
                        oku_yaz.Close();
                        
                        yeni.Close();
                        String cevir = Encoding.UTF8.GetString(cevap); /*Gelen byte veriyi String veri tipine dönüştür*/
                        if (cevir.IndexOf("123456789 ") >= 0) /*eğer gelen verinin içinde şifre geçiyorsa*/
                        {
                            if (cevir.IndexOf("mesaj_ver ") >= 0 && cevir.IndexOf(" uyarı") >= 0) /*eğer gelen veride mesaj ve uyari varsa*/
                            {
                                String temizle = cevir.Replace(textBox1.Text+" ", ""); /*şifre geçen yeri sil*/
                                String temizle2 = temizle.Replace("mesaj_ver ", "");
                                String temizle3 = temizle2.Replace("uyarı", "");
                                MessageBox.Show(temizle3, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                
                            }


                            else if (cevir.IndexOf("mesaj_ver") >= 0 && cevir.IndexOf("hata") >= 0) /*eğer gelen veride mesaj ve hata varsa*/
                            {
                                String temizle = cevir.Replace(textBox1.Text + " ", ""); /*şifre geçen yeri sil*/
                                String temizle2 = temizle.Replace("mesaj_ver ", "");
                                String temizle3 = temizle2.Replace("hata", "");
                                MessageBox.Show(temizle3, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }

                            else if (cevir.IndexOf("mesaj_ver") >= 0 && cevir.IndexOf("bilgilendir") >= 0) /*eğer gelen veride mesaj ve bilgilendir varsa*/
                            {
                                String temizle = cevir.Replace(textBox1.Text + " ", ""); /*şifre geçen yeri sil*/
                                String temizle2 = temizle.Replace("mesaj_ver ", "");
                                String temizle3 = temizle2.Replace("bilgilendir", "");
                                MessageBox.Show(temizle3, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }



                            else if (cevir.IndexOf("mesaj_ver") >= 0 && cevir.IndexOf("kritik") >= 0) /*eğer gelen veride mesaj ve kritik varsa*/
                            {
                                String temizle = cevir.Replace(textBox1.Text + " ", ""); /*şifre geçen yeri sil*/
                                String temizle2 = temizle.Replace("mesaj_ver ", "");
                                String temizle3 = temizle2.Replace("kritik", "");
                                MessageBox.Show(temizle3, "", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                            }



                            else if (cevir.IndexOf("islem ") >= 0 && cevir.IndexOf("gizli") >= 0) /*eğer gelen verideislem ve normal*/
                            {
                                String temizle = cevir.Replace(textBox1.Text + " ", ""); /*şifre geçen yeri sil*/
                                String temizle2 = temizle.Replace("islem ", "");
                                String temizle3 = temizle2.Replace("gizli ", "");
                                Process.Start(temizle3);

                            }
                            else
                            {
                                MessageBox.Show("");
                            }







                        }
                        


                       

                    }





                }

            }
            catch (Exception hata)
            {
                /*Hata kısmı */
            }
        }





        public String default_password = "123456789"; /*sunucunun default şifresi*/
        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "HoşGeldiniz "+Environment.UserName; /*karşılama mesajı */
            textBox1.PasswordChar = '*'; /*TextBoxa şifre özelliği kazandırır, yazılan karakterlerin hepsi ** olarak gözükür*/
            textBox1.Text = default_password; /*varsayılan şifre yap*/
        }

        Boolean durum = true;
        private void button3_Click(object sender, EventArgs e)
        {
            /*Şifre göster gizle kısmı */
            if(durum)
            {
                textBox1.PasswordChar = '\0'; /*görünümü normal yap*/
            }
            else
            {
                textBox1.PasswordChar = '*'; /*TextBoxa şifre özelliği kazandırır, yazılan karakterlerin hepsi ** olarak gözükür*/
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                islem = new Thread(gonder); /*yeni bir iplik oluştur*/
                islem.Start(); /*işlemi başlat*/
                MessageBox.Show("İşlem başladı");
            }
            catch(Exception hata)
            {
                MessageBox.Show(hata.Message,"hata");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                islem.Abort(); /*ipliği sonlandır*/
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message, "hata");
            }
        }
    }
}
