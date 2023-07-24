public sealed class Host
{
    internal Host( string host, int port )
    {
        Hostname = host;
        Port = port;
    }

    public string Hostname { get; }
    public int Port { get; }

    public override string ToString()
        => $"{Hostname}:{Port}";

    internal static Host Empty { get; } = new Host( string.Empty, 0 );

    public static Host Parse( string value )
    {
        var parsed = value.Split( ':' );

        if ( parsed.Length != 2 )
        {
            throw new FormatException( "Host must be in the form of host:port" );
        }

        var host = parsed[0];
        var port = int.Parse( parsed[1] );

        return new Host( host, port );
    }
}
