using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;


namespace Dolap___AIO
{
    class Program
    {

        private static readonly string Token = "CAPMONSTER KEY";


        public static string response = "";
        static void hesapacmabotu()
        {
            HttpRequest req = new HttpRequest();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Kaç Hesap Açılsın: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            var hesapsayisi = Console.ReadLine();
            for (int i = 0; i < Convert.ToInt32(hesapsayisi) + 1; i++)
            {


                try
                {
                    var degers = req.Post("https://api.capmonster.cloud/createTask", "{\"clientKey\":\""+Token+"\",\"task\":{\"type\":\"NoCaptchaTaskProxyless\",\"websiteURL\":\"https://dolap.com/giris\",\"websiteKey\":\"6LdUCqQUAAAAADFMYHEjWJjZWDCcov0F17XJ1jww\"}}", "application/json;charset=UTF-8").ToString();
                    var id = degers.Substring("taskId\"", "}").Replace(":", "");
                    while (true)
                    {
                        var response1 = req.Post("https://api.capmonster.cloud/getTaskResult ", "{\"clientKey\":\""+Token+"\",\"taskId\":" + id + "}", "application/json;charset=UTF-8").ToString();
                        if (response1.Contains("ready"))
                        {
                            response = response1.Substring("gRecaptchaResponse\":\"", "\"},");
                            break;
                        }
                        Thread.Sleep(800);
                    }
                    string ad = "";
                    string sifre = "";
                    string eposta = "";
                    var profilbilgileri = req.Get("http://rp.burakgarci.net/api.php").ToString();
                    ad = profilbilgileri.Substring("isim", ",").Replace("\"", "").Replace(":", "").Replace(" ", "");
                    eposta = profilbilgileri.Substring("eposta", ",").Replace("\"", "").Replace(":", "").Replace(" ", "").Replace("example.com", "outlook.com");
                    sifre = profilbilgileri.Substring("sifre", ",").Replace("\"", "").Replace(":", "").Replace(" ", "");
                    Random rndm = new Random();
                    var randomvalue = rndm.Next(0, 1000);
                    var nick = ad + randomvalue;
                    var value = req.Post("https://dolap.com/kayit?captchaToken=" + response, "{\"email\":\"" + eposta + "\",\"nickName\":\"" + nick + "\",\"password\":\"" + sifre + "\",\"membershipAgreement\":true,\"campaignAgreement\":false}", "application/json;charset=UTF-8").ToString();;
                    if (value.Contains("accessToken"))
                    {
                        var token = value.Substring("accessToken", ",").Replace("\"", "").Replace(":", "");
                        File.AppendAllText(Environment.CurrentDirectory + $@"\Hesap Bilgileri\Hesaplar.txt", String.Format("{0}{1}", nick + ":" + eposta + ":" + sifre, $"{Environment.NewLine}"));
                        File.AppendAllText(Environment.CurrentDirectory + $@"\Hesap Tokenleri\Tokens.txt", String.Format("{0}{1}", token, $"{Environment.NewLine}"));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Başarıyla Oluşuturldu.");
                    }
                    else
                    {
                        i++;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Captcha Error.");
                    }

                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hata Hesap Oluşturulamadı.");
                    i++;
                }
            }
        }
        static void sohbetbotu()
        {
           
            HttpRequest req = new HttpRequest();
            Console.WriteLine("id?");
            var id = Console.ReadLine();
            while (true)
            {
                using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + @"\Hesap Tokenleri\Tokens.txt"))
                {
                string line;
                    while ((line = sr.ReadLine()) != null)
                     {
                        req.Proxy = HttpProxyClient.Parse("PROXY");
                        //Burada Cookie headeri ekleyiniz. eğer çalışmazsa
                        string mesaj = "MESAJINIZ İSTERSENİZ DEĞİŞKEN ALABİLİRSİNİZ.";
                    req.Post("https://dolap.com/product/comment", "{\"commentBody\":\""+mesaj+"\",\"parentId\":null,\"productId\":\"" + id + "\"}", "application/json;charset=UTF-8");
                    }
                }
            }

            
        }
        static void Main(string[] args)
        {
            //FONSKİYONU ÇALIŞTIRINIZ BURADA SOHBET BOTUNU BAŞLATICAKTIR HESAP OLUŞTURUCU İÇİN "hesapacmabotu();" KULLANIN İSTERSENİZ DEĞİŞKEN ALABİLİRSİNİZ BEN PEK UĞRAŞMADIM.
            sohbetbotu();

            Console.Read();
        }

       
    }
}
