using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class SplitScoreSpieler : MainSpieler
    {
        public int Score { get; set; }

        public SplitScoreSpieler(string name)
        {
            SpielerName = name;
            Score = 40;
        }
    }
}
