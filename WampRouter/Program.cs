using System;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace WampRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting WampRouter..");
            const string location = "ws://127.0.0.1:8080/ws";

            using (IWampHost host = new DefaultWampHost(location))
            {
//                IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");

                // Host WAMP application components

                host.Open();

                Console.Write($"Server is running on {location}. Press <enter> to exit. ");
                Console.ReadLine();
            }

            Console.WriteLine("WampRouter has stopped");
        }
    }
}
