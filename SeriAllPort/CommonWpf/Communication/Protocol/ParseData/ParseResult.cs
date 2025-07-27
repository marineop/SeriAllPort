namespace CommonWpf.Communication.Protocol.ParseData
{
    internal enum ParseResult
    {
        Unknown,
        ErrorPacket,
        FullPacket,
        WaitForFullPacket
    }
}
