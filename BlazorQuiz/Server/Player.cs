using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorQuiz.Services
{
    public class Player
    {
        public string Name;
        public List<string> Answers = new List<string>();
        public int Position;
        public int CorrectAnswers;
    }
}