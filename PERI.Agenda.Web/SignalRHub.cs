using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Agenda.Web
{
    public class SignalRHub : Hub<ITypedHubClient>
    {
        /// <summary>
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
