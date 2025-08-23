using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ms_evva_core.Hubs
{
    public class HostHub : Hub
    {
        public async Task JoinHostGroup(string uniqueId)
        {
            Console.WriteLine($"[HostHub] JoinHostGroup invoked for uniqueId: {uniqueId} by ConnectionId: {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, uniqueId);
            Console.WriteLine($"Client {Context.ConnectionId} joined group {uniqueId}.");
        }

        public async Task LeaveHostGroup(string uniqueId)
        {
            Console.WriteLine($"[HostHub] LeaveHostGroup invoked for uniqueId: {uniqueId} by ConnectionId: {Context.ConnectionId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, uniqueId);
            Console.WriteLine($"Client {Context.ConnectionId} left group {uniqueId}.");
        }

        public async Task SendHostData(string uniqueId, object data)
        {
            Console.WriteLine($"[HostHub] SendHostData invoked for uniqueId: {uniqueId}");
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                base.Clients.Group(uniqueId).SendAsync("ReceiveHostData", data);
           

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing data from host {uniqueId}: {ex.Message}");
            }
            await Clients.Group(uniqueId).SendAsync("ReceiveHostData", data);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"[HostHub] OnDisconnectedAsync invoked for ConnectionId: {Context.ConnectionId}");
            // You might need to handle group cleanup if a client disconnects unexpectedly.
            // This could involve a mapping of ConnectionId to the groups it has joined.
            // For simplicity, we'll leave this for now.
            await base.OnDisconnectedAsync(exception);
        }
    }
}