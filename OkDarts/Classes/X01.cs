using OkDarts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfControlLibraryDarts;

namespace OkDarts.Classes
{
    public class X01 : MainSpiel, IMainSpiel
    {
        private List<X01Spieler> X01Mitspieler;
        public int StartScore = 0;
        private int ScoreRunde = 0;
        private List<X01Zustand> Zustaende = new List<X01Zustand>();
        

        public X01(List<string> mitspieler, UcWurfAnzeige wurfanzeige, UcTabelle tabelle, UcDartBoard dartBoard, int startScore) : base(mitspieler, wurfanzeige, tabelle, dartBoard)
        {
            StartScore = startScore;
            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<X01Zustand>();
            Zustaende.Add(new X01Zustand() { AnzahlWuerfe = AnzahlWuerfe, ScoreRunde = ScoreRunde, SpielerDran = SpielerDran, Wurf1Score = Wurf1Score, Wurf2Score = Wurf2Score, Wurf3Score = Wurf3Score });

            SetEvents();
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

        public void DartBoard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label Sender = (Label)sender;
            string wurfwert = Sender.Tag.ToString();
            if (CheckWurfSetWerte(wurfwert))
            {
                SetZustand(wurfwert);
                ZeichneGrids();
            }
        }

        private int BerechneWurf(string wurf) {
            string[] Wurf = wurf.Split(' ');
            string mult = Wurf[0];
            int multiplikator = 1;
            if (mult.Equals("D")) { multiplikator = 2; }
            if (mult.Equals("T")) { multiplikator = 3; }
            string val = Wurf[1];
            int value = 0;
            if (val.Equals("B")) {
                value = 25;
            } else {
                value = int.Parse(val);
            }
            return multiplikator * value;
        }

        public bool CheckWurfSetWerte(string wurfwert) {
            int wurfValue = BerechneWurf(wurfwert);
            //Fall 1 --> Spieler hat gewonnen
            if (X01Mitspieler[SpielerDran].Score == wurfValue + ScoreRunde) {
                ScoreRunde += wurfValue;
                Wurfanzeige.BtnFertig.Content = "Neue Runde";
                //isVisibleBtnfertigWinner = true;
                //isEnabledDartBoard = false;
                //isVisiblBtnNoScore = false;
                SetButtons(false, true, false);
                SetZustand(wurfwert);
                ZeichneGrids();
                return false;
            }
            //Fall 2 --> Spieler hat sich überworfen
            if (X01Mitspieler[SpielerDran].Score < wurfValue + ScoreRunde)
            {
                ScoreRunde = 0;
                //isVisibleBtnfertigWinner = true;
                //isEnabledDartBoard = false;
                //isVisiblBtnNoScore = false;
                SetButtons(false, true, false);
                SetZustand(wurfwert);
                ZeichneGrids();
                return false;
            }
            //Fall 3 --> Wurf wird vom Score des Spielers abgezogen
            ScoreRunde += wurfValue;
            if (AnzahlWuerfe == 3) {
                DartBoard.Set(false, AnzahlWuerfe, true, false);
                Wurfanzeige.BtnFertig.Visibility = Visibility.Visible;
            }
            return true;
        }

        public void SetZustand(string wurfwert) {

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
                SetButtons(false, isVisibleBtnfertigWinner, false);
            }
            Zustaende.Add(new X01Zustand { AnzahlWuerfe = AnzahlWuerfe, ScoreRunde = ScoreRunde, Wurf1Score = Wurf1Score, Wurf2Score = Wurf2Score, Wurf3Score = Wurf3Score, SpielerDran = SpielerDran });
        }

