using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorQuiz.Services;
using WampSharp.V2;
using WampSharp.V2.Client;

namespace BlazorQuiz.Server
{
    public interface IBlazorQuizServer : IDisposable
    {
        Task Execute();
    }

    public class BlazorQuizServer: IBlazorQuizServer
    {
        private Task<IAsyncDisposable> registrationTask = null;

        public async Task Execute()
        {
            const string location = "ws://localhost:8080/ws";

            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampChannel channel = channelFactory.CreateJsonChannel(location, "realm1");

            Task openTask = channel.Open();

            await openTask.ConfigureAwait(false);

            IWampRealmProxy realm = channel.RealmProxy;

            IQuizServer instance = new QuizServer(realm.Services);

            registrationTask = realm.Services.RegisterCallee(instance);

            await registrationTask.ConfigureAwait(false);

            await Task.Yield();
        }

        public void Dispose()
        {
            registrationTask?.Dispose();
        }
    }
}
