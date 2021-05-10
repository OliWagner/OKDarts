using OkDarts.Classes;
using OkDarts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlLibraryDarts;

namespace OkDarts
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IMainSpiel spiel;
        List<string> mitspieler = new List<string>();
        UcDartBoard DartBoard;
        UcWurfAnzeige WurfAnzeige;
        UcTabelle Tabelle;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGrids();
            DartBoard.Visibility = Visibility.Hidden;
            WurfAnzeige.Visibility = Visibility.Hidden;
            GrdTabelle.Visibility = Visibility.Hidden;
            //Mitspieler auswählen
            SizeChanged += MainWindow_SizeChanged;
            WinStart start = new WinStart();
            start.ShowDialog();
            mitspieler = start.Mitspieler;

        }

        private void InitializeGrids()
        {
            DartBoard = new UcDartBoard(this);
            Grid.SetColumn(DartBoard, 0);
            Grid.SetRow(DartBoard, 1);
            GrdDartBoard.Children.Add(DartBoard);

            WurfAnzeige = new UcWurfAnzeige(this);
            Grid.SetColumn(WurfAnzeige, 1);
            Grid.SetRow(WurfAnzeige, 2);
            Grid.SetColumnSpan(WurfAnzeige, 2);
            GrdWurfAnzeige.Children.Add(WurfAnzeige);

            Tabelle = new UcTabelle(this);
            Grid.SetColumn(Tabelle, 1);
            Grid.SetRow(Tabelle, 1);
            GrdTabelle.Children.Add(Tabelle);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DartBoard.Window_SizeChanged(sender, e);
            WurfAnzeige.Window_SizeChanged(sender, e);
            Tabelle.Window_SizeChanged(sender, e);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Tag.ToString().Equals("Spieler"))
            {
                WinStart start = new WinStart(mitspieler);
                start.ShowDialog();
                    if (spiel != null) {
                        spiel.NeueSpielerListe(mitspieler);
                    }
            }
            else {
                DartBoard.Visibility = Visibility.Visible;
                WurfAnzeige.Visibility = Visibility.Visible;
                GrdTabelle.Visibility = Visibility.Visible;
                Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Images/MainBg.png")));


                if (item.Tag.ToString().Equals("Close"))
                {
                    Close();
                }

                if (spiel != null)
                {
                    spiel.UnsetEvents();
                }

                if (item.Tag.ToString().Equals("101"))
                {
                    spiel = new X01(mitspieler, WurfAnzeige, Tabelle, DartBoard, 101);
                }

                if (item.Tag.ToString().Equals("301"))
                {
                    spiel = new X01(mitspieler, WurfAnzeige, Tabelle, DartBoard, 301);
                }

                if (item.Tag.ToString().Equals("501"))
                {
                    spiel = new X01(mitspieler, WurfAnzeige, Tabelle, DartBoard, 501);
                }

                if (item.Tag.ToString().Equals("701"))
                {
                    spiel = new X01(mitspieler, WurfAnzeige, Tabelle, DartBoard, 701);
                }

                if (item.Tag.ToString().Equals("901"))
                {
                    spiel = new X01(mitspieler, WurfAnzeige, Tabelle, DartBoard, 901);
                }

                if (item.Tag.ToString().Equals("Cricket"))
                {
                    spiel = new Cricket(mitspieler, WurfAnzeige, Tabelle, DartBoard);
                }

                if (item.Tag.ToString().Equals("SplitScore"))
                {
                    spiel = new SplitScore(mitspieler, WurfAnzeige, Tabelle, DartBoard);
                }

                if (item.Tag.ToString().Equals("Elimination 301"))
                {
                    spiel = new Elimination(mitspieler, WurfAnzeige, Tabelle, DartBoard, 301);
                }

                if (item.Tag.ToString().Equals("Elimination 501"))
                {
                    spiel = new Elimination(mitspieler, WurfAnzeige, Tabelle, DartBoard, 501);
                }
            }

            
        }
    }
}
