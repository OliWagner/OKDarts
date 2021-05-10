using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class SplitScoreZustand
    {
        public int AnzahlWuerfe { get; set; }
        public string Wurf1Score { get; set; }
        public string Wurf2Score { get; set; }
        public string Wurf3Score { get; set; }
        public int LastScore { get; set; }
        public bool Getroffen { get; set; }

        public SplitScoreZustand(int anzahlWuerfe, int lastScore, string wurf1Score, string wurf2Score, string wurf3Score, bool getroffen) {
            AnzahlWuerfe = anzahlWuerfe;
            LastScore = lastScore;
            Wurf1Score = wurf1Score;
            Wurf2Score = wurf1Score;
            Wurf3Score = wurf3Score;
            Getroffen = getroffen;
        }
    }
}
