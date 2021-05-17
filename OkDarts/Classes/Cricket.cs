using OkDarts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfControlLibraryDarts;

namespace OkDarts.Classes
{
    public class Cricket : MainSpiel, IMainSpiel
    {
        List<CricketSpieler> CricketMitspieler;
        List<CricketZustand> Zustaende = new List<CricketZustand>();
        List<string> ErlaubteWuerfe = new List<string>() { "S 15", "D 15", "T 15", "S 16", "D 16", "T 16", "S 17", "D 17", "T 17", "S 18", "D 18", "T 18", "S 19", "D 19", "T 19", "S 20", "D 20", "T 20", "S B", "D B" };

        public Cricket(List<string> mitspieler, UcWurfAnzeige wurfanzeige, UcTabelle tabelle, UcDartBoard dartBoard) : base(mitspieler, wurfanzeige, tabelle, dartBoard)
        {
            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<CricketZustand>();
            Zustaende.Add(new CricketZustand(CricketMitspieler, 0, 0, "", "", ""));

            SetEvents();
        }

        public void DartBoardBtnBack_Click(object sender, RoutedEventArgs e)
        {
            isEnabledDartBoard = true;
            Wurfanzeige.BtnFertig.Content = "Weiter";
            isVisibleBtnfertigWinner = false;

            CricketZustand zustand = Zustaende[Zustaende.Count - 2];
            Zustaende = new List<CricketZustand>();
            Zustaende.Add(zustand);

            AnzahlWuerfe = zustand.AnzahlWuerfe;
            Wurf1Score = zustand.Wurf1Score;
            Wurf2Score = zustand.Wurf2Score;
            Wurf3Score = zustand.Wurf3Score;
            SpielerDran = zustand.SpielerDran;

            foreach (CricketSpieler spieler in CricketMitspieler)
            {
                CricketSpieler quelle = zustand.MitSpieler.Where(x => x.SpielerName.Equals(spieler.SpielerName)).FirstOrDefault();
                spieler.Score = quelle.Score;
                spieler.Wuerfe15 = quelle.Wuerfe15;
                spieler.Wuerfe16 = quelle.Wuerfe16;
                spieler.Wuerfe17 = quelle.Wuerfe17;
                spieler.Wuerfe18 = quelle.Wuerfe18;
                spieler.Wuerfe19 = quelle.Wuerfe19;
                spieler.Wuerfe20 = quelle.Wuerfe20;
                spieler.WuerfeBull = quelle.WuerfeBull;
            }
            ZeichneGrids();
        }

        public void DartBoardBtnNoScore_Click(object sender, RoutedEventArgs e)
        {
            SetZustand("0");
            ZeichneGrids();
        }

        
        public void DartBoard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Wurf zerlegen
            Label Sender = (Label)sender;
            string wurfwert = Sender.Tag.ToString();
            string[] werte = wurfwert.Split(' ');
            if (ErlaubteWuerfe.Contains(wurfwert)) {
                int i = 0;
                if (werte[1].Equals("B")) {
                    i = 25;
                } else {
                    i = Int32.Parse(werte[1]);
                }

                AddiereWurfZuSpieler(werte[0], werte[1]);

                if (CheckAufSieg(CricketMitspieler[SpielerDran]))
                {
                    Wurfanzeige.BtnFertig.Content = "Neu!";
                    isVisibleBtnfertigWinner = true;
                    isEnabledDartBoard = false;
                    isVisiblBtnNoScore = false;
                }
                
            }
            //Neuen Zustand speichern
            SetZustand(wurfwert);
            ZeichneGrids();
        }

