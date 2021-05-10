using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    class EliminationZustand
    {
        public int AnzahlWuerfe { get; set; }
        public string Wurf1Score { get; set; }
        public string Wurf2Score { get; set; }
        public string Wurf3Score { get; set; }
        public int SpielerDran { get; set; }
        public int ScoreRunde { get; set; }
        public List<EliminationSpieler> MitSpieler { get; set; }

        public EliminationZustand(List<EliminationSpieler> mitSpieler, int anzahlWuerfe, int spielerDran, int scoreRunde, string wurf1, string wurf2, string wurf3)
        {
            AnzahlWuerfe = anzahlWuerfe;
            SpielerDran = spielerDran;
            ScoreRunde = scoreRunde;
            Wurf1Score = wurf1;
            Wurf2Score = wurf2;
            Wurf3Score = wurf3;

            MitSpieler = new List<EliminationSpieler>();
            foreach (EliminationSpieler spieler in mitSpieler)
            {
                MitSpieler.Add(new EliminationSpieler(spieler.SpielerName, spieler.Score));
            }
        }
    }
}