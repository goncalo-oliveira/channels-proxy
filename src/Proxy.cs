using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal sealed class Proxy
{
    public Proxy()
    {}

    public void Run( Host host, int listenPort )
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();

        builder.Services.AddSingleton<ProxyConnections>();

        builder.Services.AddChannels( channel =>
        {
            channel.Configure( options =>
            {
                options.Port = listenPort;
                options.Backlog = 100;
            } );

            channel.AddInputHandler<ProxyChannelHandler>();
            channel.AddChannelService<ProxyChannelService>();
            channel.AddIdleChannelService();
        } );

        builder.Services.AddClientChannelFactory( channel =>
        {
            channel.Configure( options =>
            {
                options.Host = host.Hostname;
                options.Port = host.Port;
            } );

            channel.AddInputHandler<TargetChannelHandler>();
        } );

        var app = builder.Build();

        app.Run();
    }
}
