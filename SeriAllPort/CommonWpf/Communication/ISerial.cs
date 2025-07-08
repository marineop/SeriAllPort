using CommonWpf.EventHandlers;
using System.IO;

namespace CommonWpf.Communication
{
    public interface ISerial
    {
        event ErrorEventHandler Error;
        event ConnectionStateChangedEventHandler ConnectionStateChanged;
        event BytesReceivedEventHandler BytesReceived;

        string Name { get; }

        ConnectionState ConnectionState { get; set; }

        void SendBytes(byte[] bytes);

        void SendBytes(byte[] bytes, int offset, int length);
    }
}
