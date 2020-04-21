using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [Serializable]
    public class ClearedStage
    {
        public int Stage;
        public int Score;
        public bool IsCleared;
    }
}
