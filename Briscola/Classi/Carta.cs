using System;
using System.Windows.Media.Imaging;

namespace Briscola
{
    class Carta
    {
        public string Seme { get; set; }
        public int Numero { get; set; }
        public bool Usata { get; set; }
        public int Valore { get; set; }
        public BitmapImage percorso { get; set; }
        public string nmMazzo { get; set; }


        public Carta()
        {
            Seme = "";
            Numero = 0;
            Valore = 0;
            percorso = null;
        }
        public Carta(string sm, int vl, string nomeMazzo)
        {
            nmMazzo = nomeMazzo;
            Seme = sm;
            Numero = vl;

            if (vl == 1)
                Valore = 11;
            else if (vl == 3)
                    Valore = 10;
            else if (vl == 10)
                    Valore = 4;
            else if (vl == 9)
                    Valore = 3;
            else if (vl == 8)
                    Valore = 2;
            else
                    Valore = 0;

            string imgCard = "/" + nomeMazzo + "/" + Seme + " (" + Numero.ToString() + ").png";
            percorso = new BitmapImage(new Uri(imgCard, UriKind.Relative));
        }
    }
}
