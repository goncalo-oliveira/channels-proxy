using Faactory.Channels;
using Faactory.Channels.Handlers;
using Microsoft.Extensions.Logging;

internal sealed class ProxyChannelHandler : ChannelHandler<byte[]>
{
    private readonly ILogger logger;
    public ProxyChannelHandler( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<ProxyChannelHandler>();
    }

    public override async Task ExecuteAsync( IChannelContext context, byte[] data )
    {
        if ( context.Channel.IsClientChannel() )
        {
            return;
        }

        var proxy = await context.Channel.GetRequiredService<ProxyChannelService>()
            .GetProxyAsync();

        /*
        if the proxy connection is closed, we force the client to reconnect.
        we need to do this, because we don't know whether the client needs to
        send login data or not. Potentially, this could be an option.
        */
        if ( proxy.IsClosed )
        {
            await context.Channel.CloseAsync();
            return;
        }

        await proxy.WriteAsync( data );

        logger.LogInformation( $"Proxy {context.Channel.Id} -> {data.Length} byte(s)" );
    }
}
