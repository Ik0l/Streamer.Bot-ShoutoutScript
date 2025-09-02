using Streamer.bot.Plugin.Interface;
using Streamer.bot.Plugin.Interface.Enums;

public class CPHInline : CPHInlineBase
{
    private const string NicePeople = "Nice people";
    private const string ShoutoutQueue = "Nice people queue";
    
    private bool _shoutoutAvailable;
    
    public void Init()
    {
        CPH.SendMessage("Готов отмечать людей!");
        CPH.ClearUsersFromGroup(ShoutoutQueue);
        _shoutoutAvailable = false;
    }
    
    public void Dispose()
    {
        CPH.SendMessage("Не готов отмечать людей");
    }

    public bool Execute()
    {
        return true;
    }
    
    public bool TryAddToShoutoutQueue()
    {
        CPH.TryGetArg("user", out string user);
        if (CPH.UserInGroup(user, Platform.Twitch, NicePeople))
        {
            CPH.AddUserToGroup(user, Platform.Twitch, ShoutoutQueue);
        }
        
        return true;
    }

    public bool DoShoutout()
    {
        if (!_shoutoutAvailable)
        {
            return true;
        }
        
        var queue = CPH.UsersInGroup(ShoutoutQueue);
        if (queue.Count == 0)
        {
            return true;
        }

        CPH.TwitchSendShoutoutByLogin(queue[0].Login);
        CPH.RemoveUserFromGroup(queue[0].Login, Platform.Twitch, ShoutoutQueue);

        return true;
    }

    public bool EnableShoutout()
    {
        _shoutoutAvailable = true;
        return true;
    }

    public bool DisableShoutout()
    {
        _shoutoutAvailable = false;
        return true;
    }
}
