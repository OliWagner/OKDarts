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
    public class Elimination : MainSpiel, IMainSpiel
    {
        private List<EliminationSpieler> EliminationMitspieler;

        //Müsste hier eigentlich Zielscore heissen
        public int StartScore = 0;
        private int ScoreRunde = 0;
        private List<EliminationZustand> Zustaende = new List<EliminationZustand>();


        public Elimination(List<string> mitspieler, UcWurfAnzeige wurfanzeige, UcTabelle tabelle, UcDartBoard dartBoard, int startScore) : base(mitspieler, wurfanzeige, tabelle, dartBoard)
        {
            StartScore = startScore;
            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<EliminationZustand>();
            Zustaende.Add(new EliminationZustand(EliminationMitspieler, AnzahlWuerfe, SpielerDran, ScoreRunde, Wurf1Score, Wurf2Score, Wurf3Score));
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

        private int BerechneWurf(string wurf)
        {
            string[] Wurf = wurf.Split(' ');
            string mult = Wurf[0];
            int multiplikator = 1;
            if (mult.Equals("D")) { multiplikator = 2; }
            if (mult.Equals("T")) { multiplikator = 3; }
            string val = Wurf[1];
            int value = 0;
            if (val.Equals("B"))
            {
                value = 25;
            }
            else
            {
                value = int.Parse(val);
            }
            return multiplikator * value;
        }

        public bool CheckWurfSetWerte(string wurfwert)
        {
            int wurfValue = BerechneWurf(wurfwert);
            //Zuerst checken, ob einer oder mehrere Spieler auf 0 gesetzt werden
            foreach (EliminationSpieler item in EliminationMitspieler)
            {
                if (item.Score == EliminationMitspieler[SpielerDran].Score + wurfValue + ScoreRunde) {
                    item.Score = 0;
                }
            }

            //Fall 1 --> Spieler hat gewonnen
            if (EliminationMitspieler[SpielerDran].Score + wurfValue + ScoreRunde == StartScore)
            {
                Wurfanzeige.BtnFertig.Content = "Neue Runde";
                isVisibleBtnfertigWinner = true;
                isEnabledDartBoard = false;
                isVisiblBtnNoScore = false;
                SetZustand(wurfwert);
                ZeichneGrids();
                return false;
            }

            //Fall 2 --> Spieler hat sich überworfen
            if (EliminationMitspieler[SpielerDran].Score + wurfValue + ScoreRunde > StartScore)
            {
                ScoreRunde = 0;
                isVisibleBtnfertigWinner = true;
                isEnabledDartBoard = false;
                isVisiblBtnNoScore = false;
                SetZustand(wurfwert);
                ZeichneGrids();
                return false;
            }

            //Fall 3 --> Wurf wird vom Score des Spielers abgezogen
            ScoreRunde += wurfValue;
            if (AnzahlWuerfe == 3)
            {
                Wurfanzeige.BtnFertig.Visibility = Visibility.Visible;
                isEnabledDartBoard = false;
                Wurfanzeige.BtnFertig.Visibility = Visibility.Visible;
            }
            return true;
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
            if (AnzahlWuerfe == 3)
            {
                isEnabledDartBoard = false;
                isVisiblBtnNoScore = false;
            }
            Zustaende.Add(new EliminationZustand(EliminationMitspieler, AnzahlWuerfe, SpielerDran, ScoreRunde, Wurf1Score, Wurf2Score, Wurf3Score));
        }

        public void WurfAnzeigeBtnFertig_Click(object sender, RoutedEventArgs e)
        {
            isVisibleBtnfertigWinner = false;
            isEnabledDartBoard = true;
            isVisiblBtnNoScore = true;

            EliminationMitspieler[SpielerDran].Score += ScoreRunde;

            if (Wurfanzeige.BtnFertig.Content.Equals("Weiter"))
            {
                NextSpieler();
            }
            else
            {
                
                foreach (EliminationSpieler spieler in EliminationMitspieler)
                {
                    spieler.Reset();
                }
                NextRunde();
                Wurfanzeige.BtnFertig.Content = "Weiter";
            }

            ScoreRunde = 0;
            Wurf1Score = "";
            Wurf2Score = "";
            Wurf3Score = "";
            AnzahlWuerfe = 0;
            Zustaende = new List<EliminationZustand>();
            Zustaende.Add(new EliminationZustand(EliminationMitspieler, AnzahlWuerfe, SpielerDran, ScoreRunde, Wurf1Score, Wurf2Score, Wurf3Score));
            ZeichneGrids();
        }

        public void DartBoardBtnBack_Click(object sender, RoutedEventArgs e)
        {
            isEnabledDartBoard = true;
            Wurfanzeige.BtnFertig.Content = "Weiter";
            isVisibleBtnfertigWinner = false;

            EliminationZustand zustand = Zustaende[Zustaende.Count - 2];

            foreach (EliminationSpieler es in EliminationMitspieler)
            {
                es.Score = zustand.MitSpieler.Where(x => x.SpielerName.Equals(es.SpielerName)).FirstOrDefault().Score;
            }

            Zustaende = new List<EliminationZustand>();
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
            EliminationMitspieler = new List<EliminationSpieler>();
            foreach (string item in mitspieler)
            {
                EliminationSpieler spieler = new EliminationSpieler(item);
                EliminationMitspieler.Add(spieler);
            }
        }

        public void NeueSpielerListe(List<string> mitspieler)
        {
            Reset();
            ErzeugeSpielerRunde(mitspieler);
            ZeichneGrids();
            //Der aktuelle Zustand muss zu Beginn einmal gesetzt werden
            Zustaende = new List<EliminationZustand>();
            Zustaende.Add(new EliminationZustand(EliminationMitspieler, AnzahlWuerfe, SpielerDran, ScoreRunde, Wurf1Score, Wurf2Score, Wurf3Score));
        }

        public void ZeichneGrids()
        {
            DartBoard.Set(isEnabledDartBoard, AnzahlWuerfe, Zustaende.Count > 1, true);
            Wurfanzeige.Set(EliminationMitspieler[SpielerDran].SpielerName, Wurf1Score, Wurf2Score, Wurf3Score, ScoreRunde.ToString(), "Elimination", AnzahlWuerfe == 3 || isVisibleBtnfertigWinner);
            ZeichneGridTabelle();
        }

        public void ZeichneGridTabelle()
        {
            Tabelle.GrdMain.Children.Clear();
            int counter = 0;
            foreach (EliminationSpieler spieler in EliminationMitspieler)
            {
                SetGrid(Tabelle.GrdMain, spieler.SpielerName, 0, counter, 4, 2);
                SetGrid(Tabelle.GrdMain, spieler.Score.ToString(), 5, counter, 1, 2);
                counter += 2;
            }
            Tabelle.SetFonts();
        }
    }
}
