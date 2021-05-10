using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class CricketSpieler : MainSpieler
    {
        public int Score { get; set; }
        public int Wuerfe15 { get; set; }
        public int Wuerfe16 { get; set; }
        public int Wuerfe17 { get; set; }
        public int Wuerfe18 { get; set; }
        public int Wuerfe19 { get; set; }
        public int Wuerfe20 { get; set; }
        public int WuerfeBull { get; set; }

        public CricketSpieler(string name)
        {
            SpielerName = name;
            Reset();
        }

        public void Reset() {
            Score = 0;
            Wuerfe15 = 0;
            Wuerfe16 = 0;
            Wuerfe17 = 0;
            Wuerfe18 = 0;
            Wuerfe19 = 0;
            Wuerfe20 = 0;
            WuerfeBull = 0;
        }

        public CricketSpieler(string name, int score, int w15, int w16, int w17, int w18, int w19, int w20, int wb)
        {
            SpielerName = name;
            Score = score;
            Wuerfe15 = w15;
            Wuerfe16 = w16;
            Wuerfe17 = w17;
            Wuerfe18 = w18;
            Wuerfe19 = w19;
            Wuerfe20 = w20;
            WuerfeBull = wb;
        }
    }
}
