using OkDarts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfControlLibraryDarts;

namespace OkDarts.Classes
{
    public abstract class MainSpiel
    {
        public List<string> Mitspieler { get; set; }
        public UcWurfAnzeige Wurfanzeige { get; set; }
        public UcTabelle Tabelle { get; set; }
        public UcDartBoard DartBoard { get; set; }

        public bool isEnabledDartBoard = true;
        public bool isVisibleBtnfertigWinner = false;
        public bool isVisiblBtnNoScore = true;

        public int AnzahlRunden = 0;
        public int SpielerDran = 0;
        public int SpielerGestartet = 0;
        public int AnzahlWuerfe = 0;
        public string Wurf1Score = "";
        public string Wurf2Score = "";
        public string Wurf3Score = "";

        /// <summary>
        /// Setzt die benötigten Werte
        /// </summary>
        /// <param name="mitspieler">Liste der Mitspieler, kommt aus WinStart</param>
        /// <param name="wurfanzeige">Instanz von UcWurfanzeige</param>
        /// <param name="tabelle">Instanz von Tabelle</param>
        /// <param name="dartBoard">Instanz von Dartboard</param>
        public MainSpiel(List<string> mitspieler,UcWurfAnzeige wurfanzeige, UcTabelle tabelle ,UcDartBoard dartBoard) {
            AnzahlWuerfe = 0;
            Mitspieler = mitspieler;
            Wurfanzeige = wurfanzeige;
            Tabelle = tabelle;
            DartBoard = dartBoard;
        }

        /// <summary>
        /// Setzt die Variablen für das Spiel auf die Anfangswerte
        /// Wird bei Spielerwechsel benötigt
        /// Die Erzeugung der neuen Spielerliste erfogt in der abgeleiteten Klasse
        /// </summary>
        public void Reset() {
            AnzahlRunden = 0;
            AnzahlWuerfe = 0;
            SpielerGestartet = 0;
            SpielerDran = 0;
            Wurf1Score = "";
            Wurf2Score = "";
            Wurf3Score = "";
        }

        public void NextRunde()
        {
            if (SpielerGestartet < (Mitspieler.Count() - 1))
            {
                SpielerGestartet++;
            }
            else
            {
                SpielerGestartet = 0;
            }
            SpielerDran = SpielerGestartet;
        }

        public void NextSpieler()
        {
            if (SpielerDran < (Mitspieler.Count() - 1))
            {
                SpielerDran++;
            }
            else
            {
                SpielerDran = 0;
                AnzahlRunden++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ieDartBoard">Enables/Disables Dartboard</param>
        /// <param name="ivBtnFertigWinner">Shows/hides Button Fertig/Weiter</param>
        /// <param name="ivBtnNoScore">Shows/hides Button No Score</param>
        public void SetButtons(bool ieDartBoard, bool ivBtnFertigWinner, bool ivBtnNoScore) {
            isEnabledDartBoard = ieDartBoard;
            isVisibleBtnfertigWinner = ivBtnFertigWinner;
            isVisiblBtnNoScore = ivBtnNoScore;
    }

        public void SetGrid(Grid grid, string content, int column, int row, int columnspan, int rowspan)
        {
            Label element = new Label();
            element.Content = content;
            element.HorizontalAlignment = HorizontalAlignment.Center;
            element.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            Grid.SetColumnSpan(element, columnspan);
            Grid.SetRowSpan(element, rowspan);
            grid.Children.Add(element);
        }

        public void SetGrid(Grid grid, BitmapImage image, int column, int row, int columnspan, int rowspan)
        {
            Label element = new Label();
            element.Background = new ImageBrush(image);
            //element.Margin = new Thickness(0);
            //element.HorizontalAlignment = HorizontalAlignment.Center;
            //element.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            Grid.SetColumnSpan(element, columnspan);
            Grid.SetRowSpan(element, rowspan);
            grid.Children.Add(element);
        }
    }
}
