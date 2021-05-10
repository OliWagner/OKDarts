using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class X01Zustand
    {
        public int AnzahlWuerfe { get; set; }
        public string Wurf1Score { get; set; }
        public string Wurf2Score { get; set; }
        public string Wurf3Score { get; set; }
        public int ScoreRunde { get; set; }

        public int SpielerDran { get; set; }
    }
}
