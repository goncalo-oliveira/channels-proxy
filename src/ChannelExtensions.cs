using Faactory.Channels;

internal static class ChannelExtensions
{
    public static bool IsClientChannel( this IChannel channel )
        => channel.GetType().Name.Equals( "ClientChannel" );

    public static string GetSourceChannelId( this IChannel channel )
    {
        return ( channel.Data["source"] );
    }

    public static void SetSourceChannelId( this IChannel channel, string channelId )
    {
        channel.Data["source"] = channelId;
    }
}
