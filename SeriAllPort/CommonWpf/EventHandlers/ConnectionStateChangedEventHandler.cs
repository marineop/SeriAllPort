using CommonWpf.Communication;

namespace CommonWpf.EventHandlers
{
    public delegate void ConnectionStateChangedEventHandler(object? sender, ConnectionStateChangedEventArgs e);

    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public ConnectionState ConnectionState { get; set; }

        public ConnectionStateChangedEventArgs(ConnectionState newConnectionState)
        {
            ConnectionState = newConnectionState;
        }
    }
}
