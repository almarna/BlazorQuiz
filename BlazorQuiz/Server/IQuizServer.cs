using System.Collections.Generic;
using WampSharp.V2.Rpc;

namespace BlazorQuiz.Services
{
    public interface IQuizServer
    {
        [WampProcedure("com.arguments.startQuiz")]
        void StartQuiz();

        [WampProcedure("com.arguments.addPlayer")]
        void AddPlayer(string playerName);

        [WampProcedure("com.arguments.getPlayers")]
        IEnumerable<string> getPlayers();

        [WampProcedure("com.arguments.answer")]
        void Answer(string playerName, string answer);
    }
}