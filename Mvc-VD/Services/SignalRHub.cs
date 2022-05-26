using Microsoft.AspNet.SignalR.Client;
using System;

namespace Mvc_VD.Services
{
    public interface ISignalRHub
    {
        IHubProxy GetHub();
    }

    public class SignalRHub : ISignalRHub
    {
        private static readonly HubConnection connection = new HubConnection(Extension.GetAppSetting("Realtime"));
        private static IHubProxy hub;
        public SignalRHub()
        {
            hub= connection.CreateHubProxy("shinsungHub");
            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();
        }
        public IHubProxy GetHub()
        {
            return hub;
        }
    }
}