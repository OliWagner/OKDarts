using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OkDarts.Interfaces
{
    interface IMainSpiel
    {
        //Events
        /// <summary>
        /// Wird aufgerufen, nachdem der User den Wurf auf der Dartscheibe ausgewählt hat
        /// Hier startet die Spiellogik, von hier wird der Wurf berechnet, geprüft ob das Spiel zu Ende ist, überworfen usw...
        /// Dazu gibt es die Methoden SetZustand und CheckWurf
        /// </summary>
        /// <param name="sender"> Wert wird in (Label)sender.Tag übergeben</param>
        /// <param name="e"></param>
        void DartBoard_MouseDoubleClick(object sender, MouseButtonEventArgs e);

        
        /// <summary>
        /// Addiert eine aktuelle Instanz von Zustand zur Liste
        /// </summary>
        /// <param name="wurfwert"></param>
        void SetZustand(string wurfwert);


        /// <summary>
        /// Hier die Events der einzelnen Elemente an die Ereignishandler binden
        /// </summary>
        void SetEvents();


        /// <summary>
        /// Die KlickEvents müssen wieder entfernt werden
        /// </summary>
        void UnsetEvents();
        
        /// <summary>
        /// BtnFertig erscheint wenn der Spieler drei Pfeile geworfen hat. In der Folge sollte NextSpieler aufgerufen werden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WurfAnzeigeBtnFertig_Click(object sender, System.Windows.RoutedEventArgs e);

        /// <summary>
        /// Sollte sich der User bei der Eingabe vertan haben, muss er mit Klick auf diesen Button
        /// den Zustand vor dem letzten Pfeil wieder herstellen können
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DartBoardBtnBack_Click(object sender, System.Windows.RoutedEventArgs e);

        /// <summary>
        /// Ereignis für Click auf Button für Fehlwurf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DartBoardBtnNoScore_Click(object sender, System.Windows.RoutedEventArgs e);


        //Methoden
        /// <summary>
        /// Erzeugt die benötigten Spieler für das jeweilige Spiel
        /// </summary>
        /// <param name="mitspieler">List<string> aus WinStart</string></param>
        void ErzeugeSpielerRunde(List<string> mitspieler);

        /// <summary>
        /// Wird aufgerufen wenn sich die Spielerliste ändert. Alle Werte zurück setzen!
        /// </summary>
        /// <param name="mitspieler">Neue Liste der Mitspieler</param>
        void NeueSpielerListe(List<string> mitspieler);

        /// <summary>
        /// Setzt die Liste der Spieler auf ein neues Spiel
        /// Es startet der zweite Spieler der Vorrunde
        /// Ist in MainSpiel implementiert
        /// </summary>
        void NextRunde();

        /// <summary>
        /// Setzt die Liste der Mitspieler auf den nächsten Spieler
        /// Ist in MainSpiel implementiert
        /// </summary>
        void NextSpieler();

        /// <summary>
        /// Zeichnet DartBoard, Wurfanzeige und Tabelle,
        /// ruft dann ZeichneGridTabelle auf
        /// </summary>
        void ZeichneGrids();

        /// <summary>
        /// Befüllt die Tabelle mit den Werten aus der Liste der jeweiligen Spielertypen
        /// </summary>
        void ZeichneGridTabelle();

    }
}
