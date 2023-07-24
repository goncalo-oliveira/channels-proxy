using Faactory.Channels;
using Microsoft.Extensions.Logging;

internal sealed class ProxyChannelService : ChannelService
{
    private readonly ILogger logger;
    private readonly IClientChannelFactory channelFactory;
    private readonly ProxyConnections connections;
    private IChannel? proxyChannel = null;

    public ProxyChannelService( ILoggerFactory loggerFactory, IClientChannelFactory clientChannelFactory, ProxyConnections proxyConnections )
    {
        logger = loggerFactory.CreateLogger<ProxyChannelService>();
        channelFactory = clientChannelFactory;
        connections = proxyConnections;
    }

    public override Task StartAsync( IChannel channel, CancellationToken cancellationToken )
    {
        if ( channel.IsClientChannel() )
        {
            return Task.CompletedTask;
        }

        logger.LogInformation( $"Proxy service {channel.Id} started" );

        return base.StartAsync(channel, cancellationToken);
    }

    public override async Task StopAsync( CancellationToken cancellationToken )
    {
        await base.StopAsync( cancellationToken );

        if ( proxyChannel != null )
        {
            await proxyChannel.CloseAsync();
        }
    }

    protected override async Task ExecuteAsync( CancellationToken cancellationToken )
    {
        // keep track of the source channel
        connections.Add( Channel!.Id, Channel );

        while ( !cancellationToken.IsCancellationRequested )
        {
            if ( Channel.IsClientChannel() )
            {
                break;
            }

            await Task.Delay( 1000, cancellationToken );
        }
    }

    public async Task<IChannel> GetProxyAsync()
    {
        if ( ( proxyChannel == null ) )
        {
            proxyChannel = await channelFactory.CreateAsync();

            // link the proxy channel to the source channel
            proxyChannel.SetSourceChannelId( Channel!.Id );
        }

        return ( proxyChannel );
    }
}