        public void WurfAnzeigeBtnFertig_Click(object sender, RoutedEventArgs e)
        {
            //isVisibleBtnfertigWinner = false;
            //isEnabledDartBoard = true;
            //isVisiblBtnNoScore = true;
            SetButtons(true, false, true);

            X01Mitspieler[SpielerDran].Score -= ScoreRunde;

            if (Wurfanzeige.BtnFertig.Content.Equals("Weiter"))
            {
                NextSpieler();
            }
            else {
                X01Mitspieler[SpielerDran].Siege++;
                foreach (X01Spieler spieler in X01Mitspieler)
                {
                    spieler.Score = StartScore;
                }
                NextRunde();
                Wurfanzeige.BtnFertig.Content = "Weiter";
            }
            
            ScoreRunde = 0;
            Wurf1Score = "";
            Wurf2Score = "";
            Wurf3Score = "";
            AnzahlWuerfe = 0;
            Zustaende = new List<X01Zustand>();
            Zustaende.Add(new X01Zustand() { AnzahlWuerfe = AnzahlWuerfe, ScoreRunde = ScoreRunde, SpielerDran = SpielerDran, Wurf1Score = Wurf1Score, Wurf2Score = Wurf2Score, Wurf3Score = Wurf3Score });
            ZeichneGrids();
        }

        public void DartBoardBtnBack_Click(object sender, RoutedEventArgs e)
        {
            //isEnabledDartBoard = true;
            Wurfanzeige.BtnFertig.Content = "Weiter";
            //isVisibleBtnfertigWinner = false;
            SetButtons(true, false, true);

            X01Zustand zustand = Zustaende[Zustaende.Count - 2];
            
            Zustaende = new List<X01Zustand>();
            Zustaende.Add(zustand);
            AnzahlWuerfe = zustand.AnzahlWuerfe;
            Wurf1Score = zustand.Wurf1Score;
            Wurf2Score = zustand.Wurf2Score;
            Wurf3Score = zustand.Wurf3Score;
            SpielerDran = zustand.SpielerDran;
            ScoreRunde = zustand.ScoreRunde;
            ZeichneGrids();
        }

        public void DartBoardBtnNoScore_Click(object sender, RoutedEventArgs e)
        {
            string wurfwert = " 0";
            if (CheckWurfSetWerte(wurfwert))
            {
                SetZustand(wurfwert);
                ZeichneGrids();
            }
        }

        public void ErzeugeSpielerRunde(List<string> mitspieler)
        {
            X01Mitspieler = new List<X01Spieler>();
            foreach (string item in mitspieler)
            {
                X01Spieler spieler = new X01Spieler(item, StartScore, 0);
                X01Mitspieler.Add(spieler);
            }
        }

        public void NeueSpielerListe(List<string> mitspieler)
        {
            Reset();

            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<X01Zustand>();
            Zustaende.Add(new X01Zustand() { AnzahlWuerfe = AnzahlWuerfe, ScoreRunde = ScoreRunde, SpielerDran = SpielerDran, Wurf1Score = Wurf1Score, Wurf2Score = Wurf2Score, Wurf3Score = Wurf3Score });

        }

        public void ZeichneGrids() {
            DartBoard.Set(isEnabledDartBoard, AnzahlWuerfe, Zustaende.Count > 1, isVisiblBtnNoScore);
            Wurfanzeige.Set(X01Mitspieler[SpielerDran].SpielerName, Wurf1Score, Wurf2Score, Wurf3Score, ScoreRunde.ToString(), StartScore.ToString(), AnzahlWuerfe == 3 || isVisibleBtnfertigWinner);
            ZeichneGridTabelle();
        }

        public void ZeichneGridTabelle()
        {
            Tabelle.GrdMain.Children.Clear();
            int counter = 0;
            foreach (X01Spieler spieler in X01Mitspieler)
            {
                SetGrid(Tabelle.GrdMain, spieler.SpielerName, 0, counter, 3, 2);
                SetGrid(Tabelle.GrdMain, spieler.Score.ToString(), 3, counter, 1, 2);
                SetGrid(Tabelle.GrdMain, spieler.Siege.ToString(), 4, counter, 1, 2);

                counter += 2;
            }
            Tabelle.SetFonts();
        }
    }
}
