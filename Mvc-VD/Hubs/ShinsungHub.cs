using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_VD.Hubs
{
    [HubName("shinsungHub")]
    public class ShinsungHub : Hub
    {
        public void Hello(string code)
        {
            Clients.All.hello(code);
        }
        /// <summary>
        /// Hàm Realtime thông báo ra số chuông trên TMIS
        /// </summary>
        /// <param name="count"></param>
        public void Bell(int count)
        {
            Clients.All.bell(count);
        }
        /// <summary>
        /// Hàm Realtime thông báo ra số chuông trên FGWMS
        /// </summary>
        /// <param name="total"></param>
        public void FGWMS(int total)
        {
            Clients.All.fGWMS(total);
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}