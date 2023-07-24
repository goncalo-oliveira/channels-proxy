using System.Collections.Concurrent;
using Faactory.Channels;
using Microsoft.Extensions.Logging;

internal sealed class ProxyConnections : IChannelEvents
{
    private readonly ILogger logger;
    private readonly ConcurrentDictionary<string, IChannel> connections = new ConcurrentDictionary<string, IChannel>();

    public ProxyConnections( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<ProxyConnections>();
    }

    public void Add( string key, IChannel channel )
    {
        connections.TryAdd( key, channel );

        logger.LogInformation( $"Saved channel {key}" );
    }

    public IChannel? Get( string key )
    {
        if ( !connections.TryGetValue( key, out var channel ) )
        {
            return ( null );
        }

        return ( channel );
    }

    public void ChannelClosed( IChannelInfo channelInfo )
    {
        connections.TryRemove( channelInfo.Id, out _ );

        logger.LogInformation( $"Removed channel {channelInfo.Id}" );
    }

    public void ChannelCreated(IChannelInfo channelInfo)
    { }

    public void CustomEvent(IChannelInfo channelInfo, string name, object? data)
    { }

    public void DataReceived(IChannelInfo channelInfo, byte[] data)
    { }

    public void DataSent(IChannelInfo channelInfo, int sent)
    { }
}