        private void AddiereWurfZuSpieler(string sdt, string wert) {
            int multiplikator = 0;
            if (sdt.Equals("S")) { multiplikator = 1; }
            if (sdt.Equals("D")) { multiplikator = 2; }
            if (sdt.Equals("T")) { multiplikator = 3; }
            int zahl = 0;
            if (wert.Equals("B")) {
                zahl = 25;
            } else { 
                zahl = Int32.Parse(wert);
            }
            //Wurf bewerten und Scores für Spieler ermitteln, Checken, ob das Spiel vorbei ist
            bool zahlOffen = CheckObZahlOffen(zahl);

            switch (zahl) {
                case 15:
                    CricketMitspieler[SpielerDran].Wuerfe15 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe15 > 3) {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe15 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe15 = 3;
                        if(zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 16:
                    CricketMitspieler[SpielerDran].Wuerfe16 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe16 > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe16 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe16 = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 17:
                    CricketMitspieler[SpielerDran].Wuerfe17 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe17 > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe17 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe17 = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 18:
                    CricketMitspieler[SpielerDran].Wuerfe18 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe18 > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe18 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe18 = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 19:
                    CricketMitspieler[SpielerDran].Wuerfe19 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe19 > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe19 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe19 = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 20:
                    CricketMitspieler[SpielerDran].Wuerfe20 += multiplikator;
                    if (CricketMitspieler[SpielerDran].Wuerfe20 > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].Wuerfe20 - 3;
                        CricketMitspieler[SpielerDran].Wuerfe20 = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
                case 25:
                    CricketMitspieler[SpielerDran].WuerfeBull += multiplikator;
                    if (CricketMitspieler[SpielerDran].WuerfeBull > 3)
                    {
                        int multi = CricketMitspieler[SpielerDran].WuerfeBull - 3;
                        CricketMitspieler[SpielerDran].WuerfeBull = 3;
                        if (zahlOffen)
                            CricketMitspieler[SpielerDran].Score += multi * zahl;
                    }
                    break;
            }
        }

        /// <summary>
        /// Prüft, ob eine Zahl bereits von allen Mitspielern zugeworfen wurde
        /// </summary>
        /// <param name="zahl">Der zu checkende Wert</param>
        /// <returns>true wenn die Zahl noch nicht zu ist</returns>
        private bool CheckObZahlOffen(int zahl) {
                switch (zahl) {
                    case 15:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe15 != 3)
                            return true;
                    }
                        break;
                    case 16:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe16 != 3)
                            return true;
                    }
                    break;
                    case 17:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe17 != 3)
                            return true;
                    }
                    break;
                    case 18:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe18 != 3)
                            return true;
                    }
                    break;
                    case 19:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe19 != 3)
                            return true;
                    }
                    break;
                    case 20:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.Wuerfe20 != 3)
                            return true;
                    }
                    break;
                    case 25:
                    foreach (CricketSpieler item in CricketMitspieler.Where(x => x != CricketMitspieler[SpielerDran]))
                    {
                        if (item.WuerfeBull != 3)
                            return true;
                    }
                    break;
                }
            return false;
        }

        private bool CheckAufSieg(CricketSpieler spieler) {
            if (spieler.Wuerfe15 != 3)
                return false;
            if (spieler.Wuerfe16 != 3)
                return false;
            if (spieler.Wuerfe17 != 3)
                return false;
            if (spieler.Wuerfe18 != 3)
                return false;
            if (spieler.Wuerfe19 != 3)
                return false;
            if (spieler.Wuerfe20 != 3)
                return false;
            if (spieler.WuerfeBull != 3)
                return false;
            foreach (CricketSpieler item in CricketMitspieler)
            {
                if (CricketMitspieler.Where(x => x != item).Any()) {
                    int highest = CricketMitspieler.Where(x => x != item).OrderByDescending(x => x.Score).FirstOrDefault().Score;
                    if (CricketMitspieler[SpielerDran].Score < highest)
                    {
                        return false;
                    }
                }
                
            }

            return true;
        }

        public void ErzeugeSpielerRunde(List<string> mitspieler)
        {
            CricketMitspieler = new List<CricketSpieler>();
            foreach (string item in mitspieler)
            {
                CricketSpieler spieler = new CricketSpieler(item);
                CricketMitspieler.Add(spieler);
            }
        }

        public void NeueSpielerListe(List<string> mitspieler)
        {
            Reset();

            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<CricketZustand>();
            Zustaende.Add(new CricketZustand(CricketMitspieler, 0, 0, "", "", ""));
        }

        public void SetEvents()
        {
            Wurfanzeige.BtnFertig.Click += WurfAnzeigeBtnFertig_Click;
            DartBoard.BtnBack.Click += DartBoardBtnBack_Click;
            DartBoard.BtnNoScore.Click += DartBoardBtnNoScore_Click;
            foreach (FrameworkElement ctrl in DartBoard.GrdMain.Children)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    Label lbl = (Label)ctrl;
                    lbl.MouseDoubleClick += DartBoard_MouseDoubleClick;
                }
            }
        }

        public void UnsetEvents()
        {
            Wurfanzeige.BtnFertig.Click -= WurfAnzeigeBtnFertig_Click;
            DartBoard.BtnBack.Click -= DartBoardBtnBack_Click;
            DartBoard.BtnNoScore.Click -= DartBoardBtnNoScore_Click;
            foreach (FrameworkElement ctrl in DartBoard.GrdMain.Children)
            {
                if (ctrl.GetType() == typeof(Label))
                {
                    Label lbl = (Label)ctrl;
                    lbl.MouseDoubleClick -= DartBoard_MouseDoubleClick;
                }
            }
        }

        public void SetZustand(string wurfwert)
        {
            if (!Wurf1Score.Equals("") && !Wurf2Score.Equals("") && Wurf3Score.Equals(""))
            {
                Wurf3Score = wurfwert;
            }
            if (!Wurf1Score.Equals("") && Wurf2Score.Equals("") && Wurf3Score.Equals(""))
            {
                Wurf2Score = wurfwert;
            }
            if (Wurf1Score.Equals("") && Wurf2Score.Equals("") && Wurf3Score.Equals(""))
            {
                Wurf1Score = wurfwert;
            }
            AnzahlWuerfe++;
            if (AnzahlWuerfe == 3) {
                isEnabledDartBoard = false;
            }
            Zustaende.Add(new CricketZustand(CricketMitspieler, AnzahlWuerfe, SpielerDran, Wurf1Score, Wurf2Score, Wurf3Score));
        }

        public void WurfAnzeigeBtnFertig_Click(object sender, RoutedEventArgs e)
        {
            isVisibleBtnfertigWinner = false;
            isEnabledDartBoard = true;
            isVisiblBtnNoScore = true;

            if (Wurfanzeige.BtnFertig.Content.Equals("Weiter"))
            {
                NextSpieler();
            }
            else
            {
                
                foreach (CricketSpieler spieler in CricketMitspieler)
                {
                    spieler.Reset();
                }
                NextRunde();
                Wurfanzeige.BtnFertig.Content = "Weiter";
            }

            Wurf1Score = "";
            Wurf2Score = "";
            Wurf3Score = "";
            AnzahlWuerfe = 0;
            Zustaende = new List<CricketZustand>();
            Zustaende.Add(new CricketZustand(CricketMitspieler, AnzahlWuerfe, SpielerDran, Wurf1Score, Wurf2Score, Wurf3Score));
            ZeichneGrids();
        }

        public void ZeichneGrids()
        {
            DartBoard.Set(isEnabledDartBoard, AnzahlWuerfe, Zustaende.Count > 1, isVisiblBtnNoScore);
            Wurfanzeige.Set(CricketMitspieler[SpielerDran].SpielerName, Wurf1Score, Wurf2Score, Wurf3Score, CricketMitspieler[SpielerDran].Score.ToString(), "cricket", AnzahlWuerfe == 3 || isVisibleBtnfertigWinner);
            ZeichneGridTabelle();
        }

        private BitmapImage GetPic(string runde, string picname) {
            string path = "pack://application:,,,/Images/cricket" + runde + "_" + picname + ".png";
            return new BitmapImage(new Uri(path));
        }

        public void ZeichneGridTabelle()
        {
            Tabelle.GrdMain.Children.Clear();
            int counter = 0;
            foreach (CricketSpieler spieler in CricketMitspieler)
            {
                //Spielername
                SetGrid(Tabelle.GrdMain, spieler.SpielerName, 0, counter, 2, 1);
                //15
                SetGrid(Tabelle.GrdMain, GetPic("15", spieler.Wuerfe15.ToString()), 2, counter, 1, 1);
                //16
                SetGrid(Tabelle.GrdMain, GetPic("16", spieler.Wuerfe16.ToString()), 3, counter, 1, 1);
                //17
                SetGrid(Tabelle.GrdMain, GetPic("17", spieler.Wuerfe17.ToString()), 4, counter, 1, 1);
                //18
                SetGrid(Tabelle.GrdMain, GetPic("18", spieler.Wuerfe18.ToString()), 0, counter + 1, 1, 1);
                //19
                SetGrid(Tabelle.GrdMain, GetPic("19", spieler.Wuerfe19.ToString()), 1, counter + 1, 1, 1);
                //20
                SetGrid(Tabelle.GrdMain, GetPic("20", spieler.Wuerfe20.ToString()), 2, counter + 1, 1, 1);
                //Bull
                SetGrid(Tabelle.GrdMain, GetPic("Bull", spieler.WuerfeBull.ToString()), 3, counter + 1, 1, 1);
                //Score
                SetGrid(Tabelle.GrdMain, spieler.Score.ToString(), 4, counter + 1, 1, 1);

                counter += 2;
            }
            Tabelle.SetFonts(30);
        }
    }
}
