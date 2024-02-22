using Microsoft.AspNetCore.SignalR.Client;



var connection = new HubConnectionBuilder()
    .WithUrl("ws://localhost:56993/commonedithub")
    .Build();
connection.On<UserMessage>("ReceiveMessage", ( userMessage) =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("-------------------------------------------------------------------------");
    Console.WriteLine($"{userMessage.Nickname} 说 ： {userMessage.Message}");
    Console.WriteLine("-------------------------------------------------------------------------");
    Console.ResetColor();

});

await connection.StartAsync();

if (connection.State == HubConnectionState.Connected)
{
    Console.WriteLine("连接成功是否加入修改？(Y/N)");
    var read1 = Console.ReadLine();
    if (read1.ToLower() == "y")
    {
        Random random = new Random();
        UserConnInfo userConnInfo = new UserConnInfo
        {
            GroupId ="654654a65SD46A5sd4a96SD746A5s4da65SD4A65",
            Nickname = $"用户-{random.Next(0, 1000)}"
        };
        await connection.InvokeAsync("JoinGroup", userConnInfo);

        while (true)
        {
            Console.WriteLine("输入聊天内容：");
            var setbody= Console.ReadLine();

            UserMessage userMessage = new UserMessage
            {
                GroupId = userConnInfo.GroupId,
                Message = setbody,
                Nickname = userConnInfo.Nickname
            };
            await connection.InvokeAsync("SendMessage", userMessage);

        }

    }
    else
    {
        await connection.StopAsync();
        Console.WriteLine("断开连接");
    }


}
else
{
    Console.WriteLine("连接服务端失败");

}







class UserConnInfo
{
    /// <summary>
    /// 分组id
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string Nickname { get; set; }
}

class UserMessage
{
    /// <summary>
    /// 分组id
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public object Message { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string Nickname { get; set; }
}