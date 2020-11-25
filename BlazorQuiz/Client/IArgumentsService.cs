using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace BlazorQuiz.Client
{
    public interface IArgumentsService
    {
        [WampProcedure("com.arguments.startQuiz")]
        Task StartQuiz();

        [WampProcedure("com.arguments.addPlayer")]
        Task AddPlayer(string playerName);

        [WampProcedure("com.arguments.getPlayers")]
        Task<IEnumerable<string>> GetPlayers();

        [WampProcedure("com.arguments.answer")]
        Task Answer(string playerName, string answer);
    }
}