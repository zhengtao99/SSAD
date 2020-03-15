using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [Serializable]
    public class Highscore
    {
        public User User;
        public int TotalScore;
    }
}
