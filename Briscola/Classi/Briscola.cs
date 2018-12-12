using System;
using System.Windows.Media.Imaging;

namespace Briscola
{
    class BriscolaCS
    {
        public Mazzo Mazzo1 { get; }
        public Utente Ut1 { get; }
        public Utente CPU { get; }
        public Carta C1 { get; set; }
        public Carta C2 { get; set; }
        public Carta CardBriscola { get; set; }
        public BitmapImage[] VttCarteCPU { get; }
        public BitmapImage PercorsoVuoto { get; set; }
        public BitmapImage PercorsoMazzo { get; set; }
        private bool GiocaGiocatore { get; set; }
        private int NUltimoTurno { get; set; }

        public BriscolaCS(string nomeMazzo)
        {
            Mazzo1 = new Mazzo(nomeMazzo);
            Ut1 = new Utente(Mazzo1.GetCartaIniziale());
            CPU = new Utente(Mazzo1.GetCartaIniziale());

            C1 = new Carta();
            C2 = new Carta();

            GetBriscola();

            PercorsoVuoto = new BitmapImage(new Uri("/" + nomeMazzo + "/retro.png", UriKind.Relative));
            PercorsoMazzo = PercorsoVuoto;

            VttCarteCPU = new BitmapImage[3];
            VttCarteCPU[0] = PercorsoVuoto;
            VttCarteCPU[1] = PercorsoVuoto;
            VttCarteCPU[2] = PercorsoVuoto;

            GiocaGiocatore = true;
            return;
        }

        public void GetBriscola()
        {
            CardBriscola = Mazzo1.GetCarta();
            return;
        }
        public void SetCentro1(int nCarta)
        {
            BitmapImage perc = Ut1.MieCarte[nCarta].percorso;
            C1 = Ut1.MieCarte[nCarta];

            Ut1.MieCarte.RemoveAt(nCarta);
            Ut1.addCarta(new Carta());
            C1.percorso = perc;
        }

        public Carta GetCentro2()
        {
            Carta ret = new Carta();
            Random rnd = new Random();
            while (ret.Seme == "")
            {
                int n = rnd.Next(0, 2);
                ret = CPU.MieCarte[n];
                CPU.MieCarte.RemoveAt(n);
                CPU.addCarta(new Carta());
            }

            if (NUltimoTurno == 1)
                VttCarteCPU[2] = null;
            if (NUltimoTurno == 2)
                VttCarteCPU[1] = null;
            if (NUltimoTurno == 3)
                VttCarteCPU[0] = null;

            return ret;
        }

        public void Continua()
        {

            if (GiocaGiocatore && C1.Seme == "")
                return;
            if (GiocaGiocatore && C1.Seme != "")
            {
                C2 = GetCentro2();
                return;
            }
            if (CPU.MieCarte[0].Seme == "" && CPU.MieCarte[1].Seme == "" && CPU.MieCarte[2].Seme == "")
                return;
            if (C2.Seme == "")
            {
                C2 = GetCentro2();
                return;
            }
            if (C2.Seme != "" && C1.Seme != "")
                return;

            return;
        }

        private bool Confronto()
        {
            if (C1.Seme == CardBriscola.Seme && C2.Seme != CardBriscola.Seme) 
                return true;

            if (C1.Seme != CardBriscola.Seme && C2.Seme == CardBriscola.Seme) 
                return false;

            if (C1.Seme == CardBriscola.Seme && C2.Seme == CardBriscola.Seme) 
                return CartaVSCarta();

            if (C1.Seme != CardBriscola.Seme && C2.Seme != CardBriscola.Seme) 
                return CartaVSCarta();
            return false;
        }
        public int DopoConfronto()
        {
            bool iovinco = Confronto();
            int PuntiTurno = C1.Valore + C2.Valore;
            int ret = 0;

            C1 = new Carta();
            C2 = new Carta();

            if (Mazzo1.NCarteRimaste > 1)
            {
                Ut1.MieCarte[2] = Mazzo1.GetCarta();
                CPU.MieCarte[2] = Mazzo1.GetCarta();
            }
            else
                NUltimoTurno++;

            if (NUltimoTurno == 1)
            {
                if (iovinco)
                {
                    Ut1.MieCarte[2] = new Carta(CardBriscola.Seme, CardBriscola.Numero, CardBriscola.nmMazzo);
                    CPU.MieCarte[2] = Mazzo1.GetCarta();
                }
                else
                {
                    CPU.MieCarte[2] = new Carta(CardBriscola.Seme, CardBriscola.Numero, CardBriscola.nmMazzo);
                    Ut1.MieCarte[2] = Mazzo1.GetCarta();
                }
                CardBriscola.percorso = null;
                PercorsoMazzo = null;
            }

            if (iovinco)
            {
                GiocaGiocatore = true;
                Ut1.Punteggio += PuntiTurno;
                ret = 1;
            }
            else
            {
                GiocaGiocatore = false;
                CPU.Punteggio += PuntiTurno;
                ret = 2;
            }

            if (NUltimoTurno == 4)
                ret += 2;

            return ret;
        }

        private bool CartaVSCarta()
        {
            string semeTemp = "";

            if (GiocaGiocatore)
                semeTemp = C1.Seme;
            else
                semeTemp = C2.Seme;

            if (GiocaGiocatore)
            {
                if (C2.Seme != semeTemp)
                    if(C2.Seme != CardBriscola.Seme)
                        return true;

                if (C1.Valore == C2.Valore)
                    return true;

                if (C1.Valore > C2.Valore)
                    return true;
            }

            if (!GiocaGiocatore)
            {
                if (C2.Seme != semeTemp)
                    if (C2.Seme != CardBriscola.Seme)
                        return false;
                if (C1.Valore == C2.Valore)
                    return false;
                if (C1.Valore > C2.Valore)
                    return false;
            }
            return false;
        }
    }
}
