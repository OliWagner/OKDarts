using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class CricketZustand
    {
        public int AnzahlWuerfe { get; set; }
        public string Wurf1Score { get; set; }
        public string Wurf2Score { get; set; }
        public string Wurf3Score { get; set; }
        public int SpielerDran { get; set; }
        public List<CricketSpieler> MitSpieler { get; set; }

        public CricketZustand(List<CricketSpieler> mitSpieler, int anzahlWuerfe, int spielerDran, string wurf1, string wurf2, string wurf3) {
            AnzahlWuerfe = anzahlWuerfe;
            SpielerDran = spielerDran;
            Wurf1Score = wurf1;
            Wurf2Score = wurf2;
            Wurf3Score = wurf3;

            MitSpieler = new List<CricketSpieler>();
            foreach (CricketSpieler spieler in mitSpieler)
            {
                MitSpieler.Add(new CricketSpieler(spieler.SpielerName, spieler.Score, spieler.Wuerfe15, spieler.Wuerfe16, spieler.Wuerfe17, spieler.Wuerfe18, spieler.Wuerfe19, spieler.Wuerfe20, spieler.WuerfeBull));
            }
        }
    }
}
