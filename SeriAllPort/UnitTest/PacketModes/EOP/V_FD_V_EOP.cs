using CommonWpf.Communication.PhysicalInterfaces;
using CommonWpf.Communication.Protocol.EventTypes;
using CommonWpf.Communication.Protocol.PacketFields;
using CommonWpf.Communication.Protocol.PacketModes;

namespace UnitTest.PacketModes.EOP
{
    [TestClass]
    public sealed class V_FD_V_EOP
    {
        public void Initialize(PacketModeEndOfPacketSymbol _packetMode, SimulatedSerial _simulatedSerial)
        {
            _packetMode.ReceiveBuffer = new byte[4096];
            _packetMode.IdleTimeoutMs = 0;
            _packetMode.Fields.Clear();

            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data1"));
            _packetMode.Fields.Add(PacketField.CreateFixedData("Delimiter", [0x2C]));
            _packetMode.Fields.Add(PacketField.CreateVariableLength("Data2"));
            _packetMode.Fields.Add(new EndOfPacketSymbol("EOP", [0x0D, 0x0A]));

            _packetMode.Validate();

            _packetMode.Serial = _simulatedSerial;
        }

        [TestMethod]
        public void NormalPacket()
        {
            PacketModeEndOfPacketSymbol packetMode = new PacketModeEndOfPacketSymbol();
            SimulatedSerial simulatedSerial = new SimulatedSerial();
            Initialize(packetMode, simulatedSerial);

            byte[] data1 = [0x00, 0x01];
            byte[] delimiter = [0x2C];
            byte[] data2 = [0xCC, 0xDD, 0xEE];
            byte[] eop = [0x0D, 0x0A];

            List<byte> bytes = [];
            bytes.AddRange(data1);
            bytes.AddRange(delimiter);
            bytes.AddRange(data2);
            bytes.AddRange(eop);

            simulatedSerial.SimulateReceive(bytes.ToArray());
            packetMode.BytesReceived();
            packetMode.ParsePackets(DateTime.Now, true);

            bool dequeueSuccess;

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out PacketEventType? eventType);
            Assert.IsTrue(dequeueSuccess);
            Assert.IsTrue(eventType is PacketReceived);
            if (eventType is PacketReceived packetReceived)
            {
                int index = 0;

                CollectionAssert.AreEqual(data1, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(delimiter, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(data2, packetReceived.PacketFields[index++].ActualData);
                CollectionAssert.AreEqual(eop, packetReceived.PacketFields[index++].ActualData);
            }

            dequeueSuccess = packetMode.EventQueue.TryDequeue(out eventType);
            Assert.IsFalse(dequeueSuccess);
        }
    }
}
