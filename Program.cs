using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MukorcsolyaGYAK
{
    class Verseny
    {
        public string nev { get; set; }
        public string orszag { get; set; }
        public double technikai { get; set; }
        public double komponens { get; set; }
        public int levonas { get; set; }
        public double osszPont { get; set; }
        public Verseny(string nev, string orszag, double technikai, double komponens, int levonas)
        {
            this.nev = nev;
            this.orszag = orszag;
            this.technikai = technikai;
            this.komponens = komponens;
            this.levonas = levonas;
        }
    }


    class Program
    {
        static List<Verseny> rovid = new List<Verseny>();
        static List<Verseny> donto = new List<Verseny>();
        public static double ÖsszPontszám(string nev)
        {
            var osszP=0.0;
            foreach (var r in rovid)
            {
                if (r.nev==nev)
                {
                    osszP += r.komponens+r.technikai-r.levonas;
                }
            }
            foreach (var d in donto)
            {
                if (d.nev==nev)
                {
                    osszP += d.komponens + d.technikai - d.levonas;
                }
            }
            return osszP;
        }


        static void Main(string[] args)
        {
            #region 1. feladat
                try
                {
                    FileStream fs = new FileStream("rovidprogram.csv",FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] split = sr.ReadLine().Split(';');
                        rovid.Add(new Verseny(
                            split[0],
                            split[1],
                            Convert.ToDouble(split[2].Replace('.', ',')),
                            Convert.ToDouble(split[3].Replace('.', ',')),
                            Convert.ToInt32(split[4])
                            ));
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hiba a beolvasásnál: rövidprogram "+e.Message);
                }
                try
                {
                    FileStream fs = new FileStream("donto.csv", FileMode.Open);
                    StreamReader sr = new StreamReader(fs,Encoding.UTF8);
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] split = sr.ReadLine().Split(';');
                        donto.Add(new Verseny(
                            split[0],
                            split[1],
                            Convert.ToDouble(split[2].Replace('.', ',')),
                            Convert.ToDouble(split[3].Replace('.', ',')),
                            Convert.ToInt32(split[4])
                            ));
                    }
                    sr.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hiba a beolvasásnál:döntő " + e.Message);
                }
            #endregion

            #region 2. feladat
              Console.WriteLine("2. feladat: "+rovid.Count);
            #endregion

            #region 3. feladat
                var valasz = "A magyar versenyző nem jutott be a kűrbe. ";
                foreach (var item in donto)
                {
                    if (item.orszag.ToUpper().Equals("HUN"))
                    {
                        valasz = "A magyar versenyző bejutott a kűrbe. ";
                    }
                }
                Console.WriteLine("3. feladat: "+valasz);
            #endregion

            #region 4. feladat
                Console.WriteLine("5. feladat: ");
                Console.Write("\tKérem a versenyző nevét: ");
                var beker = Console.ReadLine();
                valasz = "Ilyen nevű induló nem volt! ";
                foreach (var item in rovid)
                {
                    if (item.nev.Equals(beker))
                    {
                        valasz = null;
                    }
                }
                if (!string.IsNullOrEmpty(valasz))
                {
                    Console.WriteLine("\t"+valasz);
                }
                else
                {
                    Console.WriteLine("6. feladat: ");
                    Console.WriteLine("\tA versenyző összpontszána: {0}",ÖsszPontszám(beker));
                }
            #endregion

            #region 7. feladat
                Console.WriteLine("7. feladat: ");
                var stat = donto.GroupBy(x => x.orszag)
                    .Select(g => new
                    {
                        orszag = g.Key,
                        db = g.Count()
                    })
                    .OrderByDescending(x=>x.db);

                foreach (var item in stat)
                {
                  var i=item.db>1?"\t"+item.orszag+": "+item.db+" versenyző":"";
                    if (!string.IsNullOrEmpty(i))
                    {
                        Console.WriteLine(i);
                    }
                }
            #endregion

            #region 8. feladat
            FileStream fw = new FileStream("vegeredmeny.csv",FileMode.Create);
                using (StreamWriter sw = new StreamWriter(fw))
                {
                    foreach (var item in donto)
                    {
                        item.osszPont = ÖsszPontszám(item.nev);
                    }
                    var veger = donto.OrderByDescending(x=>x.osszPont);
                    var count = 1;
                    foreach (var item in veger)
                    {
                        sw.WriteLine($"{count};{item.nev};{item.orszag};{item.osszPont}");
                        count++;
                    }
                }
            #endregion
            Console.ReadKey();
        }
    }
}
