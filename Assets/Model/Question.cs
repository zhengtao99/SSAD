using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model
{
    [Serializable]
    public class Question
    {
         public int Id;
         public string Description;
         public List<Answer> Answers;
    }
}
