using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Hubs
{
    public class ChatHub:Hub
    {
        public  async Task SendMessage(string name, string message)
        {
          await  Clients.All.SendAsync("ReceiveMessage", name, message);
        }
    }
}
