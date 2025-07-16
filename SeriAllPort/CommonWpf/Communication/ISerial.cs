using CommonWpf.EventHandlers;
using System.IO;

namespace CommonWpf.Communication
{
    public interface ISerial
    {
        event ErrorEventHandler Error;
        event ConnectionStateChangedEventHandler ConnectionStateChanged;
        event EventHandler BytesReceived;

        string Name { get; }

        ConnectionState ConnectionState { get; }

        int ReadBytes(byte[] bytes, int offset, int capacity);

        void SendBytes(byte[] bytes);

        void SendBytes(byte[] bytes, int offset, int length);
    }
}
