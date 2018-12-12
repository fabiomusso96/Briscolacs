using System;
using System.Collections.Generic;
using System.Linq;

namespace Briscola
{
    class Mazzo
    {
        public string[] VttSemi = new string[] { "Denari", "Bastoni", "Coppe", "Spade" };
        public List<Carta> ListMazzo = new List<Carta>();

        public int NCarteRimaste { get; set; }

        private static Random rand = new Random();


        //Riempie il Mazzo per iniziare
        public Mazzo(string nomeMazzo)
        {
            int n = 1;
            for (int i = 0; i < 4; i++)
            {
                n = 1;
                for (int d = 0; d < 10; d++)
                {
                    ListMazzo.Add(new Carta(VttSemi[i], n++, nomeMazzo));
                }
            }

            ListMazzo = ListMazzo.OrderBy(x => Guid.NewGuid()).ToList();
            NCarteRimaste = 40;
        }

        //Da le prime 3 carte agli utenti
        public List<Carta> GetCartaIniziale()
        {
            List<Carta> ritorno = new List<Carta>();
            for (int i = 0; i < 3; i++)
                ritorno.Add(GetCarta());

            return ritorno;
        }

        //Restituisce una carta del mazzo
        public Carta GetCarta()
        {
            int n = rand.Next(0, NCarteRimaste--);

            ListMazzo[n].Usata = true;
            Carta app = ListMazzo[n];
            ListMazzo.RemoveAt(n);

            return app;
        }
    }
}
