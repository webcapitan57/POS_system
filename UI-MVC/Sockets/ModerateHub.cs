using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace POC.Sockets
{
    public class ModerateHub : Hub
    {
        // Join group of signalr hub based on groupcode
        public void JoinGroup(string groupCode)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, groupCode);
        }

        public void LeaveGroup(string groupCode)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupCode);
        }
        
        public async Task SendDeliveries(int[] deliveryIds)
        {
            await Clients.Caller.SendAsync("SendDeliveries", deliveryIds);
        }
    }
}