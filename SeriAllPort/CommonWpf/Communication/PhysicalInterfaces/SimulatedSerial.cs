using CommonWpf.EventHandlers;
using System.IO;

namespace CommonWpf.Communication.PhysicalInterfaces
{
    public class SimulatedSerial : ISerial
    {
        public event ErrorEventHandler? Error;
        public event ConnectionStateChangedEventHandler? ConnectionStateChanged;
        public event EventHandler? BytesReceived;

        public string Name => "Simulated Serial Device";

        public ConnectionState ConnectionState => ConnectionState.Connected;

        private readonly object _lock = new object();
        private readonly byte[] _receiveBuffer = new byte[4096];
        private int _receiveBufferLength = 0;

        public void SimulateReceive(byte[] bytes)
        {
            lock (_lock)
            {
                Buffer.BlockCopy(bytes, 0, _receiveBuffer, _receiveBufferLength, bytes.Length);
                _receiveBufferLength += bytes.Length;
            }

            BytesReceived?.Invoke(this, EventArgs.Empty);
        }

        public int ReadBytes(byte[] bytes, int offset, int capacity)
        {
            int copyLength = 0;
            lock (_lock)
            {
                copyLength = Math.Min(_receiveBufferLength, capacity);
                Buffer.BlockCopy(_receiveBuffer, 0, bytes, offset, copyLength);
                _receiveBufferLength -= copyLength;
            }

            return copyLength;
        }

        public void SendBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void SendBytes(byte[] bytes, int offset, int length)
        {
            throw new NotImplementedException();
        }
    }
}
