using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class X01Spieler :  MainSpieler
    {
        public int Score { get; set; }
        public int Siege { get; set; }

        public X01Spieler(string name, int score, int siege) {
            SpielerName = name;
            Score = score;
            Siege = siege;
        }
    }
}
