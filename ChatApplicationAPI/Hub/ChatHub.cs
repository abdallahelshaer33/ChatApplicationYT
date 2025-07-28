using Microsoft.AspNetCore.SignalR;

namespace ChatApplicationAPI.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IDictionary<string, UserRoomConnection> _conn;

    public ChatHub(IDictionary<string, UserRoomConnection> conn)
    {
        _conn = conn;
    }


    public async Task joinRoom(UserRoomConnection userconn)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userconn.Room!);
        _conn[Context.ConnectionId] = userconn;
        await Clients.Group(userconn.Room)
        .SendAsync("ReceiveMessage", "Abdallah elshaer ", $"{userconn.user} has joined to group" , DateTime.Now);
        await SendConnectedUser(userconn.Room!);


    }
    public async Task SendMessage(string message)
    {
        if (_conn.TryGetValue(Context.ConnectionId, out UserRoomConnection user))
        {
            await Clients.Group(user.Room!).SendAsync("ReceiveMessage", user.user, message, DateTime.Now);
        }
    }
    public override Task OnDisconnectedAsync(Exception? exc)
    {
        if (!_conn.TryGetValue(Context.ConnectionId, out UserRoomConnection userroom))
        {
            return base.OnDisconnectedAsync(exc);
        }
        Clients.Group(userroom.Room!).SendAsync("ReceiveMessage", "Abdallah elshaer", $"{userroom.user} has left the Group",DateTime.Now);
        SendConnectedUser(userroom.Room!);
        return base.OnDisconnectedAsync(exc);
    }

    public Task SendConnectedUser(string room)
    {
        var users = _conn.Values
        .Where(u => u.Room == room)
        .Select(s => s.user);
        return Clients.Group(room).SendAsync("connectedUser", users);
    }
}