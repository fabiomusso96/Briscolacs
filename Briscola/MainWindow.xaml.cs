using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string nm, string idxMaz)
        {
            InitializeComponent();
            NomeGiocatore = nm;
            strMazzo = idxMaz;
        }

        private BriscolaCS Brscl;
        private string NomeGiocatore;
        private string strMazzo;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNomeGiocatore.Text = txtNomeGiocatore.Text + NomeGiocatore;
            IniziaPartita();
        }

        private void IniziaPartita()
        {
            Brscl = new BriscolaCS(strMazzo);
            AggiornaImmagini();
        }

        public void AggiornaImmagini(int n = 0)
        {
            //Carte nel mazzo
            lblNcarte.Text = Brscl.Mazzo1.NCarteRimaste.ToString();

            //Le 3 carte del giocatore
            btnCarta1.Source = Brscl.Ut1.MieCarte[0].percorso;
            btnCarta2.Source = Brscl.Ut1.MieCarte[1].percorso;
            btnCarta3.Source = Brscl.Ut1.MieCarte[2].percorso;

            //2 Carte centrali
            btnCentro1.Source = Brscl.C1.percorso;
            btnCentro2.Source = Brscl.C2.percorso;

            //3 Carte CPU
            btnCPU1.Source = Brscl.VttCarteCPU[0];
            btnCPU2.Source = Brscl.VttCarteCPU[1];
            btnCPU3.Source = Brscl.VttCarteCPU[2];

            //Briscola e Mazzo
            btnBriscola.Source = Brscl.CardBriscola.percorso;
            btnMazzo.Source = Brscl.PercorsoMazzo;

            //Punteggi
            txtPuntiGiocatore.Text = Brscl.Ut1.Punteggio.ToString();
            txtPuntiCPU.Text = Brscl.CPU.Punteggio.ToString();

            return;
        }

        private void btnCarta1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelCarta(0);
        }

        private void btnCarta2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelCarta(1);
        }

        private void btnCarta3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelCarta(2);
        }

        private async void SelCarta(int nCarta, int nAzioni = 0)
        {
            //disabilita per sicurezza le carte
            btnCarta1.IsEnabled = false;
            btnCarta2.IsEnabled = false;
            btnCarta3.IsEnabled = false;

            Brscl.SetCentro1(nCarta);
            AggiornaImmagini();

            Brscl.Continua();

            //per dare una sensazione di realta aspetta 1000 millisecondi
            if (nAzioni == 0)
                await Task.Delay(1000);

            AggiornaImmagini();

            //decide il vincitore
            int qw = Brscl.DopoConfronto();

            //Stampa il nome del vincitore
            if (qw == 1)
                lblVinto.Content = "Prendi tu, " + NomeGiocatore + "...";
            if (qw == 2)
                lblVinto.Content = "Prendo io!";
            if (qw == 3)
                MessageBox.Show("Hai preso l'ultima carta, " + NomeGiocatore + "... ma vediamo se hai vinto!");
            if (qw == 4)
                MessageBox.Show("Ho preso l'ultima carta! Vediamo se ho vinto...");

            //aspetta
            if (nAzioni == 0)
                await Task.Delay(1000);

            AggiornaImmagini();
            Brscl.Continua();

            //aspetta
            if (nAzioni == 0)
                await Task.Delay(1000);

            AggiornaImmagini();

            ////////////////////////////////

            if (qw > 2)
            {
                int puntiUt = Brscl.Ut1.Punteggio;
                int puntiCPU = Brscl.Ut1.Punteggio;
                int punti = Brscl.Ut1.Punteggio + Brscl.CPU.Punteggio;
                string fineUt = "Totale punti: " + puntiUt.ToString();
                string fineCPU = "Totale punti: " + puntiCPU.ToString();

                if (Brscl.Ut1.Punteggio > Brscl.CPU.Punteggio)
                    MessageBox.Show("Hai vinto tu, " + NomeGiocatore + "!" + "\n" + fineUt);
                else
                    MessageBox.Show("Ho vinto io! \n" + fineCPU);

                //Chiude la finestra e termina il programma
                if(MessageBox.Show("Vuoi fare un'altra partita? :)", "Ehi!", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    Chiusura();
                }
                else
                {
                    Window Finestra = new Briscola.MainWindow(NomeGiocatore, strMazzo);
                    Hide();
                    Finestra.ShowDialog();
                    Close();
                }

            }

            grd1 = new System.Windows.Controls.Grid();

            //Riabilita le carte
            btnCarta1.IsEnabled = true;
            btnCarta2.IsEnabled = true;
            btnCarta3.IsEnabled = true;
        }

        private void Chiusura()
        {
            return;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
                SelCarta(rnd.Next(0, 2), 1);
        }
    }
}