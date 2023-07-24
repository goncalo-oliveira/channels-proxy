using System.CommandLine;

var rootCommand = new RootCommand( "A simple TCP proxy built on top of Channels." );

var portOption = new Option<int>(
    new string[]
    {
        "--port",
        "-p"
    },
    getDefaultValue: () => 8080,
    "Listening port"
);

var hostArg = new Argument<Host>(
    name: "host",
    description: "Target host to proxy to",
    parse: result =>
    {
        var value = result.Tokens.Single().Value;

        try
        {
            return Host.Parse( value );
        }
        catch ( FormatException )
        {
            result.ErrorMessage = "Host must be in the form of host:port";

            return Host.Empty;
        }
    }
);

rootCommand.Add( hostArg );
rootCommand.Add( portOption );

var proxy = new Proxy();

rootCommand.SetHandler(
    ( host, listenPort ) => proxy.Run( host, listenPort ),
    hostArg,
    portOption
);

rootCommand.Invoke( args );
