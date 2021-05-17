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
    public class SplitScore : MainSpiel, IMainSpiel
    {
        //Für das Spiel benötigte Variablen
        List<SplitScoreSpieler> SplitScoreMitspieler;
        List<SplitScoreZustand> Zustaende = new List<SplitScoreZustand>();
        List<string> Wurfrunden = new List<string>() { "15", "16", "D", "17", "18", "T", "19", "20", "B" };
        List<int> Wurfwerte = new List<int>() { 15, 16, 0, 17, 18, 0, 19, 20, 25 };
        bool Getroffen = false;


        public SplitScore(List<string> mitspieler, UcWurfAnzeige wurfanzeige, UcTabelle tabelle, UcDartBoard dartBoard) : base(mitspieler, wurfanzeige, tabelle, dartBoard) {
            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<SplitScoreZustand>();
            Zustaende.Add(new SplitScoreZustand(0, 40, "", "", "", Getroffen));

            SetEvents();
        }

        public void DartBoard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label Sender = (Label)sender;
            string wurfwert = Sender.Tag.ToString();
            
            BewerteWurf(wurfwert);

            SetZustand(wurfwert);

            if (AnzahlWuerfe == 3 && !Getroffen)
            {
                SplitScoreMitspieler[SpielerDran].Score /= 2;
            }

            //Checken, ob das Spiel vorbei ist
            if (AnzahlWuerfe == 3 && AnzahlRunden == 8 && SpielerDran == SplitScoreMitspieler.Count() - 1) {
                Wurfanzeige.BtnFertig.Content = "NEU!";
            }

            ZeichneGrids();
        }

        private void BewerteWurf(string wurf) {
            string[] werte = wurf.Split(' ');

            string strSDT = werte[0];
            int multiplikator = 0;
            if (strSDT.Equals("S")) { multiplikator = 1; }
            if (strSDT.Equals("D")) { multiplikator = 2; }
            if (strSDT.Equals("T")) { multiplikator = 3; }
            string strWurfwert = werte[1];
            if (AnzahlRunden == 2) {
                //Es muss ein Doppel geworfen werden
                if (strSDT.Equals("D")) {
                    SplitScoreMitspieler[SpielerDran].Score += multiplikator * Int32.Parse(strWurfwert);
                    Getroffen = true;
                }
            }
            else if (AnzahlRunden == 5) {
                //Triple
                if (strSDT.Equals("T"))
                {
                    SplitScoreMitspieler[SpielerDran].Score += multiplikator * Int32.Parse(strWurfwert);
                    Getroffen = true;
                }
            }
            else { 
                if (strWurfwert.Equals(Wurfrunden[AnzahlRunden])) {
                    SplitScoreMitspieler[SpielerDran].Score += multiplikator * Wurfwerte[AnzahlRunden];
                    Getroffen = true;
                }
            }
        }

        public void DartBoardBtnBack_Click(object sender, RoutedEventArgs e)
        {
            //isEnabledDartBoard = true;
            SetButtons(true, isVisibleBtnfertigWinner, isVisiblBtnNoScore);
            SplitScoreZustand zustand = Zustaende[Zustaende.Count - 2];
            Zustaende = new List<SplitScoreZustand>();
            Zustaende.Add(zustand);

            AnzahlWuerfe = zustand.AnzahlWuerfe;
            Wurf1Score = zustand.Wurf1Score;
            Wurf2Score = zustand.Wurf2Score;
            Wurf3Score = zustand.Wurf3Score;
            SplitScoreMitspieler[SpielerDran].Score = zustand.LastScore;
            Getroffen = zustand.Getroffen;

            ZeichneGrids();
        }

        public void DartBoardBtnNoScore_Click(object sender, RoutedEventArgs e)
        {
            SetZustand("0");
            ZeichneGrids();
        }

        public void WurfAnzeigeBtnFertig_Click(object sender, RoutedEventArgs e)
        {
            //isEnabledDartBoard = true;
            SetButtons(true, isVisibleBtnfertigWinner, isVisiblBtnNoScore);
            AnzahlWuerfe = 0;
            Getroffen = false;

            if (Wurfanzeige.BtnFertig.Content.Equals("Weiter"))
            {
                NextSpieler();
            }
            else
            {

                foreach (SplitScoreSpieler spieler in SplitScoreMitspieler)
                {
                    spieler.Score = 40;
                }
                NextRunde();
                AnzahlRunden = 0;
                Wurfanzeige.BtnFertig.Content = "Weiter";
            }

            Wurf1Score = "";
            Wurf2Score = "";
            Wurf3Score = "";
            AnzahlWuerfe = 0;
            Zustaende = new List<SplitScoreZustand>();
            Zustaende.Add(new SplitScoreZustand(AnzahlWuerfe, SplitScoreMitspieler[SpielerDran].Score, Wurf1Score, Wurf2Score, Wurf3Score, Getroffen));
            ZeichneGrids();
        }

        public void ErzeugeSpielerRunde(List<string> mitspieler)
        {
            SplitScoreMitspieler = new List<SplitScoreSpieler>();
            foreach (string item in mitspieler)
            {
                SplitScoreSpieler spieler = new SplitScoreSpieler(item);
                SplitScoreMitspieler.Add(spieler);
            }
        }

        public void NeueSpielerListe(List<string> mitspieler)
        {
            Reset();

            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<SplitScoreZustand>();
            Zustaende.Add(new SplitScoreZustand(0, 40, "", "", "", Getroffen));
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
                //isEnabledDartBoard = false;
                SetButtons(false, isVisibleBtnfertigWinner, isVisiblBtnNoScore);
            }
            Zustaende.Add(new SplitScoreZustand(AnzahlWuerfe, SplitScoreMitspieler[SpielerDran].Score, Wurf1Score, Wurf2Score, Wurf3Score, Getroffen));
            
        }

        public void ZeichneGrids()
        {
            DartBoard.Set(isEnabledDartBoard, AnzahlWuerfe, Zustaende.Count > 1, true);
            Wurfanzeige.Set(SplitScoreMitspieler[SpielerDran].SpielerName + "  " + Wurfrunden[AnzahlRunden], Wurf1Score, Wurf2Score, Wurf3Score, SplitScoreMitspieler[SpielerDran].Score.ToString(), "SplitScore", AnzahlWuerfe == 3);
            ZeichneGridTabelle();
        }

        public void ZeichneGridTabelle()
        {
            Tabelle.GrdMain.Children.Clear();
            int counter = 0;
            foreach (SplitScoreSpieler spieler in SplitScoreMitspieler)
            {
                SetGrid(Tabelle.GrdMain, spieler.SpielerName, 0, counter, 4, 2);
                SetGrid(Tabelle.GrdMain, spieler.Score.ToString(), 4, counter, 1, 2);
                
                counter += 2;
            }
            Tabelle.SetFonts();
        }
    }
}
