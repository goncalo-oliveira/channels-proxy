using Faactory.Channels;
using Faactory.Channels.Handlers;
using Microsoft.Extensions.Logging;

internal sealed class TargetChannelHandler : ChannelHandler<byte[]>
{
    private readonly ILogger logger;
    private readonly ProxyConnections connections;

    public TargetChannelHandler( ILoggerFactory loggerFactory, ProxyConnections proxyConnections )
    {
        logger = loggerFactory.CreateLogger<TargetChannelHandler>();
        connections = proxyConnections;
    }

    public override async Task ExecuteAsync( IChannelContext context, byte[] data )
    {
        if ( !context.Channel.IsClientChannel() )
        {
            return;
        }

        var source = connections.Get( context.Channel.GetSourceChannelId() );

        /*
        If the source channel no longer exists, we close the target channel.
        */
        if ( source == null )
        {
            await context.Channel.CloseAsync();
            return;
        }

        await source.WriteAsync( data );

        logger.LogInformation( $"Proxy {source.Id} <- {data.Length} byte(s)" );
    }
}
