using System;
using System.Threading.Tasks;
using WampSharp.V2;

namespace BlazorQuiz.Client
{
    public interface IWampService
    {
        Task Publish<T>(string topic, T content);
        Task<IDisposable> Subscribe<T>(string topic, Action<T> callback);
        Task<T> GetProxy<T>() where T : class;
    }
    public class WampService: IWampService
    {
        private static DefaultWampChannelFactory factory = new DefaultWampChannelFactory();
        private const string ServerAddress = "ws://127.0.0.1:8080/ws";
        private IWampChannel channel = null;

        public async Task<IWampRealmServiceProvider>GetService()
        {
            if (channel == null)
            {
                channel =  factory.CreateJsonChannel(ServerAddress, "realm1");

                await channel.Open().ConfigureAwait(false);
            }
            return channel.RealmProxy.Services;
        }

        public async Task<T> GetProxy<T>() where T:class
        {
            return (await GetService()).GetCalleeProxy<T>();
        }

        public async Task Publish<T>(string topic, T content)
        {
            (await GetService()).GetSubject<T>(topic).OnNext(content);
            await Task.Yield();
        }

        public async Task<IDisposable> Subscribe<T>(string topic, Action<T> callback)
        {
            IDisposable disposable = (await GetService()).GetSubject<T>(topic).Subscribe(callback);
            await Task.Yield();
            return disposable;
        }
    }
}