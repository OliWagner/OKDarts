using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkDarts.Classes
{
    public class EliminationSpieler : MainSpieler
    {
        public int Score { get; set; }
        
        public EliminationSpieler(string name)
        {
            SpielerName = name;
            Reset();
        }

        public void Reset()
        {
            Score = 0;
        }

        public EliminationSpieler(string name, int score)
        {
            SpielerName = name;
            Score = score;
        }
    }
}
